using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.NutriValution
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Evaluator")]

    public class DownloadEvaluationModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<DownloadEvaluationModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public DownloadEvaluationModel(SignInManager<Profile> signInManager,
            ILogger<DownloadEvaluationModel> logger,
            UserManager<Profile> userManager,
            Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }
        public Profile UserDatas { get; set; }
       
        public string ProfileLink { get; set; } 
        public SuperSetting SuperSetting { get; set; }


        public byte[] PassportImage { get; set; }
        public byte[] ValidIDImage { get; set; }
        public byte[] EmagencyImage { get; set; }

        public IList<NutritionCategory> NutritionCategory { get; set; }
        [BindProperty]
        public IList<NutritionCategory> ParticipantNutritionCategory { get; set; }

        [BindProperty]
        public IList<UserNutritionEvaluation> UserNutritionEvaluation { get; set; }

        public ParticipantList Participant { get; set; }


        public async Task<IActionResult> OnGetAsync(string id)
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



            ParticipantNutritionCategory = await _context.NutritionCategories
              //.Include(x => x.NutritionCategoryList)
              //.Include(x => x.UserNutritionEvaluation).ThenInclude(x => x.NutritionCategoryList)
              //.Where(x => x.Disable == false)
              //.Where(x => x.UserNutritionEvaluation.Any(e => e.UserId == UserDatas.Id))
              .ToListAsync();

            UserNutritionEvaluation = await _context.UserNutritionEvaluations
                .Include(x => x.NutritionCategoryList)
                .Where(x => x.UserId == UserDatas.Id)
                .ToListAsync();

            Participant = await _context.ParticipantLists
               .Include(x => x.Accomodation)
               .Include(x => x.Course).ThenInclude(x => x.CourseCategory)
               .FirstOrDefaultAsync(x => x.UserId == UserDatas.Id && x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active);

            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            Zen.Barcode.CodeQrBarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            string userinfo = "";
            try

            {
                ProfileLink = "https://" + SuperSetting.CompanyWebsiteLink + "/uap/" + UserDatas.UniqueId;
                System.Drawing.Image img = barcode.Draw(ProfileLink, 100);

                byte[] imgBytes = turnImageToByteArray(img);
                //

                string imgString = Convert.ToBase64String(imgBytes);
                BarcodeImage = imgBytes;
            }
            catch (Exception c)
            {

            }

            //
            try
            {
                WebClient webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(UserDatas.PassportFilePathUrl);
                PassportImage = imageBytes;
            }
            catch (Exception ex)
            {
                byte[] defaultBytes = Array.Empty<byte>();
                PassportImage = defaultBytes;
            }
            //
            try
            {
                WebClient webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(UserDatas.ValidIDCardUrl);
                ValidIDImage = imageBytes;
            }
            catch (Exception ex)
            {
                byte[] defaultBytes = Array.Empty<byte>();
                ValidIDImage = defaultBytes;
            }
            //
            //
            try
            {
                WebClient webClient = new WebClient();
                byte[] imageBytes = webClient.DownloadData(UserDatas.EmergencyContactValidIdCardUrl);
                EmagencyImage = imageBytes;
            }
            catch (Exception ex)
            {
                byte[] defaultBytes = Array.Empty<byte>();
                EmagencyImage = defaultBytes;
            }




            return Page();
        }
        [BindProperty]
        public byte[] BarcodeImage { get; set; }
        private byte[] turnImageToByteArray(System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            return ms.ToArray();
        }
    }
}
