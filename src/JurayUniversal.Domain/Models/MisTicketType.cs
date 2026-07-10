using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisTicketType
    {
        public long Id { get; set; }

        public long MisEventId { get; set; }
        public MisEvent MisEvent { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } // e.g., VIP, Regular, Early Bird, Free

        public string? Description { get; set; }

        public decimal Price { get; set; } // 0 means Free

        public int? TotalCapacity { get; set; } // null means unlimited

        public int SoldCount { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public ICollection<MisEventTicket> Tickets { get; set; } = new List<MisEventTicket>();
    }
}
