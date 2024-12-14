using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class Setting
    {
        public long Id { get; set; }

        [Display(Name = "Qoute")]
        public string? Qoute { get; set; }
        [Display(Name = "Top Note")]
        public string? TopNote { get; set; }

        [Display(Name = "Google Map")]
        public string? GoogleMap { get; set; }
        [Display(Name = "Show Address One In Top")]
        public bool ShowAddressOneInTop { get; set; }
        [Display(Name = "Address One")]
        public string? AddressOne { get; set; }

        [Display(Name = "Address Two")]
        public string? AddressTwo { get; set; }

        [Display(Name = "Facebook Page")]
        public string? FacebookPage { get; set; }

        [Display(Name = "Instagram Page")]
        public string? InstagramPage { get; set; }

        [Display(Name = "Twitter Page")]
        public string? TwitterPage { get; set; }

        [Display(Name = "Tiktok Page")]
        public string? TiktokPage { get; set; }

        [Display(Name = "Youtube Channel")]
        public string? YoutubeChannel { get; set; }

        [Display(Name = "Email One")]
        public string? EmailOne { get; set; }


        [Display(Name = "Show Email One In Top")]
        public bool ShowEmailOneInTop { get; set; }

        [Display(Name = "Show Email One In Footer")]
        public bool ShowEmailOneInFooter { get; set; }


        [Display(Name = "Email Two")]
        public string? EmailTwo { get; set; }
        [Display(Name = "Show Email Two In Top")]
        public bool ShowEmailTwoInTop { get; set; }

        [Display(Name = "Show Email Two In Footer")]
        public bool ShowEmailTwoInFooter { get; set; }

        [Display(Name = "Email Three")]
        public string? EmailThree { get; set; }
        [Display(Name = "Show Email Three In Top")]
        public bool ShowEmailThreeInTop { get; set; }

        [Display(Name = "Show Email Three In Footer")]
        public bool ShowEmailThreeInFooter { get; set; }

        [Display(Name = "Phone One")]
        public string? PhoneOne { get; set; }

        [Display(Name = "Show Phone One In Top")]
        public bool ShowPhoneOneInTop { get; set; }

        [Display(Name = "Show Phone One In Footer")]
        public bool ShowPhoneOneInFooter { get; set; }

        [Display(Name = "PhoneTwo")]
        public string? PhoneTwo { get; set; }
        [Display(Name = "Show Phone Two In Top")]
        public bool ShowPhoneTwoInTop { get; set; }

        [Display(Name = "Show Phone Two In Footer")]
        public bool ShowPhoneTwoInFooter { get; set; }

        [Display(Name = "Phone Three")]
        public string? PhoneThree { get; set; }
        [Display(Name = "Show Phone Three In Top")]
        public bool ShowPhoneThreeInTop { get; set; }

        [Display(Name = "Show Phone Three In Footer")]
        public bool ShowPhoneThreeInFooter { get; set; }
        [Display(Name = "Working Hour")]
        public string? WorkingHour { get; set; }


        [Display(Name = "Add Faq To Home")]
        public bool AddFaqToHome { get; set; }

        [Display(Name = "Add Faq To Footer")]
        public bool AddFaqToFooter { get; set; }

        [Display(Name = "Add Testimony To Home")]
        public bool AddTestimonyToHome { get; set; }

        [Display(Name = "Add Testimony To Footer")]
        public bool AddTestimonyToFooter { get; set; }

        [Display(Name = "Add Partner To Home")]
        public bool AddPartnerToHome { get; set; }


        [Display(Name = "Disable Main Top Menu")]
        public bool DisableMainTopMenu { get; set; }

        [Display(Name = "Custom Menu Top")]
        public string? CustomMenuTop { get; set; }

        [Display(Name = "Use Custom Menu Top")]
        public bool UseCustomMenuTop { get; set; }



        [Display(Name = "Show Products In Menu")]
        public bool ShowProductsInMenu { get; set; }


        [Display(Name = "Activate Products In Footer")]
        public bool ShowProductsInFooter { get; set; }


        [Display(Name = "Activate Products In Home")]
        public bool ActivateProductsInHome { get; set; }

        [Display(Name = "Show Three Products")]
        public bool ShowThreeProducts { get; set; }


        [Display(Name = "Show Six Products")]
        public bool ShowSixProducts { get; set; }


        [Display(Name = "Show Two Products")]
        public bool ShowTwoProducts { get; set; }

        [Display(Name = "Show Contact Us Menu")]
        public bool ShowContactUsMenu { get; set; }

        [Display(Name = "Show Contact Us Footer")]
        public bool ShowContactUsFooter { get; set; }


        [Display(Name = "Default Title Background")]
        public string? DefaultTitleBackgroundUrl { get; set; }
        public string? DefaultTitleBackgroundKey { get; set; }



        [Display(Name = "Default Attendance Start Time")]
        public DateTime? DefaultAttendanceStartTime { get; set; }


        [Display(Name = "Default Attendance Close Time")]
        public DateTime? DefaultAttendanceCloseTime { get; set; }

        [Display(Name = "Attendance For Early Minute before time")]
        public int EarlyMinute { get; set; }

        [Display(Name = "Attendance For Ontime Minute before time")]
        public int OntimeMinute { get; set; }

        [Display(Name = "Attendance For Very Minute after time")]
        public int LateMinute { get; set; }

        [Display(Name = "Attendance For Very Late Minute after time")]
        public int VeryLateMinute { get; set; }

        [Display(Name = "Default Attendance Start Time Saturday")]
        public DateTime? DefaultAttendanceStartTimeSaturday { get; set; }


        [Display(Name = "Report Close Time")]
        public DateTime? ReportCloseTime { get; set; }


        [Display(Name = "Job Card Note")]
        public string? JobCardNote { get; set; }

        [Display(Name = "Job Card Title Background")]
        public string? JobCardTitleBackgroundUrl { get; set; }
        public string? JobCardTitleBackgroundKey { get; set; }


        [Display(Name = "Bank Name")]
        public string? BankName { get; set; }

        [Display(Name = "Account Number")]
        public string? AccountNumber { get; set; }

        [Display(Name = "Account Name")]
        public string? AccountName { get; set; }

        [Display(Name = "Signature Name")]
        public string? SignatureName { get; set; }

        [Display(Name = "Signature Title")]
        public string? SignatureTitle { get; set; }


        [Display(Name = "Signature")]
        public string? SignatureUrl { get; set; }
        public string? SignatureKey { get; set; }


        [Display(Name = "Add Blog To Home")]
        public bool AddBlogToHome { get; set; }

        [Display(Name = "Add Blog To Menu")]
        public bool AddBlogToMenu { get; set; }

        [Display(Name = "Add Blog To Footer")]
        public bool AddBlogToFooter { get; set; }


        [Display(Name = "Blog Display Title")]
        public string? BlogDisplayTitle { get; set; }


        [Display(Name = "Add Career To Menu")]
        public bool AddCareerToMenu { get; set; }

        [Display(Name = "Add Career To Footer")]
        public bool AddCareerToFooter { get; set; }


        [Display(Name = "Career Display Title")]
        public string? CareerDisplayTitle { get; set; }


        [Display(Name = "Use Category Blog")]
        public bool UseCategoryBlog { get; set; }

        [Display(Name = "Enable Breaking News Ribon")]
        public bool EnableBreakingNewsRibon { get; set; }
        [Display(Name = "Breaking News Ribon Title")]
        public string? BreakingNewsRibonTitle { get; set; }
        [Display(Name = "Ribon Position")]

        public RibonPosition RibonPosition { get; set; }
    }
}
