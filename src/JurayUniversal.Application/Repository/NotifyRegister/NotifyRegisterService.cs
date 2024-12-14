using JurayUniversal.Application.Dtos.AwsDtos;
using JurayUniversal.Application.Dtos.NotifyDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;

namespace JurayUniversal.Application.Repository.NotifyRegister
{
    public class NotifyRegisterService : INotifyRegisterService
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public NotifyRegisterService(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }
        public async Task RegisterNotification(NotifyDto obj)
        {
            var settings = await _context.SuperSettings.FirstOrDefaultAsync();
            if (obj.IsEmail == true)
            {
                //get the email template and assign it

                string template = settings.EmailTemplate;
                if(template == null)
                {
                    template = "{title}<br>{subtitle}<br><br>{name}<br>{MESSAGE}";
                }
                template = template.Replace("{title}", obj.Title);
                template = template.Replace("{subtitle}", obj.NotificationTitle);
                template = template.Replace("{name}", obj.Name);
                template = template.Replace("{MESSAGE}", obj.Content);

                MailingSystem ms = new MailingSystem();
                ms.Receipient = obj.Receipient;
                ms.Title = obj.Title;
                ms.Content = template;
                ms.Retries = 0; ms.NotificationType = NotificationType.Email;
                ms.NotificationStatus = NotificationStatus.NotSent;
                _context.MailingSystems.Add(ms);


            }
            else
            {
                string messagecontent = settings.SMSTemplate;
                string smstemplate = settings.SMSTemplate;
                if (smstemplate == null)
                {
                    smstemplate = "{MESSAGE}";
                }
                smstemplate = smstemplate.Replace("{MESSAGE}", obj.Content);

                MailingSystem ms = new MailingSystem();
                ms.Receipient = obj.Receipient;
                ms.Title = obj.Title;
                ms.Content = smstemplate;
                ms.Retries = 0; ms.NotificationType = NotificationType.SMS;
                ms.NotificationStatus = NotificationStatus.NotSent;
                _context.MailingSystems.Add(ms);

            }
            await _context.SaveChangesAsync();
        }
    }
}
