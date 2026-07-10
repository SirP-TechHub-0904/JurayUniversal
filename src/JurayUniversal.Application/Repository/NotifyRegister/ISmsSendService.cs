using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Repository.NotifyRegister
{
    public enum SmsSendResult
    {
        Success,
        InsufficientKudiBalance,
        Failed
    }

    public interface ISmsSendService
    {
        Task SendSms(string contact, string message);
        Task<bool> SendSmsWithResult(string contact, string message);
        Task<SmsSendResult> SendCampaignSmsAsync(string contact, string message, string senderId);
    }
}
