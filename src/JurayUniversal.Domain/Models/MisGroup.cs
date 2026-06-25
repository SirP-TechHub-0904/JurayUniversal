using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisGroup
    {
        public long Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Group Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<MisGroupMember> Members { get; set; } = new List<MisGroupMember>();
        public virtual ICollection<MisGroupPost> Posts { get; set; } = new List<MisGroupPost>();
        public virtual ICollection<MisGroupFile> Files { get; set; } = new List<MisGroupFile>();
    }
}
