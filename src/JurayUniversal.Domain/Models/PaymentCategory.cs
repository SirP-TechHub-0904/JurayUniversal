using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class PaymentCategory
    {
        public long Id { get; set; }

        public string Name { get; set; } // e.g. Monthly Dues, Levy, Contribution
        public string Description { get; set; }

        public decimal Amount { get; set; } // expected amount

        public int Year { get; set; }
        public int? Month { get; set; } // optional (for monthly dues)

        public DateTime DueDate { get; set; }

        public bool IsCompulsory { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
