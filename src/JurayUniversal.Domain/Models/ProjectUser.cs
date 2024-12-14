using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JurayUniversal.Domain.Models
{
    public class ProjectUser
    {
        public long Id { get; set; }
        [Display(Name = "Profile")]
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        [Display(Name = "Project Team")]
         public long ProjectTeamId { get; set; }
        public ProjectTeam ProjectTeam { get; set; }
        [Display(Name = "Position")]
        public long? ProjectPositionId { get; set; }
        public ProjectPosition ProjectPosition { get; set;}
        [Display(Name = "Date Joined")]
        public DateTime DateJoined { get;set;}
        [Display(Name = "Date Left")]
        public DateTime? DateLeft { get;set;}

        public string? RoleId { get;set;} 
        public IdentityRole Role { get;set;} 
    }
}
