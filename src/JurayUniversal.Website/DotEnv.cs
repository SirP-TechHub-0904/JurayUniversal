using System;
using System.IO;

namespace JurayUniversal.Website
{
    public static class DotEnv
    {
        public static void Load(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                var appDir = AppContext.BaseDirectory;
                var currentDir = Directory.GetCurrentDirectory();
                var potentialPaths = new[]
                {
                    Path.Combine(currentDir, ".env"),
                    Path.Combine(appDir, ".env"),
                    Path.Combine(Directory.GetParent(appDir)?.FullName ?? "", ".env")
                };

                foreach (var path in potentialPaths)
                {
                    if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    {
                        filePath = path;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return;

            foreach (var rawLine in File.ReadAllLines(filePath))
            {
                var trimmed = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                    continue;

                var separatorIndex = trimmed.IndexOf('=');
                if (separatorIndex <= 0)
                    continue;

                var key = trimmed.Substring(0, separatorIndex).Trim();
                var val = trimmed.Substring(separatorIndex + 1).Trim();

                if ((val.StartsWith("\"") && val.EndsWith("\"")) ||
                    (val.StartsWith("'") && val.EndsWith("'")))
                {
                    if (val.Length >= 2)
                        val = val.Substring(1, val.Length - 2);
                }

                Environment.SetEnvironmentVariable(key, val);
            }
        }
    }
}