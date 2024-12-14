using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class DialCode
    {
        public long Id { get; set; }
        public string NumberPrefix { get; set; }
        public int PriceSettingId { get; set; }

        public PriceSetting PriceSetting { get; set; }
    }
}
