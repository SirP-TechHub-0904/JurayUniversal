using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class MisNewsletterWallet
    {
        public long Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public Profile? User { get; set; }

        [Display(Name = "Email Quota Balance")]
        public int EmailQuotaBalance { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        public ICollection<MisNewsletterWalletTransaction>? Transactions { get; set; }
    }
}
