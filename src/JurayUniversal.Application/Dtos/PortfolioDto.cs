using JurayUniversal.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Dtos
{
    public class PortfolioDto
    {
        public string ProfileId { get;set; }
        public Profile Profile { get;set; }

        public List<AdditionalProfile> AdditionalProfiles { get; set; }
        public List<ProfileCategory> ProfileCategories { get; set; }
        public List<ProfileCategoryList> ProfileCategoryLists { get; set; }
        public List<ProfilePortfolio> ProfilePortfolios { get; set; }
        public ProfilePortfolioSetting ProfilePortfolioSetting { get; set; }
        public PortfolioContactUs PortfolioContactUses { get; set; }
        public SuperSetting SuperSetting { get;set; }
        public Setting Setting { get;set; }
        public bool result { get;set;}
        public bool FirstTime { get;set;}
        public bool IsAuthenticated { get;set;}
    }
}
