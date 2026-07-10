using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisApplicantAccount
    {
        public long Id { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Phone, StringLength(50)]
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public ICollection<MisJobApplicant> Applications { get; set; } = new List<MisJobApplicant>();
    }
}
