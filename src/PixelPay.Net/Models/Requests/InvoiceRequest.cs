using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Models.Requests
{
    public class InvoiceRequest
    {
        public string BusinessId { get; set; } = string.Empty;
        public string Irn { get; set; } = string.Empty;
        public string IssueDate { get; set; } = string.Empty;

        public string InvoiceTypeCode { get; set; } = "396";

        public string DocumentCurrencyCode { get; set; } = "NGN";
        public string TaxCurrencyCode { get; set; } = "NGN";

        public string PaymentStatus { get; set; } = "PENDING";
        public string InvoiceKind { get; set; } = "B2B";

        public Party AccountingSupplierParty { get; set; } = new();
        public Party AccountingCustomerParty { get; set; } = new();

        public LegalMonetaryTotal LegalMonetaryTotal { get; set; } = new();

        public List<TaxTotal> TaxTotal { get; set; } = new();
        public List<InvoiceLine> InvoiceLine { get; set; } = new();
    }
}
