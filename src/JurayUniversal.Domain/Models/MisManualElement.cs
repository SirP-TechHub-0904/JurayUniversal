using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisManualElement
    {
        [Key]
        public long Id { get; set; }

        public long MisManualPageId { get; set; }
        public MisManualPage MisManualPage { get; set; }

        [Required]
        public string ElementName { get; set; }
        
        public string ElementType { get; set; } // e.g. Button, Table, Form
        
        public string Description { get; set; }

        // Role restriction. e.g. "GlobalEditor", "SystemAdmin", etc.
        public string RequiredRole { get; set; } 
    }
}
