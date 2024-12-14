using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq; 

namespace JurayUniversal.Domain.Models
{
    public class CompayExpenses
    {
        public long Id { get; set; }

        public string UserId { get; set; }
        public Profile User { get; set; }

        public string Note { get; set; }
        public decimal Amount { get; set; }
        [Display(Name = "Expenses Category")]
        public long ExpensesCategoryId { get; set; }
        public ExpensesCategory ExpensesCategory { get; set; }


        public DateTime Date { get; set; }
        [Display(Name = "Approved By Manager")]
        public bool ApprovedByManager { get; set; }
        [Display(Name = "Manager Note")]
        public string? ManagerNote { get; set; }
        [Display(Name = "Approved By CEO")]
        public bool ApprovedByCEO { get; set; }
        [Display(Name = "CEO Note")]
        public string? CeoNote { get; set; }

        [Display(Name = "Approved By CFO")]
        public bool ApprovedByCFO { get; set; }
        [Display(Name = "CFO Note")]
        public string? CfoNote { get; set; }


        public bool Close { get; set; }


        [Display(Name = "Prove Image")]
        public string? ProveImageUrl { get; set; }
        public string? ProveImageKey { get; set; }
    }
}
