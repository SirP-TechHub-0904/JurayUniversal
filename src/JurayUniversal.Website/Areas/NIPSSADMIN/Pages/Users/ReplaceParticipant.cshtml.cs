using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users
{

    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class ReplaceParticipantModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public ReplaceParticipantModel(
            UserManager<Profile> userManager,
            SignInManager<Profile> signInManager,
            Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {

            [Required]
            public string Email { get; set; }
            [Required]
            public string Title { get; set; }
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string MiddleName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string PhoneNumber { get; set; }


        }

        [BindProperty]
        public Profile UserData { get; set; }

        public ParticipantList ParticipantList { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserData = user;
            ParticipantList = await _context.ParticipantLists.Include(x => x.Course).ThenInclude(x => x.CourseCategory)
                           .Include(x => x.Accomodation)
                           .FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == user.Id);

            if (ParticipantList == null)
            {
                return NotFound();
            }
            ViewData["NameTitle"] = new SelectList(_context.TitleLists, "Title", "Title");

            return Page();
        }
        static string GenerateRandomAlphaNumeric(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static string GenerateRandomAlphaNumericUniqueId(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<IActionResult> OnPostAsync()
        {


            var user = await _userManager.FindByIdAsync(UserData.Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            UserData = user;
            var checkemail = await _userManager.FindByEmailAsync(Input.Email);
             if (checkemail == null)
            {
                // var emailtoken = await _userManager.GenerateChangeEmailTokenAsync(user, Input.Email);
                //var cheangeemail = await _userManager.ChangeEmailAsync(user, Input.Email, emailtoken);
                //if (cheangeemail.Succeeded)
                //{
                //    //
                //    string password = GenerateRandomAlphaNumeric(8);

                //    var removepass = await _userManager.RemovePasswordAsync(user);
                //    user.TempPass = "";
                //    if (removepass.Succeeded)
                //    {
                //        var addpassword = await _userManager.AddPasswordAsync(user, password);
                //        user.TempPass = password;
                //    }


                //    //
                //    user.PhoneNumber = Input.PhoneNumber;
                //    user.FirstName = Input.FirstName;
                //    user.LastName = Input.LastName;
                //    user.MiddleName = Input.MiddleName;

                //    user.ResetPassword = true;
                //    user.UpdateProfile = true;
                //    user.UpdateAwards = true;
                //    user.UpdateCertificate = true;
                //    user.UpdateEducation = true;
                //    user.UpdateExperience = true;
                //    user.UpdateInterest = true;
                //    user.UpdateLanguage = true;
                //    user.UpdateReference = true;
                //    user.UpdateSkills = true;
                //    user.LockoutEnabled = false;

                //    while (user.UniqueId != null)
                //    {
                //        string uniqId = GenerateRandomAlphaNumericUniqueId(8);

                //        // Check if the generated IdDigit already exists in the database
                //        var check = await _userManager.Users.FirstOrDefaultAsync(x => x.UniqueId == uniqId);

                //        if (check == null)
                //        {
                //            // If not found, update the UserDatas and exit the loop
                //            user.UniqueId = uniqId;
                //            break;
                //        }

                //    }

                //    user.Sponsor = null;
                //    user.SponsorAddress = null;
                //    user.PreviousAppointment = null;
                //    user.PositionOrRank = null;
                //    user.PermanentHomeAddress = null;
                //    user.PermanentState = null;
                //    user.PermanentLga = null;
                //    user.Address = null;
                //    user.City = null;
                //    user.State = null;
                //    user.DateOfBirth = null;
                //    user.Gender = Domain.Enum.Enum.GenderStatus.Male;
                //    user.MaritalStatus = null;
                //    user.Biography = null;
                //    user.PassportFilePathUrl = null;
                //    user.PassportFilePathKey = null;
                //    user.ValidIDCardUrl = null;
                //    user.ValidIDCardKey = null;
                //    user.InternationalPassportNumber = null;
                //    user.InternationalPassportExpiringDate = null;
                //    user.BloodGroup = null;
                //    user.Genotype = null;
                //    user.EmergencyContactName = null;
                //    user.EmergencyContactNumber = null;
                //    user.EmergencyContactRelationship = null;
                //    user.EmergencyContactValidIdCardUrl = null;
                //    user.EmergencyContactValidIdCardKey = null;
                //    user.EmailSent = false;
                //    user.SmsSent =false;
                //    await _userManager.UpdateAsync(user);
                //    //participant
                //   var ParticipantListData = await _context.ParticipantLists.Include(x => x.Course).ThenInclude(x => x.CourseCategory) 
                //          .FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == user.Id);
                //    ParticipantListData.StudyGroupId = null;
                //    _context.Attach(ParticipantListData).State = EntityState.Modified;
                //    //
                //    var accomodation = await _context.Accomodations.FirstOrDefaultAsync(x=>x.ParticipantListId == ParticipantListData.Id);
                //    accomodation.ParticipantListId = null;
                //    _context.Attach(accomodation).State = EntityState.Modified;
                //    //
                //    var profilecategorylist = await _context.ProfileCategoryLists.Where(p => p.ProfileId == user.Id).ToListAsync();
                //    foreach (var item in profilecategorylist)
                //    {
                //        _context.ProfileCategoryLists.Remove(item);
                //    }
                //    await _context.SaveChangesAsync();

                //    TempData["success"] = "successful";


                //    return RedirectToPage("./RegistrationStatus", new { id = ParticipantListData.Id });
                //}

                //TempData["error"] = "Unable to replace/Email Already Taken";
                 
                //if (user == null)
                //{
                //    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                //}

                //UserData = user;
                //ParticipantList = await _context.ParticipantLists.Include(x => x.Course).ThenInclude(x => x.CourseCategory)
                //               .Include(x => x.Accomodation)
                //               .FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == user.Id);

                //if (ParticipantList == null)
                //{
                //    return NotFound();
                //}
                //ViewData["NameTitle"] = new SelectList(_context.TitleLists, "Title", "Title");

                return Page();
            }
            else
            {
                TempData["error"] = "Email Already Taken";
                
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                UserData = user;
                ParticipantList = await _context.ParticipantLists.Include(x => x.Course).ThenInclude(x => x.CourseCategory)
                               .Include(x => x.Accomodation)
                               .FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == user.Id);

                if (ParticipantList == null)
                {
                    return NotFound();
                }
                ViewData["NameTitle"] = new SelectList(_context.TitleLists, "Title", "Title");



                return Page();
            }


        }
    }
}
