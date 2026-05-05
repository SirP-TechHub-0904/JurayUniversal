using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class Payment
    {
        public long Id { get; set; }

        public string UserId { get; set; }
        public Profile User { get; set; }

        public long PaymentCategoryId { get; set; }
        public PaymentCategory PaymentCategory { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime DatePaid { get; set; } = DateTime.UtcNow;

        // Paystack
        public string PaymentReference { get; set; } // Paystack reference
        public string TransactionId { get; set; } // optional

        public PaymentStatus Status { get; set; }
        public string Note { get; set; }

        public string? Fullname { get; set; }   // optional
        public string? Email { get; set; }      // optional
        public string? Phone { get; set; }
    }
}
