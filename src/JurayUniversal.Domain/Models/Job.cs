using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JurayUniversal.Domain.Models
{
    public class Job
    {
        public long Id { get; set; }
        [Display(Name = "Your Fullname")]
        public string Title { get; set; }
        [Display(Name = "Kindly tell us about your business")]
        public string Description { get; set; }

        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Contact Email")]
        public string? ContactEmail { get; set; }
        [Display(Name = "Contact PhoneNumber")]
        public string? ContactPhoneNumber { get; set; }
        [Display(Name = "Office Email")]
        public string? OfficeEmail { get; set; }
        [Display(Name = "Office PhoneNumber")]
        public string? OfficePhoneNumber { get; set; }

        public DateTime Date { get; set; }
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Reminder Date")]
        public DateTime? ReminderDate { get; set; }
        [Display(Name = "Expiring Date")]
        public DateTime? ExpiringDate { get; set; }
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }
        [Display(Name = "How would you want your website address to be. (e.g .org .com .ng .com.ng .net)")]
        public string? RequestDomain { get; set; }
        [Display(Name = "Approved Domain")]
        public string? ApprovedDomain { get; set; }
        [Display(Name = "Describe Organization")]
        public string? DescribeOrganization { get; set; }
        public string? Objectives { get; set; }
        [Display(Name = "What is the color of your business")]
        public string? Colors { get; set; }
        [Display(Name = "Tell us three (3) website you like. (this is to help us know the type of website you like)")]
        public string? WebsiteOnMind { get; set; }
        [Display(Name = "Request Start Date")]
        public DateTime RequestStartDate { get; set; }
        [Display(Name = "Request Finish Date")]
        public DateTime RequestFinishDate { get; set; }

        public ICollection<JobFeature> Features { get; set; }
        [Display(Name = "Do You Have a Logo?")]
        public bool DoYouHaveLogo {get;set;}
        [Display(Name = "Would you want us to redesign your logo?")]
        public bool DoWeDoLogo { get;set;}
        [Display(Name = "Job Category")]
        public long? JobCategoryId { get; set; }
        public JobCategory JobCategory { get; set; }
        public string UniqeCode { get; set; }
        public string? ProfileId { get; set; }
        public Profile Profile { get; set; }
      public bool InformationVerified { get; set; }
      public bool IHaveMadePayment { get; set; }
    }
}
