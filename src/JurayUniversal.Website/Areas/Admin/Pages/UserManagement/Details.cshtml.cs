using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Admin.Pages.UserManagement
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager,Admin")]

    public class DetailsModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<DetailsModel> _logger;
                private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public DetailsModel(SignInManager<Profile> signInManager,
            ILogger<DetailsModel> logger,
            UserManager<Profile> userManager,
            Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }
        public Profile UserDatas { get; set; } 
        public IList<AdditionalProfile> AdditionalProfile { get; set; } 
        public IList<ProfileCategory> ProfileCategories { get; set; } 

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDatas = await _userManager.FindByIdAsync(id);

            if (UserDatas == null)
            {
                return NotFound();
            }
            AdditionalProfile = await _context.AdditionalProfiles.Where(x=>x.ProfileId == UserDatas.Id).ToListAsync();
            ProfileCategories = await _context.ProfileCategories
                .Include(x=>x.ProfileCategoryLists).Where(x=>x.ProfileId == UserDatas.Id).ToListAsync();
            return Page();
        }

        public int report { get; set; }
        public int report2 { get; set; }
        [BindProperty]
        public string EmailData { get;set;}
        public async Task<IActionResult> OnPostAsync()
        {

            //if (ModelState.IsValid)
            //{
            report = 0;
            report2 = 0;
            var user = await _userManager.FindByEmailAsync(EmailData);
            try
            {
                // Define the start and end dates
                //int datetime = Convert.ToInt32(DateTime.Now.Year.ToString("yyyy"));
                DateTime startDate = new DateTime(2024, 4, 17); // January 1st, 2023
                DateTime endDate = new DateTime(2024, 12, 31); // December 31st, 2023

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


            return RedirectToPage(".Details", new {id = user.Id});

        }

    }
}
