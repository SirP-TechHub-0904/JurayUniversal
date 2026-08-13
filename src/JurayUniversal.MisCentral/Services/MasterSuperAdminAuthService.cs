using System;
using System.Text.Encodings.Web;

namespace JurayUniversal.MisCentral.Services
{
    public interface IMasterSuperAdminAuthService
    {
        string GetTargetConfigUrl(string domainName);
        string GetTargetMisUrl(string domainName);
    }

    public class MasterSuperAdminAuthService : IMasterSuperAdminAuthService
    {
        public string GetTargetConfigUrl(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                return "/";

            string formattedDomain = FormatDomain(domainName);
            return $"{formattedDomain}/MisConfiguration/Dashboard/Index";
        }

        public string GetTargetMisUrl(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
                return "/";

            string formattedDomain = FormatDomain(domainName);
            return $"{formattedDomain}/MIS/Dashboard/Index";
        }

        private string FormatDomain(string domain)
        {
            domain = domain.Trim().ToLower();
            if (domain.StartsWith("http://") || domain.StartsWith("https://"))
                return domain.TrimEnd('/');

            if (domain.Contains("localhost") || domain.Contains("127.0.0.1") || domain.Contains("147.93.128.160"))
                return $"http://{domain}";

            return $"https://{domain}";
        }
    }
}
