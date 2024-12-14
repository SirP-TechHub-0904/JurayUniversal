using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class DashboardDataList
    {
        public long Id { get; set; }
        public long DataIdQuery { get; set; }

        public long? DashboardDataId { get;set; }
        public DashboardData DashboardData { get; set; }
    }
}
