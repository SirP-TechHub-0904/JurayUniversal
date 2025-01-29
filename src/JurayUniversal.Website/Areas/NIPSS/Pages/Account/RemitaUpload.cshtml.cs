using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Application.Dtos.AwsDtos;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Account
{
    public class RemitaUploadModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IConfiguration _config;

        private readonly IStorageService _storageService;
        public RemitaUploadModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _storageService = storageService;
        }
        [BindProperty]
        public Profile UserDatas { get; set; }
        public ParticipantList ParticipantList { get; set; }
        public Accomodation UserAccomodation { get; set; }

        [BindProperty]
        public List<Accomodation> Accomodation { get; set; }

        public Setting Setting { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.UpdateProfile == true)
            {
                TempData["error"] = "Update your information";
                return RedirectToPage("/Account/UpdateProfile", new { area = "NIPSS" });

            }

            else if (user.UpdateEducation == true)
            {
                TempData["error"] = "Update your information";
                return RedirectToPage("/Account/Education", new { area = "NIPSS" });

            }
            else if (user.UpdateExperience == true)
            {
                TempData["error"] = "Update your information";
                return RedirectToPage("/Account/Experience", new { area = "NIPSS" });

            }
            else if (user.UpdateCertificate == true)
            {
                TempData["error"] = "Update your information";
                return RedirectToPage("/Account/Certificate", new { area = "NIPSS" });

            }
            else if (user.UpdateSkills == true)
            {
                TempData["error"] = "Update your information";
                return RedirectToPage("/Account/Skills", new { area = "NIPSS" });

            }
            else if (user.UpdateLanguage == true)
            {
                TempData["error"] = "Update your information";
                return RedirectToPage("/Account/Language", new { area = "NIPSS" });

            }
            else if (user.UpdateAwards == true)
            {
                TempData["error"] = "Update your information";
                return RedirectToPage("/Account/Awards", new { area = "NIPSS" });

            }

            else if (user.UpdateInterest == true)
            {
                TempData["error"] = "Update your information";
                return RedirectToPage("/Account/Interest", new { area = "NIPSS" });

            }

            else if (user.UpdateReference == true)
            {
                TempData["error"] = "Update your information";
                return RedirectToPage("/Account/Reference", new { area = "NIPSS" });

            }

            UserDatas = await _userManager.FindByIdAsync(user.Id);
            ParticipantList = await _context.ParticipantLists
                .Include(x => x.Course).ThenInclude(x => x.CourseCategory)
                .FirstOrDefaultAsync(x => x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active && x.UserId == user.Id);


            if (ParticipantList == null)
            {
                TempData["error"] = "Unable to fetch data";
                return RedirectToPage("/Dashboard/Index");
            }
            ParticipantId = ParticipantList.Id;

            UserAccomodation = await _context.Accomodations.FirstOrDefaultAsync(x => x.ParticipantListId == ParticipantList.Id);

            // Generate a random seed
            Random random = new Random();

            // Count the total number of accommodations with non-null ParticipantListId
            int totalCount = await _context.Accomodations.CountAsync(x => x.ParticipantListId != null);

            // Generate a random number between 0 and totalCount
            int skipCount = random.Next(0, totalCount);

            // Fetch 3 accommodations randomly
            Accomodation = await _context.Accomodations
                .Where(x => x.ParticipantListId == null)
                .Where(x => x.Disable == false)
                .OrderBy(x => Guid.NewGuid()) // Order randomly
                .Skip(skipCount)
                .Take(3)
                .ToListAsync();

            //
            Setting = await _context.Settings.FirstOrDefaultAsync();

            return Page();
        }
        [BindProperty]
        public int ParticipantId { get; set; }
        [BindProperty]
        public IFormFile? RemitaFile { get; set; }
        public async Task<IActionResult> OnPostAccomodationPost(long id, string userId)
        {
            var accomo = await _context
                .Accomodations
                .Include(x => x.ParticipantList)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (accomo != null)
            {

                accomo.ParticipantListId = ParticipantId;
                _context.Attach(accomo).State = EntityState.Modified;
                TempData["success"] = "Successful";
                await _context.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(userId);
                user.AccomodationUpdateStatus = Domain.Enum.Enum.AccomodationUpdateStatus.Allocated;
                await _userManager.UpdateAsync(user);

                return RedirectToPage();
            }
            TempData["error"] = "Unable to process";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var participant = await _context.ParticipantLists
                .FirstOrDefaultAsync(x => x.Id == ParticipantId);
            if (participant == null)
            {
                return RedirectToPage("./Accomodation");
            }

            if (RemitaFile != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await RemitaFile.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(RemitaFile.FileName);
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

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, null);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        participant.RemitaUrl = xresult.Url;
                        participant.RemitaKey = xresult.Key;
                        _context.ParticipantLists.Update(participant);
                        await _context.SaveChangesAsync();
                        TempData["error"] = "uploaded";
                        return RedirectToPage("./Accomodation");
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
            TempData["error"] = "unable to upload image";
            return RedirectToPage("./Accomodation");
        }

    }
}
