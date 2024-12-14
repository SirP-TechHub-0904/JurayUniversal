using SmsPlatform.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsPlatform.Repositories
{
    public interface ISmsRepository
    {
        Task<SmsResponse> SendSmsViaKudi(string senderId, string message, string recipients);
        Task<bool> HasBlackListedWords(string input, string blackLists);
        Task<List<string>> RemoveDuplicates(string input);
        Task<List<string>> FormatNumbers(List<string> numbers);
        Task<decimal> UnitsPerPage(List<string> numbers);
        Task<int> CountPage(string input);
    }
}
