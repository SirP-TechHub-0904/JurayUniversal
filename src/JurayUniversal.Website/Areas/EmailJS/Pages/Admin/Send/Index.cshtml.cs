using JurayUniversal.Application.Repository.NotifyRegister;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web.Helpers;

namespace JurayUniversal.Website.Areas.EmailJS.Pages.Admin.Send
{
    public class IndexModel : PageModel
    {
        private readonly IEmailSendService _email;

        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly RoleManager<AppRole> _roleManager;

        public IndexModel(SignInManager<Profile> signInManager,
            ILogger<IndexModel> logger,
            UserManager<Profile> userManager,
            RoleManager<AppRole> roleManager,
            IEmailSendService email)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _email = email;
        }
        //public IEnumerable<ProfileDto> UserDatasList { get; set; }

        public async Task OnGetAsync()
        {
            string k = "https://nipss24.s3.amazonaws.com/30dd1e90-a424-45b9-b23a-c1990a56fc05.jpg?AWSAccessKeyId=[REDACTED_AWS_ACCESS_KEY]&Expires=1806529807&Signature=2%2BFQ0OUrkwIFCJd162vTvEHwBgw%3D";
            string emailTemplate = @"

<img class=""img-fluid w-100"" src=""XXXX"" alt=""NIPSS PORTAL"">
";
            emailTemplate = emailTemplate.Replace("XXXX", k);
            //UserDatasList = new List<ProfileDto>();
            var UserDatas = _userManager.Users
                .Where(x => x.Email != "universal @j.io" && x.Email != "dashboard@platform.io" && x.Email != "admin@platform.io")
                .Where(x => x.UserStatus == Domain.Enum.Enum.UserStatus.Active)
            .ToList();
            //foreach (var user in UserDatas)
            //{
            //    await _email.SendEmailPostmaster(user.FullnameX, user.Email, "", "", $"Happy Easter!", emailTemplate);

            //}

            //await _email.SendEmailPostmaster("PETER ONWUKA", "onwukaemeka41@gmail.com", "", "", $"Happy Easter!", emailTemplate);

        }
    }
}
