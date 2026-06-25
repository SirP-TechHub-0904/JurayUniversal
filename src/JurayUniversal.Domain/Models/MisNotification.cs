using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisNotification
    {
        public long Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public Profile User { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        public string ActionUrl { get; set; }

        public bool IsRead { get; set; }

        public string Module { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
