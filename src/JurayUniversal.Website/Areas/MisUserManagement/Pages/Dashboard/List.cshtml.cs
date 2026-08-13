using JurayUniversal.Application.Repository.MIS.UserManagement;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using OxmlWordprocessing = DocumentFormat.OpenXml.Wordprocessing;
using iText.Kernel.Pdf;
using iText.Layout;
using iTextParagraph = iText.Layout.Element.Paragraph;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace JurayUniversal.Website.Areas.MisUserManagement.Pages.Dashboard
{
    [Authorize]
    [RequiresNewMISDashboard]
    public class ListModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public ListModel(DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Profile> Users { get; set; } = new();
        public List<UserCategory> Categories { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public long? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserStatus? Status { get; set; }

        public bool IsReadOnly { get; set; }
        public bool EnableListDownload { get; set; }

        public string CategoryAlias { get; set; } = "Category";
        public Dictionary<string, List<JurayUniversal.Domain.Models.MisUserFeatureRole>> UserRoles { get; set; } = new();

        private async Task<Profile?> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null) return user;

            var userId = _userManager.GetUserId(User);
            if (!string.IsNullOrEmpty(userId))
            {
                user = await _context.Users.OfType<Profile>().FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null) return user;
            }

            if (!string.IsNullOrEmpty(User.Identity?.Name))
            {
                user = await _context.Users.OfType<Profile>().FirstOrDefaultAsync(u => u.UserName == User.Identity.Name || u.Email == User.Identity.Name);
            }

            return user;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser == null && !(User.Identity?.IsAuthenticated ?? false))
            {
                return Forbid();
            }

            var roles = await _userManager.GetRolesAsync(currentUser);
            bool hasAccess = roles.Contains("mSuperAdmin") || roles.Contains("Admin") || roles.Contains("UserManagement_GlobalEditor") || roles.Contains("UserManagement_FeatureEditor") || roles.Contains("UserManagement_Readonly");
            
            if (!hasAccess)
            {
                return RedirectToPage("/Platform/Index", new { area = "MisUserManagement" });
            }

            IsReadOnly = !roles.Contains("mSuperAdmin") && !roles.Contains("Admin") && !roles.Contains("UserManagement_GlobalEditor") && !roles.Contains("UserManagement_FeatureEditor");

            var misSetting = await _context.MisSettings.FirstOrDefaultAsync();
            if (misSetting != null && !string.IsNullOrEmpty(misSetting.CategoryAlias))
            {
                CategoryAlias = misSetting.CategoryAlias;
            }
            EnableListDownload = misSetting?.EnableListDownload ?? false;

            Categories = await _context.UserCategories.ToListAsync();

            Users = await _context.Users.OfType<Profile>()
                .Include(u => u.UserCategory)
                .Where(u => u.UserName != "superadmin" && u.UserName != "msuperadmin" && u.Email != "superadmin@juray.ng")
                .OrderBy(u => u.LastName)
                .ToListAsync();

            var userIds = Users.Select(u => u.Id).ToList();
            var rolesList = await _context.MisUserFeatureRoles.Where(r => userIds.Contains(r.ProfileId)).ToListAsync();
            foreach (var u in Users)
            {
                UserRoles[u.Id] = rolesList.Where(r => r.ProfileId == u.Id).ToList();
            }

            return Page();
        }



        public async Task<IActionResult> OnPostBulkActionAsync(string action, List<string> ids)
        {
            if (ids == null || !ids.Any())
            {
                return new JsonResult(new { success = false, message = "No users selected" });
            }

            var usersToUpdate = await _context.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
            
            foreach (var user in usersToUpdate)
            {
                if (action == "Suspend")
                {
                    user.UserStatus = UserStatus.Suspended;
                }
                else if (action == "Activate")
                {
                    user.UserStatus = UserStatus.Active;
                }
                
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnGetExportAsync(
            string format, string title, string columns,
            string? SearchString, long? CategoryId, int? Status)
        {
            // Verify export is enabled
            var misSetting = await _context.MisSettings.FirstOrDefaultAsync();
            if (misSetting == null || !misSetting.EnableListDownload)
                return Forbid();

            // Re-run the exact same query as OnPostDataAsync (no pagination)
            var query = _context.Users
                .Include(u => u.UserCategory)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchString))
                query = query.Where(u =>
                    u.FirstName.Contains(SearchString) ||
                    u.LastName.Contains(SearchString) ||
                    u.Email.Contains(SearchString) ||
                    u.PhoneNumber.Contains(SearchString));

            if (CategoryId.HasValue && CategoryId > 0)
                query = query.Where(u => u.UserCategoryId == CategoryId);

            if (Status.HasValue)
            {
                var status = (UserStatus)Status.Value;
                query = query.Where(u => u.UserStatus == status);
            }

            var users = await query.OrderBy(x => x.LastName).ToListAsync();

            // Parse visible column names from the request
            var visibleCols = (columns ?? "sn,name,contact,category,status")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim().ToLower())
                .Where(c => c != "manage" && c != "checkbox")
                .ToList();

            // Build a column map: key => header label
            var colHeaders = new Dictionary<string, string>
            {
                ["sn"] = "S/N",
                ["name"] = "Name",
                ["contact"] = "Contact",
                ["category"] = "Category",
                ["status"] = "Status",
                ["dob"] = "Date of Birth",
                ["datereg"] = "Date Registered",
                ["gender"] = "Gender",
                ["nationality"] = "Nationality",
                ["position"] = "Position / Rank"
            };

            // Build row data (plain text, no HTML)
            Func<Profile, int, Dictionary<string, string>> buildRow = (user, index) =>
                new Dictionary<string, string>
                {
                    ["sn"] = index.ToString(),
                    ["name"] = $"{user.FirstName} {user.LastName}",
                    ["contact"] = $"{user.Email}\n{user.PhoneNumber}",
                    ["category"] = user.UserCategory?.Title ?? "-",
                    ["status"] = user.UserStatus.ToString(),
                    ["dob"] = user.DateOfBirth?.ToString("dd MMM yyyy") ?? "-",
                    ["datereg"] = user.Date.ToString("dd MMM yyyy"),
                    ["gender"] = user.Gender.ToString(),
                    ["nationality"] = user.Nationality ?? "-",
                    ["position"] = user.PositionOrRank ?? "-"
                };

            var safeTitle = string.IsNullOrWhiteSpace(title) ? "User List" : title.Trim();

            if (format == "excel")
            {
                using var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add(safeTitle.Length > 31 ? safeTitle.Substring(0, 31) : safeTitle);

                // Title row
                ws.Cell(1, 1).Value = safeTitle;
                ws.Range(1, 1, 1, visibleCols.Count).Merge();
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 1).Style.Font.FontSize = 16;
                ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Header row
                for (int c = 0; c < visibleCols.Count; c++)
                {
                    var cell = ws.Cell(2, c + 1);
                    cell.Value = colHeaders.ContainsKey(visibleCols[c]) ? colHeaders[visibleCols[c]] : visibleCols[c];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#3b82f6");
                    cell.Style.Font.FontColor = XLColor.White;
                }

                // Data rows
                for (int i = 0; i < users.Count; i++)
                {
                    var row = buildRow(users[i], i + 1);
                    for (int c = 0; c < visibleCols.Count; c++)
                    {
                        var val = row.ContainsKey(visibleCols[c]) ? row[visibleCols[c]] : "-";
                        ws.Cell(i + 3, c + 1).Value = val;
                    }
                }

                ws.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{safeTitle}.xlsx");
            }
            else if (format == "pdf")
            {
                using var stream = new MemoryStream();
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var doc = new iText.Layout.Document(pdf, iText.Kernel.Geom.PageSize.A4.Rotate());
                doc.SetMargins(30, 30, 30, 30);

                // Title
                doc.Add(new iTextParagraph(safeTitle)
                    .SetFontSize(18)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(12));

                // Table
                var table = new iText.Layout.Element.Table(visibleCols.Count)
                    .UseAllAvailableWidth();

                // Header cells
                foreach (var col in visibleCols)
                {
                    var headerText = colHeaders.ContainsKey(col) ? colHeaders[col] : col;
                    table.AddHeaderCell(new Cell()
                        .Add(new iTextParagraph(headerText).SetBold().SetFontSize(10))
                        .SetBackgroundColor(new iText.Kernel.Colors.DeviceRgb(59, 130, 246))
                        .SetFontColor(iText.Kernel.Colors.ColorConstants.WHITE));
                }

                // Data rows
                for (int i = 0; i < users.Count; i++)
                {
                    var row = buildRow(users[i], i + 1);
                    foreach (var col in visibleCols)
                    {
                        var val = row.ContainsKey(col) ? row[col] : "-";
                        table.AddCell(new Cell().Add(new iTextParagraph(val).SetFontSize(9)));
                    }
                }

                doc.Add(table);
                doc.Close();

                return File(stream.ToArray(), "application/pdf", $"{safeTitle}.pdf");
            }
            else if (format == "word")
            {
                using var stream = new MemoryStream();
                using var wordDoc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true);
                var mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new OxmlWordprocessing.Document();
                var body = mainPart.Document.AppendChild(new OxmlWordprocessing.Body());

                // Title paragraph
                var titlePara = body.AppendChild(new OxmlWordprocessing.Paragraph());
                var titleRun = titlePara.AppendChild(new OxmlWordprocessing.Run());
                titleRun.AppendChild(new OxmlWordprocessing.RunProperties(new OxmlWordprocessing.Bold(), new OxmlWordprocessing.FontSize { Val = "36" }));
                titleRun.AppendChild(new OxmlWordprocessing.Text(safeTitle));

                body.AppendChild(new OxmlWordprocessing.Paragraph()); // spacer

                // Table
                var tbl = body.AppendChild(new OxmlWordprocessing.Table());
                var tblProps = new OxmlWordprocessing.TableProperties(
                    new OxmlWordprocessing.TableBorders(
                        new OxmlWordprocessing.TopBorder { Val = OxmlWordprocessing.BorderValues.Single, Size = 4 },
                        new OxmlWordprocessing.BottomBorder { Val = OxmlWordprocessing.BorderValues.Single, Size = 4 },
                        new OxmlWordprocessing.LeftBorder { Val = OxmlWordprocessing.BorderValues.Single, Size = 4 },
                        new OxmlWordprocessing.RightBorder { Val = OxmlWordprocessing.BorderValues.Single, Size = 4 },
                        new OxmlWordprocessing.InsideHorizontalBorder { Val = OxmlWordprocessing.BorderValues.Single, Size = 4 },
                        new OxmlWordprocessing.InsideVerticalBorder { Val = OxmlWordprocessing.BorderValues.Single, Size = 4 }));
                tbl.AppendChild(tblProps);

                // Header row
                var headerRow = tbl.AppendChild(new OxmlWordprocessing.TableRow());
                foreach (var col in visibleCols)
                {
                    var headerText = colHeaders.ContainsKey(col) ? colHeaders[col] : col;
                    var tc = headerRow.AppendChild(new OxmlWordprocessing.TableCell());
                    var p = tc.AppendChild(new OxmlWordprocessing.Paragraph());
                    var r = p.AppendChild(new OxmlWordprocessing.Run());
                    r.AppendChild(new OxmlWordprocessing.RunProperties(new OxmlWordprocessing.Bold()));
                    r.AppendChild(new OxmlWordprocessing.Text(headerText));
                }

                // Data rows
                for (int i = 0; i < users.Count; i++)
                {
                    var row = buildRow(users[i], i + 1);
                    var dataRow = tbl.AppendChild(new OxmlWordprocessing.TableRow());
                    foreach (var col in visibleCols)
                    {
                        var val = row.ContainsKey(col) ? row[col] : "-";
                        var tc = dataRow.AppendChild(new OxmlWordprocessing.TableCell());
                        var p = tc.AppendChild(new OxmlWordprocessing.Paragraph());
                        var r = p.AppendChild(new OxmlWordprocessing.Run());
                        r.AppendChild(new OxmlWordprocessing.Text(val));
                    }
                }

                mainPart.Document.Save();
                wordDoc.Dispose();

                return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    $"{safeTitle}.docx");
            }

            return BadRequest("Unsupported export format.");
        }
    }
}
