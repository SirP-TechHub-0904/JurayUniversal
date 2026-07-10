using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class MisNewsletterContact
    {
        public long Id { get; set; }

        public long MisNewsletterListId { get; set; }
        public MisNewsletterList? MisNewsletterList { get; set; }

        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        [Display(Name = "Active Subscription")]
        public bool IsActive { get; set; } = true;
    }
}
