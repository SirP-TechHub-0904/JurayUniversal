using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisProjectUser
    {
        public long Id { get; set; }

        public long MisProjectId { get; set; }
        public MisProject MisProject { get; set; }

        public string UserId { get; set; }
        public Profile User { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string? Role { get; set; }
    }
}
