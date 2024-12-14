using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace JurayUniversal.Domain.Models
{
    public class SuperSetting
    {
        public long Id { get; set; }

        public string? BucketName { get; set; }
        public bool UserNipssArea { get; set; }
        [Display(Name = "Activate Login Botton on Website")]
        public bool ActivateLogin { get; set; }

        [Display(Name = "Activate Login Botton in Menu on Website")]
        public bool ActivateLoginInMenu { get; set; }

        [Display(Name = "Maximum Users")]
        public int MaximumUsers { get; set; }


        public string? TemplateLayoutKey { get; set; }
        public string? LoginTemplateKey { get; set; }
        public string? ColorTemplateKey { get; set; }
        public string? SliderTemplateKey { get; set; }
        public string? ProductTemplateKey { get; set; }
        public string? BlogTemplateKey { get; set; }
        public string? ReloaderTemplateKey { get; set; }
        public string? PageHeaderTemplateKey { get; set; }
        public string? FooterTemplateKey { get; set; }

        [Display(Name = "Website Title")]
        public string? WebsiteTitle { get; set; }

        [Display(Name = "Company Description")]
        public string? CompanyDescription { get; set; }

        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }


        [Display(Name = "Company Name Abbreviation")]
        public string? CompanyNameAbbreviation { get; set; }

        [Display(Name = "Company Website Link")]
        public string? CompanyWebsiteLink { get; set; }


        [Display(Name = "Dashboard Title")]
        public string? DashboardTitle { get; set; }


        [Display(Name = "Company Logo")]
        public string? CompanyLogoUrl { get; set; }
        public string? CompanyLogoKey { get; set; }

        [Display(Name = "Company White Logo")]
        public string? CompanyWhiteLogoUrl { get; set; }
        public string? CompanyWhiteLogoKey { get; set; }

        [Display(Name = "Company Icon")]
        public string? CompanyIconUrl { get; set; }
        public string? CompanyIconKey { get; set; }


        [Display(Name = "Reloader Icon")]
        public string? ReloaderIconUrl { get; set; }
        public string? ReloaderIconKey { get; set; }


        [Display(Name = "Just Website")]
        public bool JustWebsite { get; set; }


        [Display(Name = "Activate Only Authorized Device")]
        public bool ActivateOnlyAuthorizedDevice { get; set; }


        [Display(Name = "Activate SMS")]
        public bool ActivateSMS { get; set; }


        [Display(Name = "Email Template")]
        public string? EmailTemplate { get; set; }


        [Display(Name = "SMS Template")]
        public string? SMSTemplate { get; set; }

        [Display(Name = "Login Background Image")]
        public string? LoginBackgroundImageUrl { get; set; }
        public string? LoginBackgroundImageKey { get; set; }

        [Display(Name = "Use Normal Logo In Login")]
        public bool UseNormalLogoInLogin { get; set; }

        [Display(Name = "Use White Logo In Login")]
        public bool UseWhiteLogoInLogin { get; set; }

        [Display(Name = "Login Title")]
        public string? LoginTitle { get; set; }

        [Display(Name = "Login Note Title")]
        public string? LoginNoteTitle { get; set; }

        [Display(Name = "Login Note")]
        public string? LoginNote { get; set; }



        [Display(Name = "Login Note Footer")]
        public string? LoginNoteFooter { get; set; }


        [Display(Name = "Activate Coming Soon")]
        public bool ActivateComingSoon { get; set; }


        [Display(Name = "Product/Services Title (the title of the product or properties or investment or packages or plans) ")]
        public string? ProductTitle { get; set; }

        [Display(Name = "Home Product/Services Title (the title of the product or properties or investment or packages or plans) ")]
        public string? ProductTitleHome { get; set; }


        [Display(Name = "Show Made By Juray")]
        public bool ShowMadeByJuray { get; set; }

        [Display(Name = "Show Made By XYZ")]
        public bool ShowMadeByXYZ { get; set; }


        [Display(Name = "Activate Profile Portfolio")]
        public bool ActivateProfilePortfolio { get; set; }



        [Display(Name = "Verify Token Folio")]
        public string? VerifyTokenFolio { get; set; }


        [Display(Name = "Portfolio Image One")]
        public string? PortfolioImageOneUrl { get; set; }
        public string? PortfolioImageOneKey { get; set; }

        [Display(Name = "Portfolio Image Two")]
        public string? PortfolioImageTwoUrl { get; set; }
        public string? PortfolioImageTwoKey { get; set; }

        [Display(Name = "Portfolio Breacrum Image")]
        public string? PortfolioBreacrumImageUrl { get; set; }
        public string? PortfolioBreacrumImageKey { get; set; }

        [Display(Name = "Portfolio Title")]
        public string? PortfolioTitle { get; set; }

        [Display(Name = "Portfolio Mini Title")]
        public string? PortfolioMiniTitle { get; set; }

        [Display(Name = "Portfolio Description")]
        public string? PortfolioDescription { get; set; }


        [Display(Name = "Show In Menu Portfolio")]
        public bool ShowInMenuPortfolio { get; set; }


        [Display(Name = "Show In Footer Portfolio")]
        public bool ShowInFooterPortfolio { get; set; }

        [Display(Name = "Special Menu Button Text")]
        public string? SpecialMenuButtonText { get; set; }


        [Display(Name = "Special Menu Button Link")]
        public string? SpecialMenuButtonLink { get; set; }


        [Display(Name = "Special Menu Button Activate")]
        public bool SpecialMenuButtonActivate { get; set; }



        [Display(Name = "Activate Work With Us Footer")]
        public bool ActivateWorkWithUsFooter { get; set; }

        [Display(Name = "Title Work With Us Footer")]
        public string? TitleWorkWithUsFooter { get; set; }

        [Display(Name = "ActivateDownload Our App Footer")]
        public bool ActivateDownloadOurAppFooter { get; set; }

        [Display(Name = "Activate Subscribe Form On Footer")]
        public bool ActivateSubscribeFormOnFooter { get; set; }


        [Display(Name = "Title Download Our App Footer")]
        public string? TitleDownloadOurAppFooter { get; set; }


        [Display(Name = "Description Download Our App Footer")]
        public string? DescriptionDownloadOurAppFooter { get; set; }

        [Display(Name = "PlayStore Image")]
        public string? PlayStoreImageUrl { get; set; }
        public string? PlayStoreImageKey { get; set; }
        [Display(Name = "PlayStore Link")]
        public string? PlayStoreLink { get; set; }
        [Display(Name = "AppStore Image")]
        public string? AppStoreImageUrl { get; set; }
        public string? AppStoreImageKey { get; set; }
        [Display(Name = "AppStore Link")]
        public string? AppStoreLink { get; set; }


        public string? EncryptionNote { get; set; }
        public string? EncryptionUrl { get; set; }
        public string? EncryptionKey { get; set; }



    }
}
