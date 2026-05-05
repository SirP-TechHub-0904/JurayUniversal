using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Models.Requests
{
    public class TaxTotal
    {
        public decimal TaxAmount { get; set; }

        public List<TaxSubTotal> TaxSubtotal { get; set; } = new();
    }

    public class TaxSubTotal
    {
        public decimal TaxableAmount { get; set; }
        public decimal TaxAmount { get; set; }

        public TaxCategory TaxCategory { get; set; } = new();
    }

    public class TaxCategory
    {
        public string Id { get; set; } = "VAT";
        public decimal Percent { get; set; }
    }
    public class InvoiceLine
    {
        public string HsnCode { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;

        public int InvoicedQuantity { get; set; }
        public decimal LineExtensionAmount { get; set; }

        public Item Item { get; set; } = new();
        public Price Price { get; set; } = new();
    }
    public class Item
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class Price
    {
        public decimal PriceAmount { get; set; }
        public int BaseQuantity { get; set; }
        public string PriceUnit { get; set; } = string.Empty;
    }
}
