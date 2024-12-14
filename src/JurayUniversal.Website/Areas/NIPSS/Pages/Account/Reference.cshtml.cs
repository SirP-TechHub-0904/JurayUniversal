 
using JurayUniversal.Application.Dtos.AwsDtos;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Account
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class ReferenceModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        public ReferenceModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public Profile UserDatas { get; set; }

        [BindProperty]
        public IFormFile? imagefile { get; set; }
        [BindProperty]
        public IFormFile? imageidcard { get; set; }
        [BindProperty]
        public IFormFile? imageemmergency { get; set; }
        [BindProperty]
        public AdditionalProfile AdditionalProfileData { get; set; }

        [BindProperty]
        public ProfileCategoryList ProfileCategoryList { get; set; }


        public IList<AdditionalProfile> AdditionalProfile { get; set; }
        public IList<ProfileCategory> ProfileCategories { get; set; }


        [BindProperty]
        public Profile Input { get; set; }

        [BindProperty]
        public string redirectttt { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id != null)
            {
                if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                {
                    UserDatas = await _userManager.FindByIdAsync(id);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                UserDatas = await _userManager.FindByIdAsync(user.Id);
            }
            Input = UserDatas;
            if (UserDatas == null)
            {
                return NotFound();
            }
            AdditionalProfile = await _context.AdditionalProfiles.Where(x => x.ProfileId == UserDatas.Id).ToListAsync();
            ProfileCategories = await _context.ProfileCategories.Where(x => x.Title.ToUpper().Contains("REFERENCES"))
                .Include(x => x.ProfileCategoryLists).Where(x => x.ProfileId == UserDatas.Id).ToListAsync();
            ViewData["NameTitle"] = new SelectList(_context.TitleLists, "Title", "Title");

            return Page();
        }
        [BindProperty]
        public bool AcoomodationStatusPopup { get; set; }

        public async Task<IActionResult> OnPostAdditionalAsync()
        {
            _context.AdditionalProfiles.Add(AdditionalProfileData);
            await _context.SaveChangesAsync();
            TempData["success"] = "Successfull";

            return Page();
        }

        public async Task<IActionResult> OnPostCategoriesAsync()
        {
            _context.ProfileCategoryLists.Add(ProfileCategoryList);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(ProfileCategoryList.ProfileId);
            user.UpdateReference = false;
            await _userManager.UpdateAsync(user);
            TempData["success"] = "Successfull";

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToPage("./Reference", new { id = user.Id });

            }
            else
            {
                return RedirectToPage();

            }
        }


    }
}
