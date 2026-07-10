using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JurayUniversal.Domain.Enum;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisFinanceCategory
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompulsory { get; set; }
        public Enum.Enum.PaymentFrequency Frequency { get; set; }
        public decimal? FixedAmount { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MisFinanceTransaction> Transactions { get; set; }
    }
}
