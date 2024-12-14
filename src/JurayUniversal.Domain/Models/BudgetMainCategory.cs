using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class BudgetMainCategory
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProposalStatus Status { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string UserId { get; set; }
        public Profile User { get; set; }

        public string? AccessEmails { get; set; }

        public ICollection<BudgetSubCategory> SubCategories { get; set; }
    }
}
