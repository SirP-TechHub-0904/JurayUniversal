using System;
using System.ComponentModel.DataAnnotations;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisNewsletterPlanTransaction
    {
        public long Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public Profile? User { get; set; }

        public long MisNewsletterPlanId { get; set; }
        public MisNewsletterPlan? MisNewsletterPlan { get; set; }

        public decimal Amount { get; set; }
        
        public int Quota { get; set; }

        public string Reference { get; set; } = string.Empty;

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string PaymentProvider { get; set; } = "Paystack";

        public bool IsGlobalPurchase { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
    }
}
