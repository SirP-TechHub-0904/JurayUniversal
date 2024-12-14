using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.EntityFrameworkCore;
using static JurayUniversal.Domain.Enum.Enum;
using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using PostmarkEmailService;


namespace JurayUniversal.Application.Repository.NotifyRegister
{
    public class EmailSendService : IEmailSendService
    {
        private readonly DashboardDbContext _context;
        private readonly LoggerLibrary _logger;
        private readonly UserManager<Profile> _userManager;
        private readonly PostmarkClient _postmarkClient;

        public EmailSendService(DashboardDbContext context, LoggerLibrary logger, UserManager<Profile> userManager, PostmarkClient postmarkClient)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _postmarkClient = postmarkClient;
        }
        public async Task<bool> SendEmailWithResult(string name, string to, string cc, string bcc, string subject, string body)
        {
            var settings = await _context.SuperSettings.FirstOrDefaultAsync();
            var normalsettings = await _context.Settings.FirstOrDefaultAsync();
            string emailTemplate = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>//CompanyName//</title>
</head>
<body style=""font-family: Arial, sans-serif; margin: 0; padding: 0;"">

    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0; text-align: center;"">
                <img src=""//YOUR_LOGO_URL//"" alt=""Company Logo"" width=""200"" style=""display: inline-block;"">
            </td>
        </tr>
        <tr>
            <td style=""background-color: #f4f4f4; padding: 20px;"">
<h2 style=""color: #333333;"">//Subject//</h2>
                <h4 style=""color: #333333;text-transform: uppercase;"">Dear //Recipient_Name//,</h4>    



                <p style=""color: #666666;"">
                    //Body//
                </p>
            </td>
        </tr>
        <tr>
            <td style=""background-color: #333333; color: #ffffff; padding: 10px; text-align: center;"">
                <p>&copy; //Date// //CompanyName// | Contact us at: <a href=""mailto://CompanyEmail//"" style=""color: #ffffff; text-decoration: none;"">//CompanyEmail//</a></p>
            </td>
        </tr>
    </table>

</body>
</html>
";
            emailTemplate = emailTemplate.Replace("//CompanyName//", settings.CompanyName);
            emailTemplate = emailTemplate.Replace("//YOUR_LOGO_URL//", settings.CompanyLogoUrl);
            emailTemplate = emailTemplate.Replace("//Subject//", subject);
            emailTemplate = emailTemplate.Replace("//Body//", body);
            emailTemplate = emailTemplate.Replace("//CompanyEmail//", normalsettings.EmailOne ?? "");
            emailTemplate = emailTemplate.Replace("//Date//", DateTime.UtcNow.Year.ToString());
            emailTemplate = emailTemplate.Replace("//Recipient_Name//", name);
            // Replace "YOUR_LOGO_URL" with the actual URL of your logo image.
            // Use the emailTemplate string as needed in your application.

            // Replace these values with your actual SMTP server details
            string smtpServer = "";
            int smtpPort = 0;
            string smtpUsername = "";
            string smtpPassword = "";
            var settingconfig = await _context.DataConfigs.FirstOrDefaultAsync();
            try
            {
                if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Outlook)
                {
                    smtpServer = settingconfig.OutlookServer;
                    smtpPort = settingconfig.OutlookPort;
                    smtpUsername = settingconfig.OutlookUsername;
                    smtpPassword = settingconfig.OutlookSecurity;
                }
                else if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Google)
                {
                    smtpServer = settingconfig.GoogleServer;
                    smtpPort = settingconfig.GooglePort;
                    smtpUsername = settingconfig.GoogleUsername;
                    smtpPassword = settingconfig.GoogleSecurity;
                }
                else if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Webmail)
                {
                    smtpServer = settingconfig.WebmailServer;
                    smtpPort = settingconfig.WebmailPort;
                    smtpUsername = settingconfig.WebmailUsername;
                    smtpPassword = settingconfig.WebmailSecurity;
                }


                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
                {

                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Outlook)
                    {
                        client.EnableSsl = settingconfig.OutlookSSLEnable;
                        client.UseDefaultCredentials = settingconfig.OutlookUseDefaultCredentials;

                    }
                    else if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Google)
                    {
                        client.EnableSsl = settingconfig.GoogleSSLEnable;
                        client.UseDefaultCredentials = settingconfig.GoogleUseDefaultCredentials;

                    }
                    else if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Webmail)
                    {
                        client.EnableSsl = settingconfig.WebmailSSLEnable;
                        client.UseDefaultCredentials = settingconfig.WebmailUseDefaultCredentials;
                    }

                    MailMessage message = new MailMessage();
                    if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Outlook)
                    {
                        message.From = new MailAddress(settingconfig.OutlookSenderEmail, settingconfig.DisplayName);
                    }
                    else if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Google)
                    {

                        message.From = new MailAddress(settingconfig.GoogleSenderEmail, settingconfig.DisplayName);
                    }
                    else if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Webmail)
                    {
                        message.From = new MailAddress(settingconfig.WebmailSenderEmail, settingconfig.DisplayName);
                    }
                    message.To.Add(new MailAddress(to));

                    try
                    {
                        if (cc != null)
                        {
                            //cc emails
                            string[] ccEmails = cc.Split(',');

                            // Add CC recipients
                            foreach (var ccEmail in ccEmails)
                            {
                                message.CC.Add(ccEmail.Trim());
                            }
                        }
                    }
                    catch (Exception c)
                    {
                        _logger.Log($" cc emails {settings.CompanyName} -  {c.ToString()}");
                    }

                    try
                    {
                        //bb emails
                        if (bcc != null)
                        {
                            string[] bccEmails = bcc.Split(',');

                            // Add CC recipients
                            foreach (var bccEmail in bccEmails)
                            {
                                message.Bcc.Add(bccEmail.Trim());
                            }
                        }
                    }
                    catch (Exception c)
                    {
                        _logger.Log($" cc emails {settings.CompanyName} -  {c.ToString()}");
                    }

                    message.Subject = subject;
                    message.Body = emailTemplate;
                    message.IsBodyHtml = true;

                    client.Send(message);
                    return true;
                }

            }
            catch (Exception c)
            {

                _logger.Log($" {settings.CompanyName} -  {c.ToString()}");
                return false;
            }


            //using (var message = new MailMessage())
            //{
            //    message.To.Add(new MailAddress("cto@juray.ng", "receipient name"));
            //    message.From = new MailAddress("noreply@juray.ng", "your name");
            //    message.Subject = "My subject";
            //    message.Body = "My message";
            //    message.IsBodyHtml = false; // change to true if body msg is in html

            //    using (var client = new SmtpClient("smtp.office365.com"))
            //    {
            //        client.UseDefaultCredentials = false;
            //        client.Port = 587;
            //        client.Credentials = new NetworkCredential("noreply@juray.ng", "Nation@123");
            //        client.EnableSsl = true;

            //        try
            //        {
            //            await client.SendMailAsync(message); // Email sent
            //        }
            //        catch (Exception e)
            //        {
            //            // Email not sent, log exception
            //        }
            //    }
            //}

        }

        public async Task SendEmail(string name, string to, string cc, string bcc, string subject, string body)
        {
            var settings = await _context.SuperSettings.FirstOrDefaultAsync();
            var normalsettings = await _context.Settings.FirstOrDefaultAsync();
            string emailTemplate = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>//CompanyName//</title>
</head>
<body style=""font-family: Arial, sans-serif; margin: 0; padding: 0;"">

    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0; text-align: center;"">
                <img src=""//YOUR_LOGO_URL//"" alt=""Company Logo"" width=""200"" style=""display: inline-block;"">
            </td>
        </tr>
        <tr>
            <td style=""background-color: #f4f4f4; padding: 20px;"">
                
<h4 style=""color: #333333;"">//Subject//</h4>
                <h2 style=""color: #333333;"">Hello //Recipient_Name//,</h2>  

                <p style=""color: #666666;"">
                    //Body//
                </p>
            </td>
        </tr>
        <tr>
            <td style=""background-color: #333333; color: #ffffff; padding: 10px; text-align: center;"">
                <p>&copy; //Date// //CompanyName// | Contact us at: <a href=""mailto://CompanyEmail//"" style=""color: #ffffff; text-decoration: none;"">//CompanyEmail//</a></p>
            </td>
        </tr>
    </table>

</body>
</html>
";
            emailTemplate = emailTemplate.Replace("//CompanyName//", settings.CompanyName);
            emailTemplate = emailTemplate.Replace("//YOUR_LOGO_URL//", settings.CompanyLogoUrl);
            emailTemplate = emailTemplate.Replace("//Subject//", subject);
            emailTemplate = emailTemplate.Replace("//Body//", body);
            emailTemplate = emailTemplate.Replace("//CompanyEmail//", normalsettings.EmailOne ?? "");
            emailTemplate = emailTemplate.Replace("//Date//", DateTime.UtcNow.Year.ToString());
            emailTemplate = emailTemplate.Replace("//Recipient_Name//", name);
            // Replace "YOUR_LOGO_URL" with the actual URL of your logo image.
            // Use the emailTemplate string as needed in your application.

            // Replace these values with your actual SMTP server details
            string smtpServer = "";
            int smtpPort = 0;
            string smtpUsername = "";
            string smtpPassword = "";
            var settingconfig = await _context.DataConfigs.FirstOrDefaultAsync();
            try
            {
                if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Outlook)
                {
                    smtpServer = settingconfig.OutlookServer;
                    smtpPort = settingconfig.OutlookPort;
                    smtpUsername = settingconfig.OutlookUsername;
                    smtpPassword = settingconfig.OutlookSecurity;
                }
                else if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Google)
                {
                    smtpServer = settingconfig.GoogleServer;
                    smtpPort = settingconfig.GooglePort;
                    smtpUsername = settingconfig.GoogleUsername;
                    smtpPassword = settingconfig.GoogleSecurity;
                }
                else if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Webmail)
                {
                    smtpServer = settingconfig.WebmailServer;
                    smtpPort = settingconfig.WebmailPort;
                    smtpUsername = settingconfig.WebmailUsername;
                    smtpPassword = settingconfig.WebmailSecurity;
                }


                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
                {

                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Outlook)
                    {
                        client.EnableSsl = settingconfig.OutlookSSLEnable;
                        client.UseDefaultCredentials = settingconfig.OutlookUseDefaultCredentials;

                    }
                    else if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Google)
                    {
                        client.EnableSsl = settingconfig.GoogleSSLEnable;
                        client.UseDefaultCredentials = settingconfig.GoogleUseDefaultCredentials;

                    }
                    else if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Webmail)
                    {
                        client.EnableSsl = settingconfig.WebmailSSLEnable;
                        client.UseDefaultCredentials = settingconfig.WebmailUseDefaultCredentials;
                    }

                    MailMessage message = new MailMessage();
                    if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Outlook)
                    {
                        message.From = new MailAddress(settingconfig.OutlookSenderEmail);
                    }
                    else if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Google)
                    {
                        message.From = new MailAddress(settingconfig.GoogleSenderEmail);
                    }
                    else if (settingconfig.MailSender == Domain.Enum.Enum.MailSender.Webmail)
                    {
                        message.From = new MailAddress(settingconfig.WebmailSenderEmail);
                    }
                    message.To.Add(new MailAddress(to));

                    try
                    {
                        if (cc != null)
                        {
                            //cc emails
                            string[] ccEmails = cc.Split(',');

                            // Add CC recipients
                            foreach (var ccEmail in ccEmails)
                            {
                                message.CC.Add(ccEmail.Trim());
                            }
                        }
                    }
                    catch (Exception c)
                    {
                        _logger.Log($" cc emails {settings.CompanyName} -  {c.ToString()}");
                    }

                    try
                    {
                        //bb emails
                        if (bcc != null)
                        {
                            string[] bccEmails = bcc.Split(',');

                            // Add CC recipients
                            foreach (var bccEmail in bccEmails)
                            {
                                message.Bcc.Add(bccEmail.Trim());
                            }
                        }
                    }
                    catch (Exception c)
                    {
                        _logger.Log($" cc emails {settings.CompanyName} -  {c.ToString()}");
                    }

                    message.Subject = subject;
                    message.Body = emailTemplate;
                    message.IsBodyHtml = true;

                    client.Send(message);
                    try
                    {
                        //get user
                        var updateuser = await _userManager.FindByEmailAsync(to);
                        if (updateuser != null)
                        {
                            updateuser.EmailSent = true;
                            await _userManager.UpdateAsync(updateuser);
                        }

                    }
                    catch (Exception e)
                    {

                    }
                }

            }
            catch (Exception c)
            {

                _logger.Log($" {settings.CompanyName} -  {c.ToString()}");
            }


            //using (var message = new MailMessage())
            //{
            //    message.To.Add(new MailAddress("cto@juray.ng", "receipient name"));
            //    message.From = new MailAddress("noreply@juray.ng", "your name");
            //    message.Subject = "My subject";
            //    message.Body = "My message";
            //    message.IsBodyHtml = false; // change to true if body msg is in html

            //    using (var client = new SmtpClient("smtp.office365.com"))
            //    {
            //        client.UseDefaultCredentials = false;
            //        client.Port = 587;
            //        client.Credentials = new NetworkCredential("noreply@juray.ng", "Nation@123");
            //        client.EnableSsl = true;

            //        try
            //        {
            //            await client.SendMailAsync(message); // Email sent
            //        }
            //        catch (Exception e)
            //        {
            //            // Email not sent, log exception
            //        }
            //    }
            //}

        }

        public async Task SendEmailPostmaster(string name, string to, string cc, string bcc, string subject, string body)
        {

            var settings = await _context.SuperSettings.FirstOrDefaultAsync();
            var normalsettings = await _context.Settings.FirstOrDefaultAsync();
            string emailTemplate = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>//CompanyName//</title>
</head>
<body style=""font-family: Arial, sans-serif; margin: 0; padding: 0;"">

    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0; text-align: center;"">
                <img src=""//YOUR_LOGO_URL//"" alt=""Company Logo"" width=""200"" style=""display: inline-block;"">
            </td>
        </tr>
        <tr>
            <td style=""background-color: #f4f4f4; padding: 20px;"">
<h2 style=""color: #333333;"">//Subject//</h2>
                <h4 style=""color: #333333;text-transform: uppercase;"">Dear //Recipient_Name//,</h4>    



                <p style=""color: #666666;"">
                    //Body//
                </p>
            </td>
        </tr>
        <tr>
            <td style=""background-color: #333333; color: #ffffff; padding: 10px; text-align: center;"">
                <p>&copy; //Date// //CompanyName// | Contact us at: <a href=""mailto://CompanyEmail//"" style=""color: #ffffff; text-decoration: none;"">//CompanyEmail//</a></p>
            </td>
        </tr>
    </table>

</body>
</html>
";
            emailTemplate = emailTemplate.Replace("//CompanyName//", settings.CompanyName);
            emailTemplate = emailTemplate.Replace("//YOUR_LOGO_URL//", settings.CompanyLogoUrl);
            emailTemplate = emailTemplate.Replace("//Subject//", subject);
            emailTemplate = emailTemplate.Replace("//Body//", body);
            emailTemplate = emailTemplate.Replace("//CompanyEmail//", normalsettings.EmailOne ?? "");
            emailTemplate = emailTemplate.Replace("//Date//", DateTime.UtcNow.Year.ToString());
            emailTemplate = emailTemplate.Replace("//Recipient_Name//", name);
            // Replace "YOUR_LOGO_URL" with the actual URL of your logo image.
            // Use the emailTemplate string as needed in your application.

            PostmarkResponse response = new PostmarkResponse();
            var message = new PostmarkMessage
            {
                From = "NIPSS <admin@nipssportal.com.ng>",
                To = to,
                Subject = subject,
                HtmlBody = emailTemplate
            };

            try
            {
                response = await _postmarkClient.SendMessageAsync(message);


                Console.WriteLine("Email sent successfully. Message ID: " + response.MessageID);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                
            }

        }
        public async Task<bool> SendEmailPostmasterReturn(string name, string to, string cc, string bcc, string subject, string body)
        {

            var settings = await _context.SuperSettings.FirstOrDefaultAsync();
            var normalsettings = await _context.Settings.FirstOrDefaultAsync();
            string emailTemplate = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>//CompanyName//</title>
</head>
<body style=""font-family: Arial, sans-serif; margin: 0; padding: 0;"">

    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0; text-align: center;"">
                <img src=""//YOUR_LOGO_URL//"" alt=""Company Logo"" width=""200"" style=""display: inline-block;"">
            </td>
        </tr>
        <tr>
            <td style=""background-color: #f4f4f4; padding: 20px;"">
<h2 style=""color: #333333;"">//Subject//</h2>
                <h4 style=""color: #333333;text-transform: uppercase;"">Dear //Recipient_Name//,</h4>    



                <p style=""color: #666666;"">
                    //Body//
                </p>
            </td>
        </tr>
        <tr>
            <td style=""background-color: #333333; color: #ffffff; padding: 10px; text-align: center;"">
                <p>&copy; //Date// //CompanyName// | Contact us at: <a href=""mailto://CompanyEmail//"" style=""color: #ffffff; text-decoration: none;"">//CompanyEmail//</a></p>
            </td>
        </tr>
    </table>

</body>
</html>
";
            emailTemplate = emailTemplate.Replace("//CompanyName//", settings.CompanyName);
            emailTemplate = emailTemplate.Replace("//YOUR_LOGO_URL//", settings.CompanyLogoUrl);
            emailTemplate = emailTemplate.Replace("//Subject//", subject);
            emailTemplate = emailTemplate.Replace("//Body//", body);
            emailTemplate = emailTemplate.Replace("//CompanyEmail//", normalsettings.EmailOne ?? "");
            emailTemplate = emailTemplate.Replace("//Date//", DateTime.UtcNow.Year.ToString());
            emailTemplate = emailTemplate.Replace("//Recipient_Name//", name);
            // Replace "YOUR_LOGO_URL" with the actual URL of your logo image.
            // Use the emailTemplate string as needed in your application.

            PostmarkResponse response = new PostmarkResponse();
            var message = new PostmarkMessage
            {
                From = "NIPSS <admin@nipssportal.com.ng>",
                To = to,
                Subject = subject,
                HtmlBody = emailTemplate
            };

            try
            {
                response = await _postmarkClient.SendMessageAsync(message);


                Console.WriteLine("Email sent successfully. Message ID: " + response.MessageID);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                return false;
            }

        }

        public async Task SendEmailPostmasterAddFrom(string from, string name, string to, string cc, string bcc, string subject, string body)
        {
            var settings = await _context.SuperSettings.FirstOrDefaultAsync();
            var normalsettings = await _context.Settings.FirstOrDefaultAsync();
            string emailTemplate = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>//CompanyName//</title>
</head>
<body style=""font-family: Arial, sans-serif; margin: 0; padding: 0;"">

    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0; text-align: center;"">
                <img src=""//YOUR_LOGO_URL//"" alt=""Company Logo"" width=""200"" style=""display: inline-block;"">
            </td>
        </tr>
        <tr>
            <td style=""background-color: #f4f4f4; padding: 20px;"">
<h2 style=""color: #333333;"">//Subject//</h2>
                <h4 style=""color: #333333;text-transform: uppercase;"">Dear //Recipient_Name//,</h4>    



                <p style=""color: #666666;"">
                    //Body//
                </p>
            </td>
        </tr>
        <tr>
            <td style=""background-color: #333333; color: #ffffff; padding: 10px; text-align: center;"">
                <p>&copy; //Date// //CompanyName// | Contact us at: <a href=""mailto://CompanyEmail//"" style=""color: #ffffff; text-decoration: none;"">//CompanyEmail//</a></p>
            </td>
        </tr>
    </table>

</body>
</html>
";
            emailTemplate = emailTemplate.Replace("//CompanyName//", settings.CompanyName);
            emailTemplate = emailTemplate.Replace("//YOUR_LOGO_URL//", settings.CompanyLogoUrl);
            emailTemplate = emailTemplate.Replace("//Subject//", subject);
            emailTemplate = emailTemplate.Replace("//Body//", body);
            emailTemplate = emailTemplate.Replace("//CompanyEmail//", normalsettings.EmailOne ?? "");
            emailTemplate = emailTemplate.Replace("//Date//", DateTime.UtcNow.Year.ToString());
            emailTemplate = emailTemplate.Replace("//Recipient_Name//", name);
            // Replace "YOUR_LOGO_URL" with the actual URL of your logo image.
            // Use the emailTemplate string as needed in your application.

            PostmarkResponse response = new PostmarkResponse();
            var message = new PostmarkMessage
            {
                From = from,
                To = to,
                Subject = subject,
                HtmlBody = emailTemplate
            };

            try
            {
                response = await _postmarkClient.SendMessageAsync(message);


                Console.WriteLine("Email sent successfully. Message ID: " + response.MessageID);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);

            }
        }
    }
}
