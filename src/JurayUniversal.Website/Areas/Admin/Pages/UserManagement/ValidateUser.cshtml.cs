using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Admin.Pages.UserManagement
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin")]

    public class ValidateUserModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public ValidateUserModel(SignInManager<Profile> signInManager,
            ILogger<IndexModel> logger,
            UserManager<Profile> userManager,
            JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public Profile UserDatas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            return Page();
        }

        public int report { get; set; }
        public int report2 { get; set; }
         public async Task<IActionResult> OnPostAsync()
        {

            //if (ModelState.IsValid)
            //{
              report = 0;
              report2 = 0;
            var user = await _userManager.FindByEmailAsync(UserDatas.Email);
                try
                {
                    // Define the start and end dates
                    //int datetime = Convert.ToInt32(DateTime.Now.Year.ToString("yyyy"));
                    DateTime startDate = new DateTime(2023, 4, 17); // January 1st, 2023
                    DateTime endDate = new DateTime(2023, 12, 31); // December 31st, 2023

                    // Loop through each date from start to end date
                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        // Check if the current date is Monday to Saturday
                        if (date.DayOfWeek != DayOfWeek.Sunday)
                        {
                            var xuser = await _userManager.FindByIdAsync(user.Id);
                        var checkreport = await _context.Report.FirstOrDefaultAsync(x => x.Date == date.Date && x.UserId == user.Id);
                        if (checkreport == null)
                        {
                            Report R = new Report();

                            R.UserId = xuser.Id;
                            R.Date = date.Date;
                            _context.Report.Add(R);
                            report++;
                        }
                        var checkAttendance = await _context.Attendances.FirstOrDefaultAsync(x => x.Date == date.Date && x.UserId == user.Id);
                        if (checkAttendance == null)
                        {
                            Attendance AA = new Attendance();

                            AA.UserId = xuser.Id;
                            AA.Date = date.Date;
                            _context.Attendances.Add(AA);
                            report2++;
                        }
                    }
                    }
                    await _context.SaveChangesAsync(); TempData["success"] = "Successful";
                }
                catch (Exception d)
                {

                }
             

            return Page();
            
        }

    }
}
