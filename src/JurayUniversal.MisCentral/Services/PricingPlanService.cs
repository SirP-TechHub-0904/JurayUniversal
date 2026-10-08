using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace JurayUniversal.MisCentral.Services
{
    public class PricingPlanItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public decimal MonthlyPrice { get; set; }
        public string Currency { get; set; } = "₦";
        public string BillingCycle { get; set; } = "month";
        public bool IsPopular { get; set; }
        public int SortOrder { get; set; } = 1;
        public bool IsActive { get; set; } = true;
        public List<string> FeaturesList { get; set; } = new();
        public string CtaText { get; set; } = "Get Started Now";
        public string CtaPlanKey { get; set; } = "Starter";
    }

    public interface IPricingPlanService
    {
        Task<List<PricingPlanItem>> GetAllPlansAsync(bool onlyActive = false);
        Task<PricingPlanItem?> GetPlanByIdAsync(string id);
        Task<PricingPlanItem> SavePlanAsync(PricingPlanItem plan);
        Task<bool> DeletePlanAsync(string id);
        Task<bool> TogglePopularAsync(string id);
        Task<bool> ToggleActiveAsync(string id);
    }

    public class PricingPlanService : IPricingPlanService
    {
        private readonly string _filePath;
        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        public PricingPlanService(IWebHostEnvironment env)
        {
            var dataDir = Path.Combine(env.ContentRootPath, "App_Data");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
            _filePath = Path.Combine(dataDir, "pricing_plans.json");
            EnsureSeedData();
        }

        private void EnsureSeedData()
        {
            if (!File.Exists(_filePath))
            {
                var seedList = new List<PricingPlanItem>
                {
                    new PricingPlanItem
                    {
                        Id = "plan_starter",
                        Name = "Starter Plan",
                        Subtitle = "Perfect for small schools, single branches & growing businesses",
                        MonthlyPrice = 150000,
                        Currency = "₦",
                        BillingCycle = "month",
                        IsPopular = false,
                        SortOrder = 1,
                        IsActive = true,
                        CtaText = "Get Started Now",
                        CtaPlanKey = "Starter",
                        FeaturesList = new List<string>
                        {
                            "100% Private Workspace",
                            "Up to 5 Features of Your Choice",
                            "Custom Web Portal Address",
                            "Daily Automatic Backups",
                            "Standard Email & WhatsApp Support"
                        }
                    },
                    new PricingPlanItem
                    {
                        Id = "plan_growth",
                        Name = "Growth Plan",
                        Subtitle = "Ideal for secondary schools, colleges & established businesses",
                        MonthlyPrice = 350000,
                        Currency = "₦",
                        BillingCycle = "month",
                        IsPopular = true,
                        SortOrder = 2,
                        IsActive = true,
                        CtaText = "Get Started Now",
                        CtaPlanKey = "Growth",
                        FeaturesList = new List<string>
                        {
                            "100% Private Dedicated Environment",
                            "Up to 10 Features of Your Choice",
                            "Custom Domain (portal.yourschool.com)",
                            "Online CBT Exams & E-Voting Portals",
                            "Priority 24/7 Phone & WhatsApp Support"
                        }
                    },
                    new PricingPlanItem
                    {
                        Id = "plan_enterprise",
                        Name = "Enterprise Plan",
                        Subtitle = "For large universities, hospital networks & group organizations",
                        MonthlyPrice = 750000,
                        Currency = "₦",
                        BillingCycle = "month",
                        IsPopular = false,
                        SortOrder = 3,
                        IsActive = true,
                        CtaText = "Contact Us",
                        CtaPlanKey = "Enterprise",
                        FeaturesList = new List<string>
                        {
                            "Maximum Speed & Unlimited Storage",
                            "All 16 Features Included",
                            "Unlimited Staff, Students & User Accounts",
                            "Dedicated Account Manager & Staff Training",
                            "Custom Institutional Branding & Mobile Setup"
                        }
                    }
                };

                var json = JsonSerializer.Serialize(seedList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
        }

        public async Task<List<PricingPlanItem>> GetAllPlansAsync(bool onlyActive = false)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!File.Exists(_filePath)) return new List<PricingPlanItem>();
                var json = await File.ReadAllTextAsync(_filePath);
                var items = JsonSerializer.Deserialize<List<PricingPlanItem>>(json) ?? new List<PricingPlanItem>();
                if (onlyActive) items = items.Where(x => x.IsActive).ToList();
                return items.OrderBy(x => x.SortOrder).ToList();
            }
            catch
            {
                return new List<PricingPlanItem>();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<PricingPlanItem?> GetPlanByIdAsync(string id)
        {
            var all = await GetAllPlansAsync();
            return all.FirstOrDefault(x => x.Id == id);
        }

        public async Task<PricingPlanItem> SavePlanAsync(PricingPlanItem plan)
        {
            await _semaphore.WaitAsync();
            try
            {
                var list = new List<PricingPlanItem>();
                if (File.Exists(_filePath))
                {
                    var json = await File.ReadAllTextAsync(_filePath);
                    list = JsonSerializer.Deserialize<List<PricingPlanItem>>(json) ?? new List<PricingPlanItem>();
                }

                var existing = list.FirstOrDefault(x => x.Id == plan.Id);
                if (existing != null)
                {
                    existing.Name = plan.Name;
                    existing.Subtitle = plan.Subtitle;
                    existing.MonthlyPrice = plan.MonthlyPrice;
                    existing.Currency = plan.Currency;
                    existing.BillingCycle = plan.BillingCycle;
                    existing.IsPopular = plan.IsPopular;
                    existing.SortOrder = plan.SortOrder;
                    existing.IsActive = plan.IsActive;
                    existing.FeaturesList = plan.FeaturesList;
                    existing.CtaText = plan.CtaText;
                    existing.CtaPlanKey = plan.CtaPlanKey;
                }
                else
                {
                    list.Add(plan);
                }

                var updatedJson = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, updatedJson);
                return plan;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> DeletePlanAsync(string id)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!File.Exists(_filePath)) return false;
                var json = await File.ReadAllTextAsync(_filePath);
                var list = JsonSerializer.Deserialize<List<PricingPlanItem>>(json) ?? new List<PricingPlanItem>();
                var removed = list.RemoveAll(x => x.Id == id);
                if (removed > 0)
                {
                    var updatedJson = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
                    await File.WriteAllTextAsync(_filePath, updatedJson);
                    return true;
                }
                return false;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> TogglePopularAsync(string id)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!File.Exists(_filePath)) return false;
                var json = await File.ReadAllTextAsync(_filePath);
                var list = JsonSerializer.Deserialize<List<PricingPlanItem>>(json) ?? new List<PricingPlanItem>();
                var item = list.FirstOrDefault(x => x.Id == id);
                if (item == null) return false;

                // Make this one popular and remove from others if true
                item.IsPopular = !item.IsPopular;

                var updatedJson = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, updatedJson);
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> ToggleActiveAsync(string id)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!File.Exists(_filePath)) return false;
                var json = await File.ReadAllTextAsync(_filePath);
                var list = JsonSerializer.Deserialize<List<PricingPlanItem>>(json) ?? new List<PricingPlanItem>();
                var item = list.FirstOrDefault(x => x.Id == id);
                if (item == null) return false;

                item.IsActive = !item.IsActive;

                var updatedJson = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, updatedJson);
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
