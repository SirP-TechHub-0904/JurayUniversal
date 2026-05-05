using PixelPay.Net.Models.Requests;
using PixelPay.Net.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Interfaces
{
    public interface IStaticAccountService
    {
        Task<StaticBankResponse> GetBanksAsync();

        Task<StaticAccountListResponse> GetAccountsAsync(int page = 0, int size = 20);

        Task<CreateStaticAccountResponse> CreateAsync(CreateStaticAccountRequest request);
    }
}
