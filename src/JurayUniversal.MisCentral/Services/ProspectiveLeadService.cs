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
    public class ProspectiveLead
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // Organization Info
        public string OrganizationName { get; set; } = string.Empty;
        public string OrganizationType { get; set; } = string.Empty; // School, University, Hospital, Government, Corporate, Community
        public string EstimatedSize { get; set; } = string.Empty; // Under 250, 250 - 1,000, 1,000 - 5,000, 5,000+
        public string StateOrCity { get; set; } = string.Empty;

        // Contact Person
        public string ContactPersonName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Requirements & Features
        public List<string> FeaturesOfInterest { get; set; } = new();
        public string PrimaryGoal { get; set; } = string.Empty;
        public string RolloutTimeline { get; set; } = "Within 30 Days"; // Immediate, Within 30 Days, Next Quarter, Exploring
        public string AdditionalNotes { get; set; } = string.Empty;

        // Admin Follow-up Workflow
        public string Status { get; set; } = "New"; // New, Contacted, Demo Scheduled, Won/Converted, Closed
        public string? AdminNotes { get; set; }
        public DateTime? LastFollowUpUtc { get; set; }
    }

    public interface IProspectiveLeadService
    {
        Task<List<ProspectiveLead>> GetAllLeadsAsync();
        Task<ProspectiveLead?> GetLeadByIdAsync(string id);
        Task<ProspectiveLead> CreateLeadAsync(ProspectiveLead lead);
        Task<bool> UpdateStatusAsync(string id, string status, string? adminNotes);
        Task<bool> DeleteLeadAsync(string id);
    }

    public class ProspectiveLeadService : IProspectiveLeadService
    {
        private readonly string _filePath;
        private static readonly SemaphoreSlim _semaphore = new(1, 1);

        public ProspectiveLeadService(IWebHostEnvironment env)
        {
            var dataDir = Path.Combine(env.ContentRootPath, "App_Data");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
            _filePath = Path.Combine(dataDir, "prospective_leads.json");
            EnsureSeedData();
        }

        private void EnsureSeedData()
        {
            if (!File.Exists(_filePath))
            {
                var seedList = new List<ProspectiveLead>
                {
                    new ProspectiveLead
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        CreatedAtUtc = DateTime.UtcNow.AddDays(-2),
                        OrganizationName = "Ahmadu Bello Science Academy",
                        OrganizationType = "Primary / Secondary School",
                        EstimatedSize = "1,000 - 5,000 Students",
                        StateOrCity = "Kaduna",
                        ContactPersonName = "Malam Haruna Bello",
                        JobTitle = "Principal",
                        Email = "principal@absa.edu.ng",
                        Phone = "+2348031234567",
                        FeaturesOfInterest = new List<string> { "School & Student Portal", "Online CBT Exams", "Fees & Card Payments", "Instant SMS Alerts" },
                        PrimaryGoal = "Automate termly report card calculations and online student fee payment with instant SMS alerts.",
                        RolloutTimeline = "Immediate (This Term)",
                        Status = "New"
                    },
                    new ProspectiveLead
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        CreatedAtUtc = DateTime.UtcNow.AddDays(-4),
                        OrganizationName = "Greenfield Specialist Hospital",
                        OrganizationType = "Hospital / Healthcare",
                        EstimatedSize = "250 - 1,000 Staff/Patients",
                        StateOrCity = "Abuja FCT",
                        ContactPersonName = "Dr. Grace Oladipo",
                        JobTitle = "Medical Director",
                        Email = "dr.grace@greenfieldhealth.ng",
                        Phone = "+2348029876543",
                        FeaturesOfInterest = new List<string> { "Staff Payroll & HR", "Accounting & Invoicing", "Inventory & Store Tracking" },
                        PrimaryGoal = "Implement computerized pharmacy stock tracking, doctor consults, and automated monthly staff payroll.",
                        RolloutTimeline = "Within 30 Days",
                        Status = "Demo Scheduled",
                        AdminNotes = "Demo scheduled for Friday 11:00 AM with their ICT Director."
                    }
                };

                var json = JsonSerializer.Serialize(seedList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
        }

        public async Task<List<ProspectiveLead>> GetAllLeadsAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<ProspectiveLead>();
                }
                var json = await File.ReadAllTextAsync(_filePath);
                var items = JsonSerializer.Deserialize<List<ProspectiveLead>>(json);
                return items?.OrderByDescending(x => x.CreatedAtUtc).ToList() ?? new List<ProspectiveLead>();
            }
            catch
            {
                return new List<ProspectiveLead>();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<ProspectiveLead?> GetLeadByIdAsync(string id)
        {
            var leads = await GetAllLeadsAsync();
            return leads.FirstOrDefault(x => x.Id == id);
        }

        public async Task<ProspectiveLead> CreateLeadAsync(ProspectiveLead lead)
        {
            await _semaphore.WaitAsync();
            try
            {
                var list = new List<ProspectiveLead>();
                if (File.Exists(_filePath))
                {
                    var json = await File.ReadAllTextAsync(_filePath);
                    list = JsonSerializer.Deserialize<List<ProspectiveLead>>(json) ?? new List<ProspectiveLead>();
                }
                list.Insert(0, lead);
                var updatedJson = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, updatedJson);
                return lead;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> UpdateStatusAsync(string id, string status, string? adminNotes)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!File.Exists(_filePath)) return false;
                var json = await File.ReadAllTextAsync(_filePath);
                var list = JsonSerializer.Deserialize<List<ProspectiveLead>>(json) ?? new List<ProspectiveLead>();
                var item = list.FirstOrDefault(x => x.Id == id);
                if (item == null) return false;

                item.Status = status;
                if (!string.IsNullOrWhiteSpace(adminNotes))
                {
                    item.AdminNotes = adminNotes;
                }
                item.LastFollowUpUtc = DateTime.UtcNow;

                var updatedJson = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, updatedJson);
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> DeleteLeadAsync(string id)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!File.Exists(_filePath)) return false;
                var json = await File.ReadAllTextAsync(_filePath);
                var list = JsonSerializer.Deserialize<List<ProspectiveLead>>(json) ?? new List<ProspectiveLead>();
                var count = list.RemoveAll(x => x.Id == id);
                if (count > 0)
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
    }
}
