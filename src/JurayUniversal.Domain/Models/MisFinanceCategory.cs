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
        public string? Key { get; set; }
        public bool AllowGuestPayment { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Target Group / Branch / Community")]
        public long? MisGroupId { get; set; }
        public virtual MisGroup? MisGroup { get; set; }

        [Display(Name = "Target Member Category")]
        public long? UserCategoryId { get; set; }
        public virtual UserCategory? UserCategory { get; set; }

        public ICollection<MisFinanceTransaction> Transactions { get; set; }
    }
}
