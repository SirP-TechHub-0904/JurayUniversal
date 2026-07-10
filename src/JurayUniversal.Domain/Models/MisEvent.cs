using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisEvent
    {
        public long Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required, StringLength(300)]
        public string Venue { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? BannerImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Links to the overall finance category to track all ticket revenue for this event
        public long? MisFinanceCategoryId { get; set; }
        public MisFinanceCategory? MisFinanceCategory { get; set; }

        public ICollection<MisTicketType> TicketTypes { get; set; } = new List<MisTicketType>();
    }
}
