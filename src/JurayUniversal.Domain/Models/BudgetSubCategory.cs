using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class BudgetSubCategory
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Show {  get; set; }

        public long BudgetMainCategoryId {  get; set; }
        public BudgetMainCategory BudgetMainCategory { get; set; }

        public ICollection<BudgetList> BudgetList { get; set; }
    }
}
