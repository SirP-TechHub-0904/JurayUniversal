using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class CompanyFund
    {
        public long Id { get; set; }
        [Display(Name = "Fund Type")]
        public FundType FundType { get; set; }
        [Display(Name = "Fund Status")]
        public FundStatus FundStatus { get; set; }
        [Display(Name = "Fund Source")]
        public long FundSourceId { get; set; }
        public FundSource FundSource { get; set; }
        [Display(Name = "Fund Means")]
        public FundMeans FundMeans { get; set; }
        [Display(Name = "Receipt")]
        public string? ReceiptUrl { get; set; }
        public string? ReceiptKey { get; set; }
        public DateTime Date { get; set;}
        public string Note { get; set; }
        public decimal Amount { get; set; }
    }
}
