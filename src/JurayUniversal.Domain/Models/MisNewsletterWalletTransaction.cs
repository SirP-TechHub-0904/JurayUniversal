using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JurayUniversal.Domain.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisNewsletterWalletTransaction
    {
        public long Id { get; set; }

        public long MisNewsletterWalletId { get; set; }
        public MisNewsletterWallet? MisNewsletterWallet { get; set; }

        [Display(Name = "Amount (Quota)")]
        public int Amount { get; set; }

        [Display(Name = "Transaction Type")]
        public Enum.Enum.MisWalletTransactionType TransactionType { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
