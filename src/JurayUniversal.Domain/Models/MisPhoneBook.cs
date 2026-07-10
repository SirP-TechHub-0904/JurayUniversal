using System;
using System.Collections.Generic;

namespace JurayUniversal.Domain.Models
{
    public class MisPhoneBook
    {
        public long Id { get; set; }
        
        public string? UserId { get; set; }
        public Profile? User { get; set; }
        
        public string? Name { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MisContact> Contacts { get; set; } = new List<MisContact>();
    }
}
