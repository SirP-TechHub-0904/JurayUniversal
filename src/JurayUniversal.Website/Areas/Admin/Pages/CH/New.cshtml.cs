using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Admin.Pages.CH
{
    public class NewModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public NewModel(SignInManager<Profile> signInManager,
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

        [BindProperty]
        public UserCategory UserCategory { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            UserCategory = await _context.UserCategories.FirstOrDefaultAsync(x=>x.Id == id);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            //if (ModelState.IsValid)
            //{
            var user = new Profile
            {
                UserName = UserDatas.Email,
                Email = UserDatas.Email,
                PhoneNumber = UserDatas.PhoneNumber,
                FirstName = UserDatas.FirstName,
                LastName = UserDatas.LastName,
                Date = DateTime.UtcNow,
                RefCode = "", 
                UserCategoryId = UserDatas.UserCategoryId,
            };


            user.Id = Guid.NewGuid().ToString();
            var result = await _userManager.CreateAsync(user, UserDatas.PhoneNumber);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                await _userManager.AddToRoleAsync(user, "User");
                try
                {
                    List<string> category = new List<string> { "EDUCATION", "EXPERIENCE", "CERTIFICATIONS", "SKILLS", "LANGUAGES", "AWARDS", "INTEREST", "REFERENCES" };

                    foreach (string item in category)
                    {
                        ProfileCategory p = new ProfileCategory();
                        p.ProfileId = user.Id;
                        p.Title = item;
                        _context.ProfileCategories.Add(p);
                        await _context.SaveChangesAsync();

                    }
                }
                catch (Exception c)
                {

                }

                try
                {
                    // Define the start and end dates
                    //int datetime = Convert.ToInt32(DateTime.Now.Year.ToString("yyyy"));
                    DateTime startDate = DateTime.UtcNow.Date; // January 1st, 2023
                    int Year = DateTime.UtcNow.Year;
                    DateTime endDate = new DateTime(Year, 12, 31); // December 31st, 2023

                    // Loop through each date from start to end date
                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        // Check if the current date is Monday to Saturday
                        if (date.DayOfWeek != DayOfWeek.Sunday)
                        {
                            var xuser = await _userManager.FindByIdAsync(user.Id);

                            Report R = new Report();

                            R.UserId = xuser.Id;
                            R.Date = date.Date;
                            _context.Report.Add(R);

                            Attendance AA = new Attendance();

                            AA.UserId = xuser.Id;
                            AA.Date = date.Date;
                            _context.Attendances.Add(AA);

                        }
                    }
                    await _context.SaveChangesAsync(); TempData["success"] = "Successful";
                }
                catch (Exception d)
                {

                }


                return RedirectToPage("./Index", new {id = user.UserCategoryId});

            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            //}

            return Page();
        }

    }
}
