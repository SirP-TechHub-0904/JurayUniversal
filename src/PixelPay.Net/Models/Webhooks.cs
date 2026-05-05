using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Models
{
    public class PaymentWebhook
    {
        public string? Id { get; set; }
        public bool AllowInternationalPayments { get; set; }

        public string? ProfileId { get; set; }
        public string? ProductId { get; set; }

        public string? Reference { get; set; }

        public string? Location { get; set; }
        public bool IsInternal { get; set; }

        public string? MerchantName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        public decimal Amount { get; set; }
        public decimal Fee { get; set; }

        public List<FeeBreakdown> FeeBreakdown { get; set; } = new();

        public bool DebitCustomerFee { get; set; }

        public string? PaymentMethod { get; set; }

        public bool SupportCard { get; set; }
        public bool SupportUssd { get; set; }
        public bool SupportPayByTransfer { get; set; }

        public string? Currency { get; set; }

        public string? RedirectUrl { get; set; }
        public bool RedirectFlag { get; set; }

        public bool Notified { get; set; }
        public DateTime? DateNotified { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? ExpireAt { get; set; }

        public bool IsProd { get; set; }
    }

    public class FeeBreakdown
    {
        public string? Txn { get; set; }
        public decimal Amount { get; set; }
    }
}
