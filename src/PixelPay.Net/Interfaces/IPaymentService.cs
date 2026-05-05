using PixelPay.Net.Models.Requests;
using PixelPay.Net.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Interfaces
{
    public interface IPaymentService
    {
        Task<InitiatePaymentResponse> InitiateAsync(string paymentLinkId);

        Task<CardPaymentResponse> PayAsync(string sessionId, CardPaymentRequest request);

        Task<CardPaymentResponse> SubmitOtpAsync(SubmitOtpRequest request);

        Task<CardPaymentResponse> RetryOtpAsync(RetryOtpRequest request);

        Task<CardPaymentResponse> Authorize3DsAsync(Authorize3DsRequest request);

        Task<ConfirmPaymentResponse> ConfirmAsync(string sessionId);
    }
}
