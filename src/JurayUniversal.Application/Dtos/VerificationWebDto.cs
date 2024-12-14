using JurayUniversal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Dtos
{
    public class VerificationWebDto
    {
        public bool SettingFound { get;set;}
        public bool Portfolio { get;set;}
        public bool PortfolioAdmin { get;set;}
        public string Path { get;set; }
        public string PortfolioPath { get;set; }
        public string PortfolioAdminPath { get;set; }
        public string Area { get;set; }
        public SuperSetting SuperSetting { get;set; }
        public Setting Setting { get;set; }
        public ProfilePortfolioSetting ProfilePortfolioSetting { get;set; }
    }
}
