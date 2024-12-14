using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SmsPlatform.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmsPlatform.Repositories
{
    public class SmsRepository : ISmsRepository
    {

        private readonly DashboardDbContext _context;


        public SmsRepository(DashboardDbContext context)
        {
            _context = context;

        }

        public async Task<bool> HasBlackListedWords(string input, string blackLists)
        {
            Regex wordFilter = new Regex(blackLists);
            return await Task.FromResult(wordFilter.IsMatch(input));
        }

        public async Task<List<string>> RemoveDuplicates(string input)
        {
            input = input.Replace("\r\n", ",");
            IList<string> numbers = input.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            return await Task.FromResult(numbers.Distinct().ToList());
        }

        public async Task<List<string>> FormatNumbers(List<string> numbers)
        {
            List<string> formatedNumbers = new List<string>();

            var codes = await _context.DialCodes.Include(x => x.PriceSetting).ToListAsync();

            foreach (var code in codes)
            {
                var getNumbers = numbers.Where(x => x.StartsWith(code.NumberPrefix));
                foreach (var item in getNumbers.ToList())
                {
                    string newItem;
                    if (item.StartsWith("0"))
                    {
                        newItem = item.Substring(1);
                        newItem = code.PriceSetting.InternationalDialCode + newItem;
                    }
                    else
                    {
                        newItem = code.PriceSetting.InternationalDialCode + item;
                    }

                    formatedNumbers.Add(newItem);
                    numbers.Remove(item);
                }
            }

            if (numbers.Count() > 0)
            {
                formatedNumbers.AddRange(numbers);
            }

            return formatedNumbers;
        }

        public async Task<decimal> UnitsPerPage(List<string> numbers)
        {
            decimal units = 0;

            var settings = await _context.SmsSettings.AsNoTracking().FirstOrDefaultAsync();
            var codes = await _context.DialCodes.Include(x => x.PriceSetting).ToListAsync();

            foreach (var item in codes)
            {
                var selectNumbers = numbers.Where(x => x.StartsWith(item.PriceSetting.InternationalDialCode + RemoveZero(item.NumberPrefix)));
                units += selectNumbers.Count() * item.PriceSetting.UnitsPerSms;

                foreach (var number in selectNumbers.ToList())
                {
                    numbers.Remove(number);
                }
            }

            if (numbers.Count() > 0)
            {
                units += numbers.Count() * settings.FlatUnitsPerSms;
            }

            return units;
        }

        public async Task<string> RemoveZero(string number)
        {
            if (number.StartsWith("0"))
            {
                number = number.Substring(1);
            }
            return await Task.FromResult(number);
        }

        public async Task<int> CountPage(string input)
        {
            int remainder, inputCount = Math.DivRem(input.Length, 160, out remainder);
            return await Task.FromResult(remainder > 0 ? inputCount + 1 : inputCount);
        }


        public async Task<SmsResponse> SendSmsViaKudi(string senderId, string message, string recipients)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://my.kudisms.net/api/sms");
            var content = new MultipartFormDataContent();
            content.Add(new StringContent("JeFaYNc8ZzvfDWQ3yB0HSml6jxnLwurVAkTKdMPtb715o4piUEq2RChgIXOGs9"), "token");
            content.Add(new StringContent("senderId"), "senderID");
            content.Add(new StringContent(recipients), "recipients");
            content.Add(new StringContent(message), "message");
            content.Add(new StringContent("1"), "gateway");
            request.Content = content;
            var xresponse = await client.SendAsync(request);
            xresponse.EnsureSuccessStatusCode();
            string responseBody = await xresponse.Content.ReadAsStringAsync();
            SmsResponse response = JsonConvert.DeserializeObject<SmsResponse>(responseBody);
            return response;
        }

         
    }
}
