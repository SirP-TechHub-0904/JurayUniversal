using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Models.Responses
{
    public class PaymentLinkResponse : PixelPayBaseResponse
    {
        public string? Reference { get; set; }

        public string? Currency { get; set; }

        public string? SessionId { get; set; }
    }
}
