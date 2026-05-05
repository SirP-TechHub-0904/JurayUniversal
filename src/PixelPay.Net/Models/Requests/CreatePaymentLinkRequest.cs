using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelPay.Net.Models.Requests
{
    public class CreatePaymentLinkRequest
    {
        public decimal Amount { get; set; }

        public string Currency { get; set; } = "NGN";

        public string Reference { get; set; } = string.Empty;

        public int Hours { get; set; }

        // Optional
        public InvoiceRequest? Invoice { get; set; }
    }

    public class CardPaymentRequest
    {
        public string Pan { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
    }
    public class SubmitOtpRequest
    {
        public string SessionId { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
    public class RetryOtpRequest
    {
        public string SessionId { get; set; } = string.Empty;
    }
    public class Authorize3DsRequest
    {
        public string SessionId { get; set; } = string.Empty;
    }
    public class UssdStatusRequest
    {
        public string SessionId { get; set; } = string.Empty;
    }
    public class InitiateUssdRequest
    {
        public string SessionId { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
    }
    public class CreateTransferRequest
    {
        public string Bank { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
    }
    public class TransferStatusRequest
    {
        public string Reference { get; set; } = string.Empty;
    }
    public class ValidateAccountRequest
    {
        public string BankCode { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }

    public class ExecutePayoutRequest
    {
        public string Ref { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string Bank { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Pin { get; set; } = string.Empty;
        public string Narration { get; set; } = string.Empty;
    }
    public class CreateStaticAccountRequest
    {
        public string Bank { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
    }

    public class PayAttitudeRequest
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
    }









}
