using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages
{
    public class UAPModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<UAPModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public UAPModel(SignInManager<Profile> signInManager,
            ILogger<UAPModel> logger,
            UserManager<Profile> userManager,
            Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }
        public Profile UserDatas { get; set; }
        public ParticipantList Participant { get; set; }
        public IList<ProfileCategory> ProfileCategories { get; set; }
        public string ProfileLink { get; set; }

        public SuperSetting SuperSetting { get; set; }
        public StaffManager StaffManager { get; set; }

        public bool Ntfound { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDatas = await _userManager.Users.FirstOrDefaultAsync(x => x.UniqueId == id);

            if (UserDatas == null)
            {
                TempData["error"] = "Unable to Verify Account";
                Ntfound = true;
                return Page();
            }

            //$" <option value=\"Participant\">Participant</option>\r\n                            <option value=\"Staff\">Staff</option>\r\n                   " +
            //    $"         <option value=\"Directing\">Directing Staff</option>\r\n                            <option value=\"Management\">Management Staff</option>";
            if (UserDatas.Role.Contains("Participant"))
            {
                Participant = await _context.ParticipantLists 
                    .Include(x=>x.Course).ThenInclude(x=>x.CourseCategory)
                    .FirstOrDefaultAsync(x=>x.UserId == UserDatas.Id && x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active);
            }
            else  
            {
                StaffManager = await _context.StaffManagers

                                .FirstOrDefaultAsync(x => x.UserId == UserDatas.Id);

            }

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
