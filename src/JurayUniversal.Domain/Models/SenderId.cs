using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Domain.Models
{
    public class SenderId
    {
        public long Id { get; set; }
        public SenderIdStatus SenderIdStatus { get; set; }
        public string Title { get; set; }
        public DateTime Date {  get; set; }
    }
}
