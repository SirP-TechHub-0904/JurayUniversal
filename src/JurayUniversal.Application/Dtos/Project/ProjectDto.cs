using JurayUniversal.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;
using System.Xml.Linq;
using System.Web.Mvc;

namespace JurayUniversal.Application.Dtos.Project
{
    public class ProjectDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string UniqueID { get; set; }

        public string Description { get; set; }
        public string? Materials { get; set; }
        public string? Note { get; set; }
        public string? Authentications { get; set; }

        [Display(Name = "Referred By")]
        public string ProfileReferral { get; set; }
        [Display(Name = "Project Category")]
        public string ProjectCategory { get; set; }

        public string? LogoPicture { get; set; }
        public string? Urls { get; set; }
        [Display(Name = "Date Start")]
        public DateTime? DateStarted { get; set; }
        [Display(Name = "Date Finish")]
        public DateTime? DateFinised { get; set; }
        public string? Duration { get;set;}
        [Display(Name = "Last Update Date")]
        public DateTime? LastUpdateDate { get; set; }

        public ProjectStatus ProjectStatus { get; set; }
          public string? Budget { get; set; }
        [Display(Name = "Amount Spent")]
        public string? AmountSpent { get; set; }
        [Display(Name = "Financial Report")]
        public string? FinancialReport { get; set; }
        public ICollection<ProjectFeature> ProjectFeatures { get; set; }
        public ICollection<ProjectTeam> ProjectTeams { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; }
        public ICollection<ProjectFile> ProjectFiles { get; set; }

    }
}
