using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class ContactUs
    {
        public ContactUs()
        {
            Date = DateTime.UtcNow.AddHours(1);
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Report { get; set; }
        public DateTime Date { get; set; }
        public DateTime LastUpdated { get; set; }

    }
}
