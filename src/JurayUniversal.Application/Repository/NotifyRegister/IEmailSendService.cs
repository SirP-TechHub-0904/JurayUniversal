using JurayUniversal.Application.Dtos.NotifyDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Repository.NotifyRegister
{
    public interface IEmailSendService
    {
        Task SendEmail(string name, string to, string cc, string bcc, string subject, string body);
        Task SendEmailPostmaster(string name, string to, string cc, string bcc, string subject, string body);
        Task SendEmailPostmasterAddFrom(string from, string name, string to, string cc, string bcc, string subject, string body);
        Task<bool> SendEmailWithResult(string name, string to, string cc, string bcc, string subject, string body);
        Task<bool> SendEmailPostmasterReturn(string name, string to, string cc, string bcc, string subject, string body);
    }
}
