using JurayUniversal.Application.Dtos;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Repository.NotifyRegister
{
    public class SmsSendService : ISmsSendService
    {

        private readonly UserManager<Profile> _userManager;

        public SmsSendService(UserManager<Profile> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> SendSmsWithResult(string contact, string message)
        {
            SmsResponse responsed = new SmsResponse();
            string apiToken = "JeFaYNc8ZzvfDWQ3yB0HSml6jxnLwurVAkTKdMPtb715o4piUEq2RChgIXOGs9";

            try
            {
                var clientsend = new HttpClient();
                var smsrequest = new HttpRequestMessage(HttpMethod.Post, "https://my.kudisms.net/api/sms");
                var smscontent = new MultipartFormDataContent();
                smscontent.Add(new StringContent(apiToken), "token");
                smscontent.Add(new StringContent("NIPSS KURU"), "senderID");
                smscontent.Add(new StringContent(contact), "recipients");
                smscontent.Add(new StringContent(message), "message");
                smscontent.Add(new StringContent("1"), "gateway");
                smsrequest.Content = smscontent;
                var xresponse = await clientsend.SendAsync(smsrequest);
                xresponse.EnsureSuccessStatusCode();
                string responseBody = await xresponse.Content.ReadAsStringAsync();
                responsed = JsonConvert.DeserializeObject<SmsResponse>(responseBody);
                if (responsed.status.ToLower().Contains("success"))
                {
                    return true;
                }
                return false;
                //try
                //{
                //    //get user
                //    string last10DigitsPhoneNumber1 = contact.Substring(Math.Max(0, contact.Length - 10));
                //    var updateuser = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber.Contains(last10DigitsPhoneNumber1));
                //    if (updateuser != null)
                //    {
                //        updateuser.EmailSent = true;
                //        await _userManager.UpdateAsync(updateuser);
                //    }

                //}
                //catch (Exception e)
                //{

                //}
            }
            catch (Exception c)
            {
                return false;
            }


        }

        public async Task SendSms(string contact, string message)
        {
            SmsResponse responsed = new SmsResponse();
            string apiToken = "JeFaYNc8ZzvfDWQ3yB0HSml6jxnLwurVAkTKdMPtb715o4piUEq2RChgIXOGs9";

            try
            {
                var clientsend = new HttpClient();
                var smsrequest = new HttpRequestMessage(HttpMethod.Post, "https://my.kudisms.net/api/sms");
                var smscontent = new MultipartFormDataContent();
                smscontent.Add(new StringContent(apiToken), "token");
                smscontent.Add(new StringContent("NIPSS KURU"), "senderID");
                smscontent.Add(new StringContent(contact), "recipients");
                smscontent.Add(new StringContent(message), "message");
                smscontent.Add(new StringContent("1"), "gateway");
                smsrequest.Content = smscontent;
                var xresponse = await clientsend.SendAsync(smsrequest);
                xresponse.EnsureSuccessStatusCode();
                string responseBody = await xresponse.Content.ReadAsStringAsync();
                responsed = JsonConvert.DeserializeObject<SmsResponse>(responseBody);
                 
            }
            catch (Exception c)
            {

            }


        }
    }
}
