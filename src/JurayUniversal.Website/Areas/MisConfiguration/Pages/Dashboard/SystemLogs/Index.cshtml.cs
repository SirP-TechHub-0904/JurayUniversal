using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JurayUniversal.Website.Areas.MisConfiguration.Pages.Dashboard.SystemLogs
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public IndexModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        public List<FileInfo> LogFiles { get; set; } = new List<FileInfo>();

        public void OnGet()
        {
            string tenantHost = Request.Host.Value.Replace(":", "_");
            string logFolder = Path.Combine(_env.WebRootPath, "logs", tenantHost);

            if (Directory.Exists(logFolder))
            {
                var dirInfo = new DirectoryInfo(logFolder);
                LogFiles = dirInfo.GetFiles("*.txt")
                                  .OrderByDescending(f => f.LastWriteTime)
                                  .ToList();
            }
        }

        public IActionResult OnPostDownload(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return RedirectToPage();

            string tenantHost = Request.Host.Value.Replace(":", "_");
            string filePath = Path.Combine(_env.WebRootPath, "logs", tenantHost, fileName);

            // Prevent directory traversal attacks
            if (fileName.Contains("/") || fileName.Contains("\\") || fileName.Contains(".."))
            {
                return RedirectToPage();
            }

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            fileStream.CopyTo(memoryStream);
                            return File(memoryStream.ToArray(), "text/plain", fileName);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    TempData["error"] = "Error reading log file: " + ex.Message;
                    return RedirectToPage();
                }
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return RedirectToPage();

            // Prevent directory traversal attacks
            if (fileName.Contains("/") || fileName.Contains("\\") || fileName.Contains(".."))
            {
                return RedirectToPage();
            }

            string tenantHost = Request.Host.Value.Replace(":", "_");
            string filePath = Path.Combine(_env.WebRootPath, "logs", tenantHost, fileName);

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                    TempData["success"] = "Log file deleted successfully.";
                }
                catch (System.Exception ex)
                {
                    TempData["error"] = "Cannot delete the active log file: " + ex.Message;
                }
            }

            return RedirectToPage();
        }
    }
}
