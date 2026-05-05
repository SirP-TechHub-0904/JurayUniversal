using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Models.Responses
{
    public class PixelPayBaseResponse
    {
        public int? Code { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }
    }

    public class InitiatePaymentResponse
    {
        public string? Status { get; set; }
        public string? SessionId { get; set; }
        public decimal Amount { get; set; }
        public decimal CardFee { get; set; }
        public decimal ConvenienceFee { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Currency { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
    public class CardPaymentResponse
    {
        public string? Status { get; set; }
        public string? ResponseCode { get; set; }
        public string? Message { get; set; }
        public string? AuthMethod { get; set; }
        public string? TransactionId { get; set; }
        public string? TransactionRef { get; set; }
        public RedirectDataResponse? RedirectData { get; set; }
    }

    public class RedirectDataResponse
    {
        public string? Jwt { get; set; }
        public string? MD { get; set; }
        public string? ACSUrl { get; set; }
        public string? TermUrl { get; set; }
        public string? EciFlag { get; set; }
    }
    public class ConfirmPaymentResponse
    {
        public string? Status { get; set; }
        public string? ResponseCode { get; set; }
        public string? TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? CardMask { get; set; }
        public string? CardScheme { get; set; }
        public string? MerchantReference { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? SettlementDate { get; set; }
    }
    public class UssdBankResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public List<UssdBank> UssdBank { get; set; } = new();
    }

    public class UssdBank
    {
        public string BankCode { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
    }
   

    public class InitiateUssdResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public string? SessionId { get; set; }
        public decimal Total { get; set; }
        public DateTime? Expiry { get; set; }
    }
    public class UssdStatusResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public string? Response { get; set; } // 00, 06, 05
        public string? RedirectUrl { get; set; }
    }
    public class TransferBankResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public List<string> Banks { get; set; } = new();
    }

   
    public class CreateTransferResponse
    {
        public int Code { get; set; }

        public string? WalletNumber { get; set; }
        public string? AccountName { get; set; }
        public string? AccountBank { get; set; }

        public decimal Amount { get; set; }
        public decimal Fee { get; set; }

        public string? Reference { get; set; }

        public string? TimeoutInMins { get; set; }

        public string? Error { get; set; }

        public bool Redirect { get; set; }
    }
  
    public class TransferStatusResponse
    {
        public int Code { get; set; }

        public string? Error { get; set; }

        public string? WalletNumber { get; set; }

        public decimal? Amount { get; set; }
    }
    public class PayoutBankListResponse
    {
        public List<PayoutBank> Banks { get; set; } = new();
    }

    public class PayoutBank
    {
        public string? BankCode { get; set; }
        public string? BankName { get; set; }
    }
   
    public class ValidateAccountResponse
    {
        public bool IsValid { get; set; }
        public string? AccountName { get; set; }
        public string? BankName { get; set; }
        public string? Message { get; set; }
    }
   

    public class ExecutePayoutResponse
    {
        public string? Status { get; set; }
        public string? Message { get; set; }

        public PayoutData? Data { get; set; }
    }

    public class PayoutData
    {
        public string? TransactionRef { get; set; }
        public string? ProviderRef { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? BeneficiaryAccount { get; set; }
        public string? BeneficiaryBank { get; set; }
        public string? Status { get; set; }
    }
    public class WalletsResponse
    {
        public int Status { get; set; }
        public string? Message { get; set; }

        public List<Wallet> Wallets { get; set; } = new();

        public int Count { get; set; }
        public int Total { get; set; }

        public string? Error { get; set; }
    }

    public class Wallet
    {
        public string? Id { get; set; }
        public string? ProfileId { get; set; }
        public string? Currency { get; set; }
        public string? Symbol { get; set; }
        public decimal Amount { get; set; }
        public string? LastUpdated { get; set; }
    }

    public class PayoutStatusResponse
    {
        public int Code { get; set; }
        public string? ResponseCode { get; set; }

        public string? BankCode { get; set; }
        public string? AccountNumber { get; set; }
        public string? Beneficiary { get; set; }

        public decimal Amount { get; set; }
        public decimal Fee { get; set; }

        public string? Message { get; set; }
        public string? Error { get; set; }
    }
    public class StaticAccountListResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public List<StaticAccount> Data { get; set; } = new();
    }

    public class StaticAccount
    {
        public string? Id { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? BankName { get; set; }
        public string? BankCode { get; set; }
        public string? Reference { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }

        public DateTime? DateCreated { get; set; }
    }

    public class StaticBankResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public List<string> Banks { get; set; } = new();

        public string? BankMode { get; set; }
    }
   

    public class CreateStaticAccountResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public StaticAccount? Data { get; set; }
    }

   
    public class PayAttitudeResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public string? Response { get; set; } // "00"
        public string? RedirectUrl { get; set; }
        public string? SessionId { get; set; }
    }












}
