using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.Admin.Pages.IOffice.Activity
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager,Admin")]

    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IList<OfficeActivityDialy> OfficeActivityDialy { get; set; }
        public int Customer { get; set; }
        public string Title { get; set; }
        public bool Today { get; set; }
        public decimal Amount { get; set; }
        [BindProperty]
        public long Id { get; set; }
        public async Task OnGetAsync(string startdate = null, string enddate = null, long id = 0)
        {
            DateTime startDate = DateTime.Now.Date; // Today's date
            DateTime endDate = startDate.AddDays(1).AddMilliseconds(-1); // End of today

            if (startdate != null && enddate == null)
            {
                startDate = DateTime.Parse(startdate).Date;
                endDate = DateTime.Parse(startdate).Date;
            }
            else if (startdate == null && enddate != null)
            {
                startDate = DateTime.Parse(enddate).Date;
                endDate = DateTime.Parse(enddate).Date;
            }
            else if (startdate != null && enddate != null)
            {
                startDate = DateTime.Parse(startdate).Date;
                endDate = DateTime.Parse(enddate).Date;

            }
            else
            {
                Today = true;
            }

            TempData["startdate"] = startDate.Date.ToString("ddd dd, MMM, yyyy");
            TempData["enddate"] = endDate.Date.ToString("ddd dd, MMM, yyyy");
            // Filter the list based on the date range

            var ActivityDialy = _context.OfficeActivityDialies
                 .Include(o => o.LastUpdateBy)
                 .Include(o => o.OfficeActivityCategory).AsQueryable();
            if(id > 0)
            {
                            ActivityDialy = ActivityDialy.Where(activity => activity.OfficeActivityCategoryId == id).AsQueryable();
                Id = id;
                var categ = await _context.OfficeActivityCategories.FirstOrDefaultAsync(x=>x.Id == id);
                Title = "FOR "+ categ.Title;
            }
            ActivityDialy = ActivityDialy.Where(activity => activity.Date >= startDate && activity.Date <= endDate).AsQueryable();
            OfficeActivityDialy = await ActivityDialy.ToListAsync();
            Customer = await ActivityDialy.SumAsync(x => x.Customer);
            Amount = await ActivityDialy.SumAsync(x => x.Amount);
        }
    }
}
