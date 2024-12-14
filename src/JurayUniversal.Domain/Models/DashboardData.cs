using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class DashboardData
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? Key { get;set; }
        public bool UseAccounts { get; set; }
        public ICollection<DashboardDataList> DashboardDataLists { get; set; }
    }
}
