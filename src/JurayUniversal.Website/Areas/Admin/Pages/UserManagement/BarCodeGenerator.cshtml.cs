using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using ZXing.QrCode;
using ZXing;
using ZXing.Common;
using System.IO;
using System.Drawing;
using System.Reflection.Emit;
using static System.Net.Mime.MediaTypeNames;

namespace JurayUniversal.Website.Areas.Admin.Pages.UserManagement
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager,Admin")]

    public class BarCodeGeneratorModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;


        public BarCodeGeneratorModel(SignInManager<Profile> signInManager,
            ILogger<IndexModel> logger,
            UserManager<Profile> userManager, JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

       
        [BindProperty]
        public Profile UserDatas { get; set; }
        public string ProfileLink { get; set; }
        public string Companyname { get; set; }
        public SuperSetting SuperSetting { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDatas = await _userManager.FindByIdAsync(id);
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            if (UserDatas == null)
            {
                return NotFound();
            }
             Zen.Barcode.CodeQrBarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            string userinfo = "";
            try

            {
                ProfileLink = "https://"+ SuperSetting.CompanyWebsiteLink+"/profile/" + UserDatas.PhoneNumber;
                System.Drawing.Image img = barcode.Draw(ProfileLink, 100);
                 
                byte[] imgBytes = turnImageToByteArray(img);
                //
                
                string imgString = Convert.ToBase64String(imgBytes);
                Image = imgBytes;
            }
            catch (Exception c)
            {

            }
            try

            {
                Companyname = "https://" + SuperSetting.CompanyWebsiteLink;
                System.Drawing.Image Jurayimg = barcode.Draw(Companyname, 100);

                byte[] JurayimgimgBytes = turnImageToByteArray(Jurayimg);
                //

                string JurayimgimgString = Convert.ToBase64String(JurayimgimgBytes);
                JurayImage = JurayimgimgBytes;
            }
            catch (Exception c)
            {

            }


            return Page();
        }
        [BindProperty]
        public byte[] Image { get; set; }
        [BindProperty]
        public byte[] JurayImage { get; set; }
        private byte[] turnImageToByteArray(System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            return ms.ToArray();
        }
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
           

            return RedirectToPage("./Index");
        }

    }
}
