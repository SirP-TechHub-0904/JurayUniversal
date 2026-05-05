using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Models.Requests
{
    public class Party
    {
        public string PartyName { get; set; } = string.Empty;
        public string Tin { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;

        public Address PostalAddress { get; set; } = new();
    }
}
