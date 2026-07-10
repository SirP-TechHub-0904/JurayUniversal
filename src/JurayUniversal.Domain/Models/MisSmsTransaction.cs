using System;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisSmsTransaction
    {
        public long Id { get; set; }
        
        public decimal Amount { get; set; }
        
        public MisWalletTransactionType TransactionType { get; set; }
        
        public string? Description { get; set; }
        
        public string? Reference { get; set; }

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public string? PaymentProvider { get; set; }

        public string? UserId { get; set; }
        public Profile? User { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
