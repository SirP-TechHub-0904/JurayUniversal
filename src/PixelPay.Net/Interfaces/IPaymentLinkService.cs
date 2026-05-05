using PixelPay.Net.Models.Requests;
using PixelPay.Net.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Interfaces
{
    public interface IPaymentLinkService
    {
        Task<PaymentLinkResponse> CreateAsync(CreatePaymentLinkRequest request);

        Task<PaymentLinkResponse> GetStatusAsync(string reference);
    }
}
