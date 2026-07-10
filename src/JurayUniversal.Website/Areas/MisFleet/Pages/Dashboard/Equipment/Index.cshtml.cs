using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisFleet.Pages.Dashboard.Equipment
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisEquipment> Equipments { get; set; } = new List<MisEquipment>();
        public decimal TotalAssetValue { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
        public decimal TotalDepreciation { get; set; }
        public int ActiveAssetsCount { get; set; }
        public int MaintenanceRequiredCount { get; set; }

        // Chart Data
        public List<string> CategoryLabels { get; set; } = new List<string>();
        public List<int> CategoryData { get; set; } = new List<int>();
        
        public List<string> MonthLabels { get; set; } = new List<string>();
        public List<decimal> MaintenanceTrendData { get; set; } = new List<decimal>();

        public async Task OnGetAsync()
        {
            Equipments = await _context.MisEquipments
                .Include(e => e.MisEquipmentCategory)
                .Include(e => e.MisMaintenanceLogs)
                .Include(e => e.AssignedToUser)
                .AsNoTracking()
                .ToListAsync();

            TotalAssetValue = Equipments.Sum(e => e.CurrentValue());
            TotalMaintenanceCost = Equipments.Sum(e => e.MisMaintenanceLogs?.Sum(m => m.Cost) ?? 0);
            TotalDepreciation = Equipments.Sum(e => e.PurchasePrice - e.CurrentValue());
            ActiveAssetsCount = Equipments.Count(e => e.CurrentStatus == Domain.Enum.Enum.MisEquipmentStatus.Available);
            MaintenanceRequiredCount = Equipments.Count(e => e.CurrentStatus == Domain.Enum.Enum.MisEquipmentStatus.Maintenance);

            // Category Distribution Chart
            var categoryGroup = Equipments.GroupBy(e => e.MisEquipmentCategory?.Name ?? "Uncategorized");
            CategoryLabels = categoryGroup.Select(g => g.Key).ToList();
            CategoryData = categoryGroup.Select(g => g.Count()).ToList();

            // Maintenance Trend (Last 6 Months)
            var logs = Equipments.SelectMany(e => e.MisMaintenanceLogs ?? new List<MisMaintenanceLog>()).ToList();
            for (int i = 5; i >= 0; i--)
            {
                var month = System.DateTime.UtcNow.AddMonths(-i);
                MonthLabels.Add(month.ToString("MMM yyyy"));
                
                var cost = logs.Where(l => l.Date.Month == month.Month && l.Date.Year == month.Year).Sum(l => l.Cost);
                MaintenanceTrendData.Add(cost);
            }
        }
    }
}


