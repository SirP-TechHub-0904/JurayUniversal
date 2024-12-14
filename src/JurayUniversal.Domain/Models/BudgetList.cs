using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class BudgetList
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public long BudgetSubCategoryId { get; set; }
        public BudgetSubCategory BudgetSubCategory { get; set; }
        public bool Show {  get; set; }
        public decimal AmountInNaira { get; set; }
        public decimal AmountInDollar { get; set; }

        public ProposalStatus Status { get; set; }
    }
}
