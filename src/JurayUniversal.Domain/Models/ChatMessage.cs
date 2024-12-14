using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class ChatMessage
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public Profile User { get; set; }
        public string Message { get; set; }

        public string? ReceiverId { get; set; }
        public Profile Receiver { get; set; }

        public DateTime Date { get; set; }
        public bool All { get; set; }
        public bool Deleted { get; set; }
    }
}
