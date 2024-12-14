using System.ComponentModel.DataAnnotations;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class ProjectMain
    {
        public long Id { get; set; }
        public string Title { get; set; }
        [Display(Name = "Unique ID")]
        public string UniqueId { get; set; }
        public string Description { get; set; }
        public string? Materials { get; set; }
        public string? Note { get; set; }
        [Display(Name = "Referred By")]
        public string ProfileReferralId { get; set; }
        public Profile ProfileReferral { get; set; }
        [Display(Name = "Project Category")]
        public long ProjectCategoryId { get; set; }
        public ProjectCategory ProjectCategory { get; set; }

        public string? Urls { get; set; }
        public string? Authentications { get; set; }
        public decimal Budget { get; set; }
        [Display(Name = "Amount Spent")]
        public decimal AmountSpent { get; set; }
        [Display(Name = "Financial Report")]
        public string? FinancialReport { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Date Start")]
        public DateTime? DateStarted { get; set; }
        [Display(Name = "Date Finish")]
        public DateTime? DateFinised { get; set; }
        [Display(Name = "Last Update")]
        public DateTime? LastDateUpdated { get; set; }

        public string? LogoUrl { get; set; }
        public string? LogoKey { get; set; }

        [Display(Name = "Project Status")]
        public ProjectStatus ProjectStatus { get; set; }
        public ICollection<ProjectFeature> ProjectFeatures { get; set; }
        public ICollection<ProjectTeam> ProjectTeams { get; set; }
        public ICollection<ProjectFile> ProjectFiles { get; set; }


    }
}
