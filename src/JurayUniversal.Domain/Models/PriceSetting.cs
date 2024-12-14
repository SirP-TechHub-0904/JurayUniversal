using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class PriceSetting
    {
        
        public long Id { get; set; }
        public string Country { get; set; }
        public string NetworkProvider { get; set; }
        public int DigitCount { get; set; }
        public decimal UnitsPerSms { get; set; }
        public string InternationalDialCode { get; set; }

        public virtual ICollection<DialCode> DialCodes { get; set; }
    }
}
