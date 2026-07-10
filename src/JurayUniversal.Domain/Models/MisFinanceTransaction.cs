using System;
using System.Collections.Generic;
using JurayUniversal.Domain.Enum;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisFinanceTransaction
    {
        public long Id { get; set; }
        public long MisFinanceCategoryId { get; set; }
        public MisFinanceCategory Category { get; set; }

        public string? UserId { get; set; }
        public Profile? UserProfile { get; set; }

        // For public/guest payments
        public string? DonorName { get; set; }
        public string? DonorEmail { get; set; }
        public string? DonorPhone { get; set; }

        public decimal Amount { get; set; }
        public string TransactionReference { get; set; }
        public string? PaymentProvider { get; set; } // "PixelPay" | "Paystack"

        public Enum.Enum.PaymentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }

        public string? Notes { get; set; }
    }
}
