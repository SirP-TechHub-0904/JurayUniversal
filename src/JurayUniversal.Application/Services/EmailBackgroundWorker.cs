using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JurayUniversal.Application.Repository.NotifyRegister;

namespace JurayUniversal.Application.Services
{
    public class EmailBackgroundWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmailBackgroundWorker> _logger;

        public EmailBackgroundWorker(IServiceProvider serviceProvider, ILogger<EmailBackgroundWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessEmailsAsync();
                    await PruneOldLogsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in EmailBackgroundWorker.");
                }

                // Wait for 1 minute before checking again
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ProcessEmailsAsync()
        {
            using var rootScope = _serviceProvider.CreateScope();
            var tenantOptions = rootScope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<JurayUniversal.Infrastructure.TenantSupport.TenantConfigurationOptions>>().Value;
            var tenantProvider = rootScope.ServiceProvider.GetRequiredService<JurayUniversal.Infrastructure.TenantSupport.ITenantProvider>();

            var tenantsToProcess = tenantOptions.Tenants.Select(t => t.Name).ToList();
            tenantsToProcess.Insert(0, null);

            foreach (var tenantName in tenantsToProcess.Distinct())
            {
                var safeTenantName = (tenantName ?? "System").Replace(":", "_");
                using (Serilog.Context.LogContext.PushProperty("Tenant", safeTenantName))
                {
                    try
                    {
                        using (tenantProvider.BeginScope(tenantName))
                        {
                            using var scope = _serviceProvider.CreateScope();
                            var context = scope.ServiceProvider.GetRequiredService<DashboardDbContext>();
                            var zeptoMailService = scope.ServiceProvider.GetRequiredService<IZeptoMailService>();

                            var settings = await context.MisSettings.FirstOrDefaultAsync();
                            if (settings == null || string.IsNullOrWhiteSpace(settings.ZeptoMailApiKey))
                                continue; // ZeptoMail not configured for this tenant

                            int maxRetries = settings.EmailMaxRetries > 0 ? settings.EmailMaxRetries : 5;

                            // Get Pending or Failed emails that haven't exceeded retry limit
                            var emailsToSend = await context.MisNewsletterEmailLogs
                                .Where(x => (x.Status == JurayUniversal.Domain.Enum.Enum.EmailLogStatus.Pending || 
                                            x.Status == JurayUniversal.Domain.Enum.Enum.EmailLogStatus.Failed) && 
                                            x.RetryCount < maxRetries)
                                .OrderBy(x => x.SentDatetime)
                                .Take(50) // Process in batches of 50
                                .ToListAsync();

                            foreach (var email in emailsToSend)
                            {
                                email.RetryCount++;
                                
                                var result = await zeptoMailService.SendEmailAsync(email.EmailAddress, "Recipient", email.Subject, email.MailBody, Guid.NewGuid().ToString());

                                if (result.IsSuccess)
                                {
                                    email.Status = JurayUniversal.Domain.Enum.Enum.EmailLogStatus.Sent;
                                    email.ReceivedDatetime = DateTime.UtcNow;
                                    email.ErrorMessage = null;
                                }
                                else
                                {
                                    email.Status = JurayUniversal.Domain.Enum.Enum.EmailLogStatus.Failed;
                                    email.ErrorMessage = result.ErrorMessage ?? "Unknown ZeptoMail error.";
                                    _logger.LogWarning($"Failed to send email {email.Id} for tenant {tenantName}: {email.ErrorMessage}");
                                }

                                await context.SaveChangesAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed to process emails for tenant: {tenantName ?? "default"}");
                    }
                }
            }
        }

        private async Task PruneOldLogsAsync()
        {
            using var rootScope = _serviceProvider.CreateScope();
            var tenantOptions = rootScope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<JurayUniversal.Infrastructure.TenantSupport.TenantConfigurationOptions>>().Value;
            var tenantProvider = rootScope.ServiceProvider.GetRequiredService<JurayUniversal.Infrastructure.TenantSupport.ITenantProvider>();

            var tenantsToProcess = tenantOptions.Tenants.Select(t => t.Name).ToList();
            tenantsToProcess.Insert(0, null);

            foreach (var tenantName in tenantsToProcess.Distinct())
            {
                var safeTenantName = (tenantName ?? "System").Replace(":", "_");
                using (Serilog.Context.LogContext.PushProperty("Tenant", safeTenantName))
                {
                    try
                    {
                        using (tenantProvider.BeginScope(tenantName))
                        {
                            using var scope = _serviceProvider.CreateScope();
                            var context = scope.ServiceProvider.GetRequiredService<DashboardDbContext>();

                            int maxLogs = 10000;
                            DateTime cutoffDate = DateTime.UtcNow.AddDays(-30);

                            var totalCount = await context.MisNewsletterEmailLogs.CountAsync();

                            if (totalCount > maxLogs)
                            {
                                var oldLogs = await context.MisNewsletterEmailLogs
                                    .Where(x => x.SentDatetime < cutoffDate)
                                    .OrderBy(x => x.SentDatetime)
                                    .Take(1000)
                                    .ToListAsync();

                                if (oldLogs.Any())
                                {
                                    context.MisNewsletterEmailLogs.RemoveRange(oldLogs);
                                    await context.SaveChangesAsync();
                                    _logger.LogInformation($"Pruned {oldLogs.Count} old email logs for tenant {tenantName ?? "default"}.");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed to prune email logs for tenant: {tenantName ?? "default"}");
                    }
                }
            }
        }
    }
}
