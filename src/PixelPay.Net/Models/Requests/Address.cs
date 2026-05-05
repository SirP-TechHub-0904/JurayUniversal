using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Models.Requests
{
    public class Address
    {
        public string StreetName { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string PostalZone { get; set; } = string.Empty;
        public string Country { get; set; } = "NG";
    }
}
