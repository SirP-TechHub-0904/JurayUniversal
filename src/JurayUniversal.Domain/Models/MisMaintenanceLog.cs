using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisMaintenanceLog
    {
        public long Id { get; set; }

        public long MisEquipmentId { get; set; }
        public MisEquipment MisEquipment { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Cost { get; set; }

        [Display(Name = "Performed By (Vendor/Mechanic)")]
        public string? PerformedBy { get; set; }
    }
}
