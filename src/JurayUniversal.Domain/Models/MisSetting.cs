using System;
using System.ComponentModel.DataAnnotations;

namespace JurayUniversal.Domain.Models
{
    public class MisSetting
    {
        public long Id { get; set; }
        
        [Display(Name = "Category Alias")]
        public string? CategoryAlias { get; set; }

        [Display(Name = "Sub-Category Alias")]
        public string? SubCategoryAlias { get; set; }

        [Display(Name = "Enable List Download")]
        public bool EnableListDownload { get; set; }

        [Display(Name = "Group/Committee Alias")]
        public string? GroupAlias { get; set; }

        [Display(Name = "Alumni/Past Member Alias")]
        public string? AlumniAlias { get; set; } = "Alumni";

        // --- PixelPay Settings ---
        [Display(Name = "Enable PixelPay")]
        public bool EnablePixelPay { get; set; } = false;

        [Display(Name = "PixelPay Base URL")]
        public string? PixelPayBaseUrl { get; set; } = "https://core.mypixelpay.com";

        [Display(Name = "PixelPay Client ID (systemid)")]
        public string? PixelPayKeyId { get; set; }

        [Display(Name = "PixelPay Secret Key (token)")]
        public string? PixelPaySecretKey { get; set; }

        [Display(Name = "PixelPay Signature")]
        public string? PixelPaySignature { get; set; }

        [Display(Name = "PixelPay Currency")]
        public string? PixelPayCurrency { get; set; } = "NGN";

        [Display(Name = "PixelPay Webhook Secret")]
        public string? PixelPayWebhookSecret { get; set; }

        // --- Paystack Settings ---
        [Display(Name = "Enable Paystack")]
        public bool EnablePaystack { get; set; } = false;

        [Display(Name = "Paystack Public Key")]
        public string? PaystackPublicKey { get; set; }

        [Display(Name = "Paystack Secret Key")]
        public string? PaystackSecretKey { get; set; }

        [Display(Name = "Paystack Webhook Secret")]
        public string? PaystackWebhookSecret { get; set; }

        // --- SMS / Mass Comm Settings ---
        [Display(Name = "Company SMS Wallet Balance")]
        public decimal SmsCreditBalance { get; set; }

        // --- Email Newsletter Settings ---
        [Display(Name = "Company Newsletter Quota")]
        public int GlobalNewsletterQuota { get; set; }

        [Display(Name = "Company Newsletter Quota Expiry")]
        public DateTime? GlobalNewsletterQuotaExpiry { get; set; }

        [Display(Name = "Enable ZeptoMail")]
        public bool EnableZeptoMail { get; set; } = true;

        [Display(Name = "ZeptoMail API Key")]
        public string? ZeptoMailApiKey { get; set; }

        [Display(Name = "ZeptoMail SendMail ID")]
        public string? ZeptoMailSendMailId { get; set; }

        [Display(Name = "ZeptoMail Sender Email")]
        public string? ZeptoMailSenderEmail { get; set; }

        [Display(Name = "ZeptoMail Sender Name")]
        public string? ZeptoMailSenderName { get; set; }

        [Display(Name = "Max Email Retries")]
        public int EmailMaxRetries { get; set; } = 5;

        // --- MisSmartAssist (BYOK AI) Settings ---
        [Display(Name = "Active AI Provider")]
        public Enum.Enum.MisAiProvider ActiveAiProvider { get; set; } = Enum.Enum.MisAiProvider.None;

        [Display(Name = "Google Gemini API Key")]
        public string? GeminiApiKey { get; set; }

        [Display(Name = "OpenAI (ChatGPT) API Key")]
        public string? OpenAIApiKey { get; set; }

        [Display(Name = "Anthropic (Claude) API Key")]
        public string? AnthropicApiKey { get; set; }

        // --- Captcha Settings ---
        [Display(Name = "Active Captcha")]
        public JurayUniversal.Domain.Enum.Enum.CaptchaType ActiveCaptcha { get; set; } = JurayUniversal.Domain.Enum.Enum.CaptchaType.None;

        [Display(Name = "Google reCAPTCHA Site Key")]
        public string? GoogleRecaptchaSiteKey { get; set; }

        [Display(Name = "Google reCAPTCHA Secret Key")]
        public string? GoogleRecaptchaSecretKey { get; set; }
    }
}
