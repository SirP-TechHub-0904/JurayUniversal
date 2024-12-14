using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class ProfilePortfolio
    {
         public long Id { get; set; }
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageKey { get; set; }
    }
}
