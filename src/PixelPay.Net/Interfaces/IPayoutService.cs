using PixelPay.Net.Models.Requests;
using PixelPay.Net.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Interfaces
{
    public interface IPayoutService
    {
        Task<PayoutBankListResponse> GetBanksAsync();

        Task<ValidateAccountResponse> ValidateAccountAsync(ValidateAccountRequest request);

        Task<ExecutePayoutResponse> ExecuteAsync(ExecutePayoutRequest request);

        Task<WalletsResponse> GetWalletsAsync();

        Task<PayoutStatusResponse> GetStatusAsync(string reference);
    }
}
