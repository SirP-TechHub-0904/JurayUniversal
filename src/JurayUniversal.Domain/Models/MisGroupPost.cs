using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisGroupPost
    {
        public long Id { get; set; }

        public long MisGroupId { get; set; }
        public MisGroup MisGroup { get; set; }

        public string UserProfileId { get; set; } = string.Empty;
        public Profile UserProfile { get; set; }

        [Required]
        [Display(Name = "Message Content")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Date Posted")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
