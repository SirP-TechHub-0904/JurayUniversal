using System;
using System.ComponentModel.DataAnnotations;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class MisFeatureInvoice
    {
        public long Id { get; set; }

        public long MisFeatureId { get; set; }
        public MisFeature MisFeature { get; set; } = null!;

        [Display(Name = "Amount Due")]
        public decimal AmountDue { get; set; }

        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; }

        [Display(Name = "Payment Date")]
        public DateTime? PaymentDate { get; set; }
    }
}
