using PixelPay.Net.Models.Requests;
using PixelPay.Net.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Interfaces
{
    public interface ITransferService
    {
        Task<TransferBankResponse> GetBanksAsync();

        Task<CreateTransferResponse> CreateAsync(CreateTransferRequest request);

        Task<TransferStatusResponse> CheckStatusAsync(TransferStatusRequest request);
    }
}
