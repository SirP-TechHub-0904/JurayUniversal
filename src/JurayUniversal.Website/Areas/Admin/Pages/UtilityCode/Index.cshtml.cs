using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Admin.Pages.UtilityCode
{
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IList<Report> Report { get; set; }

        public async Task OnGetAsync()
        
        {
            // Define the start and end dates
            DateTime startDate = new DateTime(2023, 4, 17); // January 1st, 2023
            DateTime endDate = new DateTime(2023, 12, 31); // December 31st, 2023

            // Loop through each date from start to end date
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                // Check if the current date is Monday to Saturday
                if (date.DayOfWeek == DayOfWeek.Wednesday)
                {
                     
                    Training t = new Training();
                   
                    t.Topic = "--Topic--";
                    t.Date = date.Date;
                    _context.Trainings.Add(t);
                     
                }
                if (date.DayOfWeek == DayOfWeek.Wednesday)
                {

                    TrainingSchedule x = new TrainingSchedule();
                    x.Moderator = "Moderator Name";
                    x.Trainner = "Trainner Name";
                    x.Topic = "--Topic--";
                    x.Date = date.Date;
                    x.TrainingStatus = Domain.Enum.Enum.TrainingStatus.None;
                    _context.TrainingSchedules.Add(x);

                }
            }
            await _context.SaveChangesAsync(); TempData["success"] = "Successful";
        }
    }
}
