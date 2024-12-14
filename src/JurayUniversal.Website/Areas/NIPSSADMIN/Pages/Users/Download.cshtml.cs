
using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class DownloadModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<DownloadModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public DownloadModel(SignInManager<Profile> signInManager,
            ILogger<DownloadModel> logger,
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
        public string ProfileLink { get; set; }
        public ParticipantList Participant { get; set; }
        public SuperSetting SuperSetting { get; set; }


        public byte[] PassportImage { get; set; }
        public byte[] ValidIDImage { get; set; }
        public byte[] EmagencyImage { get; set; }
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
            AdditionalProfile = await _context.AdditionalProfiles.Where(x => x.ProfileId == UserDatas.Id).ToListAsync();
            ProfileCategories = await _context.ProfileCategories
                .Include(x => x.ProfileCategoryLists).Where(x => x.ProfileId == UserDatas.Id).ToListAsync();

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
