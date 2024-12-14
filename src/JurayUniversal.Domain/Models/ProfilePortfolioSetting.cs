using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Domain.Models
{
    public class ProfilePortfolioSetting
    {
         public long Id { get; set; }
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }
        public string? DefaultWebLink { get; set; }
        public string? UpgradeWebLink { get; set; }   
        public bool UseUpgradeWebLink { get; set; }
        public DateTime RegDate { get;set;}
        public int TimeFrame { get;set;}
        public bool EnableContactUs { get; set; }
        public bool DisableAccount { get; set; }
        public bool AccountUpgraded { get; set; }
        public string? Token { get; set; }
        public bool FirstTime { get;set;}

        public long? PortfolioTemplateId { get; set; }
        public PortfolioTemplate PortfolioTemplate { get;set;}
    }
}
