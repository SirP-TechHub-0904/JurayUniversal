using JurayUniversal.Application.Dtos.AwsDtos;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Account
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class UpdateProfileModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;
        public UpdateProfileModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _storageService = storageService;
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

        public class InputModel
        {
            [Display(Name = "First Name")]
            [Required]
            public string? FirstName { get; set; }

            [Display(Name = "Title")]
            [Required]
            public string? Title { get; set; }

            [Display(Name = "Middle Name")]
            [Required]
            public string? MiddleName { get; set; }

            [Display(Name = "Last Name")]
            [Required]
            public string? LastName { get; set; }

            [Required]
            public string PhoneNumber { get; set; }

            [Required]
            public string AltPhoneNumber { get; set; }
            [Required]
            public string Email { get; set; }
            [Required]
            public string OfficeEmail { get; set; }
            [Required]
            public string PermanentHomeAddress { get; set; }
            [Required]
            public string PermanentLga { get; set; }
            [Required]
            public string PermanentState { get; set; }
            [Required]
            public string Address { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string State { get; set; }
            [Required]
            public string Country { get; set; }
            [Required]
            public DateTime? DateOfBirth { get; set; }
            [Required]
            public GenderStatus Gender { get; set; }
            [Required]
            public string MaritalStatus { get; set; }
            [Required]
            public string Biography { get; set; }

            [Required]
            public string? Referee { get; set; }
            [Required]
            public string? BVN { get; set; }

            [Required]
            public string? EmergencyContactName { get; set; }

            [Required]
            public string? EmergencyContactNumber { get; set; }

            [Required]
            public string? EmergencyContactRelationship { get; set; }

        }

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
            ProfileCategories = await _context.ProfileCategories
                .Include(x => x.ProfileCategoryLists).Where(x => x.ProfileId == UserDatas.Id).ToListAsync();
            ViewData["NameTitle"] = new SelectList(_context.TitleLists, "Title", "Title");

            return Page();
        }
        [BindProperty]
        public bool AcoomodationStatusPopup { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var user = await _userManager.FindByIdAsync(Input.Id);
                var userinfo = await _userManager.FindByIdAsync(Input.Id);
                //
                if (Input.PermanentLga == null && user.PermanentLga == null)
                {
                    TempData["noteerror"] = "Permanent LGA Required";
                    return Page();
                }
                if (Input.DateOfBirth == null)
                {
                    TempData["noteerror"] = "Date Of Birth Required";
                }



                //image
                if (imagefile != null)
                {
                    try
                    {
                        // Process file
                        await using var memoryStream = new MemoryStream();
                        await imagefile.CopyToAsync(memoryStream);

                        var fileExt = Path.GetExtension(imagefile.FileName);
                        var docName = $"{Guid.NewGuid()}{fileExt}";
                        // call server

                        var s3Obj = new Application.Dtos.AwsDtos.S3Object()
                        {
                            BucketName = await _storageService.BucketName(),
                            InputStream = memoryStream,
                            Name = docName
                        };

                        var cred = new AwsCredentials()
                        {
                            AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                            SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                        };

                        var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, userinfo.PassportFilePathKey);
                        // 
                        if (xresult.Message.Contains("200"))
                        {
                            userinfo.PassportFilePathUrl = xresult.Url;
                            userinfo.PassportFilePathKey = xresult.Key;
                        }
                        else
                        {
                            TempData["error"] = "unable to upload image";
                            //return Page();
                        }
                    }
                    catch (Exception c)
                    {

                    }
                }


                //image
                if (imageidcard != null)
                {
                    try
                    {
                        // Process file
                        await using var memoryStream = new MemoryStream();
                        await imageidcard.CopyToAsync(memoryStream);

                        var fileExt = Path.GetExtension(imageidcard.FileName);
                        var docName = $"{Guid.NewGuid()}{fileExt}";
                        // call server

                        var s3Obj = new Application.Dtos.AwsDtos.S3Object()
                        {
                            BucketName = await _storageService.BucketName(),
                            InputStream = memoryStream,
                            Name = docName
                        };

                        var cred = new AwsCredentials()
                        {
                            AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                            SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                        };

                        var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, userinfo.ValidIDCardKey);
                        // 
                        if (xresult.Message.Contains("200"))
                        {
                            userinfo.ValidIDCardUrl = xresult.Url;
                            userinfo.ValidIDCardKey = xresult.Key;
                        }
                        else
                        {
                            TempData["error"] = "unable to upload image";
                            //return Page();
                        }
                    }
                    catch (Exception c)
                    {

                    }
                }


                //image
                if (imageemmergency != null)
                {
                    try
                    {
                        // Process file
                        await using var memoryStream = new MemoryStream();
                        await imageemmergency.CopyToAsync(memoryStream);

                        var fileExt = Path.GetExtension(imageemmergency.FileName);
                        var docName = $"{Guid.NewGuid()}{fileExt}";
                        // call server

                        var s3Obj = new Application.Dtos.AwsDtos.S3Object()
                        {
                            BucketName = await _storageService.BucketName(),
                            InputStream = memoryStream,
                            Name = docName
                        };

                        var cred = new AwsCredentials()
                        {
                            AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                            SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                        };

                        var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, userinfo.EmergencyContactValidIdCardUrl);
                        // 
                        if (xresult.Message.Contains("200"))
                        {
                            userinfo.EmergencyContactValidIdCardUrl = xresult.Url;
                            userinfo.EmergencyContactValidIdCardKey = xresult.Key;
                        }
                        else
                        {
                            TempData["error"] = "unable to upload image";
                            //return Page();
                        }
                    }
                    catch (Exception c)
                    {

                    }
                }


                userinfo.Title = Input.Title;
                userinfo.FirstName = Input.FirstName;
                userinfo.LastName = Input.LastName;
                userinfo.MiddleName = Input.MiddleName;

                userinfo.AltPhoneNumber = Input.AltPhoneNumber;
                userinfo.PositionOrRank = Input.PositionOrRank;
                userinfo.PermanentHomeAddress = Input.PermanentHomeAddress;

                userinfo.Address = Input.Address;
                userinfo.City = Input.City;
                userinfo.State = Input.State;


                userinfo.EmergencyContactName = Input.EmergencyContactName;
                userinfo.EmergencyContactNumber = Input.EmergencyContactNumber;
                userinfo.EmergencyContactRelationship = Input.EmergencyContactRelationship;
                userinfo.Sponsor = Input.Sponsor;
                userinfo.SponsorAddress = Input.SponsorAddress;
                userinfo.PermanentState = Input.PermanentState;
                if (Input.PermanentLga != null)
                {
                    userinfo.PermanentLga = Input.PermanentLga;
                }
                if (Input.DateOfBirth != null)
                {
                    userinfo.DateOfBirth = Input.DateOfBirth;
                }
                userinfo.Gender = Input.Gender;
                userinfo.MaritalStatus = Input.MaritalStatus;
                userinfo.Biography = Input.Biography;


                userinfo.BloodGroup = Input.BloodGroup;
                userinfo.Genotype = Input.Genotype;
                userinfo.PreviousAppointment = Input.PreviousAppointment;
                userinfo.InternationalPassportNumber = Input.InternationalPassportNumber;
                userinfo.InternationalPassportExpiringDate = Input.InternationalPassportExpiringDate;


                userinfo.UpdateProfile = false;
                await _userManager.UpdateAsync(userinfo);
                TempData["success"] = "Successfull";

                if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                {
                    return RedirectToPage("./Education", new {id = userinfo.Id});

                }
                else
                {
                    return Redirect("./Education");

                }
            }
            catch (Exception c)
            {
                var user = await _userManager.FindByIdAsync(Input.Id); 
                UserDatas = await _userManager.FindByIdAsync(user.Id);

                if (UserDatas == null)
                {
                    return NotFound();
                }
                AdditionalProfile = await _context.AdditionalProfiles.Where(x => x.ProfileId == UserDatas.Id).ToListAsync();
                ProfileCategories = await _context.ProfileCategories
                    .Include(x => x.ProfileCategoryLists).Where(x => x.ProfileId == UserDatas.Id).ToListAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAdditionalAsync()
        {
            _context.AdditionalProfiles.Add(AdditionalProfileData);
            await _context.SaveChangesAsync();
            TempData["success"] = "Successfull";
            return Redirect("./UpdateProfile#additional");
        }

        public async Task<IActionResult> OnPostCategoriesAsync()
        {
            _context.ProfileCategoryLists.Add(ProfileCategoryList);
            await _context.SaveChangesAsync();
            TempData["success"] = "Successfull";
            return Redirect("./UpdateProfile#" + redirectttt);
        }


    }
}
