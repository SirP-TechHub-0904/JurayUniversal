using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class PortfolioContactUs
    {
         public long Id { get; set; }
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        public string? Subject { get;set; }
        public string? Message { get;set; }
        public string? Email { get;set; }
        public string? Phone { get;set; }
        public DateTime Date { get;set;}
    }
}
