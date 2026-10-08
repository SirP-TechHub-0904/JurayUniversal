using System;
using System.Collections.Generic;
using System.Linq;

namespace JurayUniversal.MisCentral.Services
{
    /// <summary>
    /// Provides a registry of prohibited and reserved workspace subdomains.
    /// Prevents tenants from registering system/administrative reserved keywords.
    /// </summary>
    public static class ReservedWorkspaceNames
    {
        private static readonly HashSet<string> ReservedSlugs = new HashSet<string>(new[]
        {
            // Administration & Roles
            "admin", "administrator", "superadmin", "supadmin", "root", "sysadmin", "system",
            "manager", "moderator", "security", "host", "master", "central", "portal",
            "console", "dashboard", "miscentral",

            // Protocols & Infrastructure
            "api", "apis", "gateway", "graphql", "rest", "webhook", "webhooks",
            "cdn", "dns", "ns", "ns1", "ns2", "mail", "email", "smtp", "imap", "pop",
            "ftp", "ssh", "ssl", "cert", "certs", "status", "health", "monitor", "ping", "metrics",

            // Auth & Account
            "auth", "oauth", "sso", "login", "signin", "signup", "logout", "register",
            "onboarding", "account", "accounts", "billing", "pay", "payment", "payments",
            "checkout", "cart", "internal", "dev", "development", "staging", "test",
            "prod", "production", "sandbox",

            // Corporate & Core
            "juray", "jurayuniversal", "universal", "cloud",
            "support", "help", "helpdesk", "docs", "documentation", "legal", "terms",
            "privacy", "compliance", "about", "contact", "marketplace", "store", "shop",

            // Loopbacks & System tokens
            "localhost", "127", "local", "null", "undefined", "default", "public",
            "private", "static", "assets", "media", "www", "web", "app"
        }, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Checks whether the specified workspace slug is reserved by the system.
        /// </summary>
        public static bool IsReserved(string? slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return true;
            }

            var clean = slug.Trim().ToLowerInvariant();
            return ReservedSlugs.Contains(clean);
        }
    }
}
