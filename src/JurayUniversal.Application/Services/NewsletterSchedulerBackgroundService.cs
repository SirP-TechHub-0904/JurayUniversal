using JurayUniversal.Domain.Enum;
using DomainEnum = JurayUniversal.Domain.Enum.Enum;
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

namespace JurayUniversal.Application.Services
{
    public class NewsletterSchedulerBackgroundService : BackgroundService
    {
        private readonly ILogger<NewsletterSchedulerBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public NewsletterSchedulerBackgroundService(ILogger<NewsletterSchedulerBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessScheduledCampaignsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing Scheduled Newsletter Campaigns.");
                }

                // Wait 1 minute before polling again
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ProcessScheduledCampaignsAsync()
        {
            using (var rootScope = _serviceProvider.CreateScope())
            {
                var tenantOptions = rootScope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<JurayUniversal.Infrastructure.TenantSupport.TenantConfigurationOptions>>().Value;
                var tenantProvider = rootScope.ServiceProvider.GetRequiredService<JurayUniversal.Infrastructure.TenantSupport.ITenantProvider>();

                var tenantsToProcess = tenantOptions.Tenants.Select(t => t.Name).ToList();
                tenantsToProcess.Insert(0, null); // process default connection string as well

                foreach (var tenantName in tenantsToProcess.Distinct())
                {
                    var safeTenantName = (tenantName ?? "System").Replace(":", "_");
                    using (Serilog.Context.LogContext.PushProperty("Tenant", safeTenantName))
                    {
                        try
                        {
                            using (tenantProvider.BeginScope(tenantName))
                            {
                                using (var scope = _serviceProvider.CreateScope())
                                {
                                    var context = scope.ServiceProvider.GetRequiredService<DashboardDbContext>();

                                    var supersetting = await context.SuperSettings.FirstOrDefaultAsync();
                                    string baseUrl = "https://yourdomain.com";

                                    if (supersetting != null && !string.IsNullOrWhiteSpace(supersetting.CompanyWebsiteLink))
                                    {
                                        baseUrl = supersetting.CompanyWebsiteLink.Trim();
                                        if (!baseUrl.StartsWith("http://") && !baseUrl.StartsWith("https://"))
                                        {
                                            baseUrl = "https://" + baseUrl;
                                        }
                                        baseUrl = baseUrl.TrimEnd('/');
                                    }

                                    // Get campaigns that are scheduled or pending
                                    var pendingCampaigns = await context.MisNewsletterCampaigns
                                        .Include(c => c.Recipients)
                                        .Where(c => (c.Status == DomainEnum.MisNewsletterCampaignStatus.Scheduled && c.ScheduledDate.HasValue && c.ScheduledDate.Value <= DateTime.UtcNow) ||
                                                    c.Status == DomainEnum.MisNewsletterCampaignStatus.Pending)
                                        .ToListAsync();

                                    foreach (var campaign in pendingCampaigns)
                                    {
                                        campaign.Status = DomainEnum.MisNewsletterCampaignStatus.Sending;
                                        await context.SaveChangesAsync();

                                        try
                                        {
                                            if (campaign.Recipients == null || !campaign.Recipients.Any())
                                            {
                                                campaign.Status = DomainEnum.MisNewsletterCampaignStatus.Failed;
                                                await context.SaveChangesAsync();
                                                continue;
                                            }

                                            // Queue to EmailLog
                                            foreach (var recipient in campaign.Recipients)
                                            {
                                                if (recipient.DeliveryStatus == DomainEnum.MisNewsletterDeliveryStatus.Sent)
                                                    continue; // Skip already sent if this is a retry

                                                string personalizedBody = campaign.HtmlBody
                                                    .Replace("{{Name}}", recipient.Name ?? "Valued Member")
                                                    .Replace("{{Email}}", recipient.EmailAddress)
                                                    .Replace("{{UnsubscribeLink}}", $"{baseUrl}/Unsubscribe?token={recipient.TrackingGuid}");

                                                var log = new MisNewsletterEmailLog
                                                {
                                                    EmailAddress = recipient.EmailAddress,
                                                    Subject = campaign.Subject,
                                                    MailBody = personalizedBody + $"<img src=\"{baseUrl}/api/tracking/pixel/{recipient.TrackingGuid}\" width=\"1\" height=\"1\" />",
                                                    SentDatetime = DateTime.UtcNow,
                                                    Status = JurayUniversal.Domain.Enum.Enum.EmailLogStatus.Pending,
                                                    CampaignId = campaign.Id,
                                                    RetryCount = 0
                                                };
                                                context.MisNewsletterEmailLogs.Add(log);
                                                recipient.DeliveryStatus = DomainEnum.MisNewsletterDeliveryStatus.Sent; // Mark recipient processed so it isn't requeued
                                            }
                                            
                                            campaign.Status = DomainEnum.MisNewsletterCampaignStatus.Completed;
                                            await context.SaveChangesAsync();
                                        }
                                        catch (Exception ex)
                                        {
                                            _logger.LogError(ex, $"Failed to process newsletter campaign {campaign.Id} for tenant {tenantName}");
                                            campaign.Status = DomainEnum.MisNewsletterCampaignStatus.Failed;
                                            await context.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to process newsletter for tenant: {tenantName ?? "default"}");
                        }
                    }
                }
            }
        }
    }
}
