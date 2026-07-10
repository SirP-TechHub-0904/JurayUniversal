using JurayUniversal.Application.Repository.NotifyRegister;
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
    public class SmsSchedulerBackgroundService : BackgroundService
    {
        private readonly ILogger<SmsSchedulerBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SmsSchedulerBackgroundService(ILogger<SmsSchedulerBackgroundService> logger, IServiceProvider serviceProvider)
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
                    _logger.LogError(ex, "Error occurred executing Scheduled SMS Campaigns.");
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
                                using (var scope = _serviceProvider.CreateScope())
                                {
                                    var context = scope.ServiceProvider.GetRequiredService<DashboardDbContext>();
                                    var smsService = scope.ServiceProvider.GetRequiredService<ISmsSendService>();

                                    // Get campaigns that are scheduled (and the time has come) or currently in Pending state
                                    var pendingCampaigns = await context.MisSmsCampaigns
                                        .Include(c => c.Recipients)
                                        .Where(c => (c.Status == DomainEnum.MisSmsCampaignStatus.Scheduled && c.ScheduledDate.HasValue && c.ScheduledDate.Value <= DateTime.UtcNow) ||
                                                    c.Status == DomainEnum.MisSmsCampaignStatus.Pending)
                                        .ToListAsync();

                                    foreach (var campaign in pendingCampaigns)
                                    {
                                        campaign.Status = DomainEnum.MisSmsCampaignStatus.Sending;
                                        await context.SaveChangesAsync();

                                        try
                                        {
                                            if (campaign.Recipients == null || !campaign.Recipients.Any())
                                            {
                                                campaign.Status = DomainEnum.MisSmsCampaignStatus.Failed;
                                                await context.SaveChangesAsync();
                                                continue;
                                            }

                                            // Collect active contacts
                                            var contacts = campaign.Recipients.Select(r => r.PhoneNumber).ToList();
                                            string commaSeparatedContacts = string.Join(",", contacts);

                                            if (string.IsNullOrEmpty(commaSeparatedContacts))
                                            {
                                                campaign.Status = DomainEnum.MisSmsCampaignStatus.Failed;
                                                await context.SaveChangesAsync();
                                                continue;
                                            }

                                            // Use gateway 2 for campaigns
                                            var sendResult = await smsService.SendCampaignSmsAsync(commaSeparatedContacts, campaign.Message ?? "", campaign.SenderId ?? "SMS");

                                            if (sendResult == SmsSendResult.Success)
                                            {
                                                campaign.Status = DomainEnum.MisSmsCampaignStatus.Completed;
                                                foreach(var rec in campaign.Recipients)
                                                {
                                                    rec.DeliveryStatus = DomainEnum.MisSmsDeliveryStatus.Sent;
                                                }
                                            }
                                            else if (sendResult == SmsSendResult.InsufficientKudiBalance)
                                            {
                                                // Keep it in Pending so it auto-retries when admin funds the Kudi account
                                                campaign.Status = DomainEnum.MisSmsCampaignStatus.Pending;
                                                // Delivery status remains Pending
                                            }
                                            else
                                            {
                                                campaign.Status = DomainEnum.MisSmsCampaignStatus.Failed;
                                                foreach (var rec in campaign.Recipients)
                                                {
                                                    rec.DeliveryStatus = DomainEnum.MisSmsDeliveryStatus.Failed;
                                                }
                                            }

                                            await context.SaveChangesAsync();
                                        }
                                        catch (Exception ex)
                                        {
                                            _logger.LogError(ex, $"Failed to process SMS campaign {campaign.Id} for tenant {tenantName}");
                                            campaign.Status = DomainEnum.MisSmsCampaignStatus.Failed;
                                            await context.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to process SMS for tenant: {tenantName ?? "default"}");
                        }
                    }
                }
            }
        }
    }
}

