using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class MisNewsletterList
    {
        public long Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public Profile? User { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MisNewsletterContact>? Contacts { get; set; }
    }
}
