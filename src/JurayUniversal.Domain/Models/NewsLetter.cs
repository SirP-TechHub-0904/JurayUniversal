using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class NewsLetter
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public bool Active { get; set; }
    }
}
