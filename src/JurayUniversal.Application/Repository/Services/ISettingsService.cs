using JurayUniversal.Application.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Repository.Services
{
    public interface ISettingsService
    {
        Task<bool> GetMaxUserCount();

        Task<VerificationWebDto> ValidateWeb(HttpContext httpContext);
        Task<VerificationWebDto> ProfileData(HttpContext httpContext);
        Task<VerificationWebDto> ContainPortfolioValidateWeb(HttpContext httpContext);

        Task<bool> CreateAccount(PortfolioAccountSetupDto data, HttpContext httpContext);
        Task<PortfolioDto> GetPortfolioAccount(string accesstoken, HttpContext httpContext);
        Task<PortfolioNameAndImageDto> GetPortfolioNameAndPassport(HttpContext httpContext);
        Task<bool> AddDemo(HttpContext httpContext);

        Task<bool> UseHttpsWWW();
        Task<bool> UseHttps();

    }
}
