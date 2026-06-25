using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JurayUniversal.Domain.Models
{
    public class MisGroupMember
    {
        public long Id { get; set; }

        public long MisGroupId { get; set; }
        public MisGroup MisGroup { get; set; }

        public string UserProfileId { get; set; } = string.Empty;
        public Profile UserProfile { get; set; }

        [StringLength(100)]
        [Display(Name = "Role / Position")]
        public string? RolePosition { get; set; } // e.g. "Chairman", "Secretary", "Member"

        public string MembershipStatus { get; set; } = "Active"; // Active, Pending, Declined

        [Display(Name = "Date Joined")]
        public DateTime DateJoined { get; set; } = DateTime.UtcNow;
    }
}
