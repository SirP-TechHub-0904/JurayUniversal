using System;

namespace JurayUniversal.Domain.Models
{
    public class MisContact
    {
        public long Id { get; set; }
        
        public long MisPhoneBookId { get; set; }
        public MisPhoneBook? MisPhoneBook { get; set; }
        
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
