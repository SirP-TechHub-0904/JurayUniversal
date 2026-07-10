using JurayUniversal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Repository.NotifyRegister
{
    public interface IZeptoMailService
    {
        Task<(bool IsSuccess, string ErrorMessage)> SendEmailAsync(string toEmail, string toName, string subject, string htmlBody, string trackingGuid);
    }
}
