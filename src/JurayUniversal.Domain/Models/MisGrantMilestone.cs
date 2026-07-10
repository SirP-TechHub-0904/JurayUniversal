using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisGrantMilestone
    {
        public long Id { get; set; }

        public long MisGrantId { get; set; }
        public MisGrant MisGrant { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; } // e.g., "Tranche 1", "Q3 Report Delivery"

        public decimal ExpectedAmount { get; set; }

        public decimal? AmountReceived { get; set; } // Null if not yet received

        public DateTime ExpectedDate { get; set; }

        public DateTime? ReceivedDate { get; set; }
        
        public string? Note { get; set; }
    }
}
