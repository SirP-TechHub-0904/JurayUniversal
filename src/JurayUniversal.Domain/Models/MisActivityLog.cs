using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisActivityLog
    {
        public long Id { get; set; }

        [Required]
        public string ActionType { get; set; } 

        [Required]
        public string Description { get; set; } 

        public string? PerformedByProfileId { get; set; }
        public string? PerformedByFullName { get; set; }

        public string? TargetProfileId { get; set; }
        public string? TargetFullName { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
        
        public string? Module { get; set; } 
    }
}
