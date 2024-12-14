using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class XProjectMessage
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime RepliedDate { get; set; }

        public string SenderId { get; set; }
        public Profile Sender {  get; set; }

        public string? ReceiverId { get; set; }
        public Profile Receiver { get; set; }

        public long XProjectId {  get; set; }
        public XProject XProject { get; set; }
    }
}
