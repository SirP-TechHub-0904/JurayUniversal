using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class FundSource
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public Profile User { get; set; }
        public string Description { get; set; }

        public ICollection<CompanyFund> CompanyFunds { get; set; }
    }
}
