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

        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; }
        public bool IsReadOnly { get; set; }
        public bool EnableListDownload { get; set; }

        public string CategoryAlias { get; set; } = "Category";
        public Dictionary<string, List<JurayUniversal.Domain.Models.MisUserFeatureRole>> UserRoles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
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

            var query = _context.Users.OfType<Profile>()
                .Include(u => u.UserCategory)
                .Where(u => u.UserName != "superadmin" && u.UserName != "msuperadmin" && u.Email != "superadmin@juray.ng")
                .AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(u => (u.FirstName != null && u.FirstName.Contains(SearchString)) || 
                                         (u.LastName != null && u.LastName.Contains(SearchString)) || 
                                         (u.Email != null && u.Email.Contains(SearchString)));
            }

            if (CategoryId.HasValue)
            {
                query = query.Where(u => u.UserCategoryId == CategoryId.Value);
            }

            if (Status.HasValue)
            {
                query = query.Where(u => u.UserStatus == Status.Value);
            }

            int pageSize = 1000; // Increase significantly to let DataTables handle it
            PageIndex = pageIndex ?? 1;

            var count = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (PageIndex < 1) PageIndex = 1;
            if (TotalPages > 0 && PageIndex > TotalPages) PageIndex = TotalPages;

            Users = await query
                .OrderBy(u => u.LastName)
                .Take(pageSize)
                .ToListAsync();

            var userIds = Users.Select(u => u.Id).ToList();
            var rolesList = await _context.MisUserFeatureRoles.Where(r => userIds.Contains(r.ProfileId)).ToListAsync();
            foreach (var u in Users)
            {
                UserRoles[u.Id] = rolesList.Where(r => r.ProfileId == u.Id).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAllIdsAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Forbid();

            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            string searchString = Request.Form["SearchString"].FirstOrDefault();
            string categoryIdStr = Request.Form["CategoryId"].FirstOrDefault();
            string statusStr = Request.Form["Status"].FirstOrDefault();

            long? categoryId = !string.IsNullOrEmpty(categoryIdStr) ? Convert.ToInt64(categoryIdStr) : null;
            UserStatus? status = !string.IsNullOrEmpty(statusStr) && Enum.TryParse<UserStatus>(statusStr, out var parsedStatus) ? parsedStatus : null;

            var superAdminRoleIds = await _context.Roles.Where(r => r.Name == "mSuperAdmin" || r.Name == "SuperAdmin").Select(r => r.Id).ToListAsync();
            var superAdminUserIds = await _context.UserRoles.Where(ur => superAdminRoleIds.Contains(ur.RoleId)).Select(ur => ur.UserId).ToListAsync();

            var query = _context.Users.OfType<Profile>()
                .Where(u => !superAdminUserIds.Contains(u.Id))
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower();
                query = query.Where(m =>
                    (m.FirstName != null && m.FirstName.ToLower().Contains(searchValue)) ||
                    (m.LastName != null && m.LastName.ToLower().Contains(searchValue)) ||
                    (m.Email != null && m.Email.ToLower().Contains(searchValue)) ||
                    (m.PhoneNumber != null && m.PhoneNumber.ToLower().Contains(searchValue)));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(u => 
                    (u.FirstName != null && u.FirstName.ToLower().Contains(searchString)) || 
                    (u.LastName != null && u.LastName.ToLower().Contains(searchString)) || 
                    (u.Email != null && u.Email.ToLower().Contains(searchString)));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(u => u.UserCategoryId == categoryId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(u => u.UserStatus == status.Value);
            }

            var ids = await query.Select(u => u.Id).ToListAsync();
            return new JsonResult(ids);
        }

        public async Task<IActionResult> OnPostDataAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Forbid();
            }

            var dtParams = JurayUniversal.Website.Helpers.DataTablesHelper.GetParameters(Request);
            var searchValue = dtParams.SearchValue;
            var sortColumnDirection = dtParams.SortColumnDirection;
            var sortColumnIndex = dtParams.SortColumnIndex;
            int pageSize = dtParams.Length;
            int skip = dtParams.Start;

            // Custom filters passed via DataTables ajax data
            string searchString = Request.Form["SearchString"].FirstOrDefault();
            string categoryIdStr = Request.Form["CategoryId"].FirstOrDefault();
            string statusStr = Request.Form["Status"].FirstOrDefault();

            long? categoryId = !string.IsNullOrEmpty(categoryIdStr) ? Convert.ToInt64(categoryIdStr) : null;
            UserStatus? status = !string.IsNullOrEmpty(statusStr) && Enum.TryParse<UserStatus>(statusStr, out var parsedStatus) ? parsedStatus : null;

            var superAdminRoleIds = await _context.Roles.Where(r => r.Name == "mSuperAdmin" || r.Name == "SuperAdmin").Select(r => r.Id).ToListAsync();
            var superAdminUserIds = await _context.UserRoles.Where(ur => superAdminRoleIds.Contains(ur.RoleId)).Select(ur => ur.UserId).ToListAsync();

            var query = _context.Users.OfType<Profile>()
                .Include(u => u.UserCategory)
                .Where(u => !superAdminUserIds.Contains(u.Id))
                .AsQueryable();

            // Apply global DataTables search
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower();
                query = query.Where(m =>
                    (m.FirstName != null && m.FirstName.ToLower().Contains(searchValue)) ||
                    (m.LastName != null && m.LastName.ToLower().Contains(searchValue)) ||
                    (m.Email != null && m.Email.ToLower().Contains(searchValue)) ||
                    (m.PhoneNumber != null && m.PhoneNumber.ToLower().Contains(searchValue)));
            }

            // Apply custom Top-Bar filters
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                query = query.Where(u => 
                    (u.FirstName != null && u.FirstName.ToLower().Contains(searchString)) || 
                    (u.LastName != null && u.LastName.ToLower().Contains(searchString)) || 
                    (u.Email != null && u.Email.ToLower().Contains(searchString)));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(u => u.UserCategoryId == categoryId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(u => u.UserStatus == status.Value);
            }

            int recordsTotal = await query.CountAsync();

            if (!string.IsNullOrEmpty(sortColumnIndex))
            {
                int index = Convert.ToInt32(sortColumnIndex);
                bool isAsc = sortColumnDirection == "asc";
                switch (index)
                {
                    case 1: // s/n
                        query = isAsc ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date);
                        break;
                    case 2: // name
                        query = isAsc ? query.OrderBy(x => x.FirstName).ThenBy(x => x.LastName) : query.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName);
                        break;
                    case 3: // contact (email)
                        query = isAsc ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email);
                        break;
                    case 4: // category
                        query = isAsc ? query.OrderBy(x => x.UserCategory.Title) : query.OrderByDescending(x => x.UserCategory.Title);
                        break;
                    case 5: // status
                        query = isAsc ? query.OrderBy(x => x.UserStatus) : query.OrderByDescending(x => x.UserStatus);
                        break;
                    case 6: // dob
                        query = isAsc ? query.OrderBy(x => x.DateOfBirth) : query.OrderByDescending(x => x.DateOfBirth);
                        break;
                    case 7: // date registered
                        query = isAsc ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date);
                        break;
                    case 8: // gender
                        query = isAsc ? query.OrderBy(x => x.Gender) : query.OrderByDescending(x => x.Gender);
                        break;
                    case 9: // nationality
                        query = isAsc ? query.OrderBy(x => x.Nationality) : query.OrderByDescending(x => x.Nationality);
                        break;
                    case 10: // position
                        query = isAsc ? query.OrderBy(x => x.PositionOrRank) : query.OrderByDescending(x => x.PositionOrRank);
                        break;
                    default:
                        query = query.OrderBy(x => x.LastName);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.LastName);
            }

            List<Profile> users;
            if (pageSize == -1)
            {
                users = await query.Skip(skip).ToListAsync();
            }
            else
            {
                users = await query.Skip(skip).Take(pageSize).ToListAsync();
            }

            var userIds = users.Select(u => u.Id).ToList();
            var rolesList = await _context.MisUserFeatureRoles.Where(r => userIds.Contains(r.ProfileId)).ToListAsync();
            
            var userRolesMap = new Dictionary<string, List<JurayUniversal.Domain.Models.MisUserFeatureRole>>();
            foreach (var u in users)
            {
                userRolesMap[u.Id] = rolesList.Where(r => r.ProfileId == u.Id).ToList();
            }

            var roles = await _userManager.GetRolesAsync(currentUser);
            bool isReadOnly = !roles.Contains("mSuperAdmin") && !roles.Contains("Admin") && !roles.Contains("UserManagement_GlobalEditor") && !roles.Contains("UserManagement_FeatureEditor");

            var data = new List<object>();
            int sn = skip;

            foreach (var user in users)
            {
                sn++;
                
                string checkboxHtml = $"<input type=\"checkbox\" class=\"form-check-input shadow-none row-checkbox\" value=\"{user.Id}\">";
                
                string rolesHtml = "";
                if (userRolesMap.ContainsKey(user.Id) && userRolesMap[user.Id].Any())
                {
                    rolesHtml = "<span class=\"badge bg-light text-primary border border-primary border-opacity-25 mt-1 shadow-sm\" style=\"font-size: 10px; padding: 4px 6px;\">Administration Privilege</span>";
                }
                else
                {
                    rolesHtml = "<span class=\"badge bg-light text-muted border border-secondary border-opacity-25 mt-1\" style=\"font-size: 10px; padding: 4px 6px;\">User</span>";
                }

                string nameHtml = $@"
                    <a href=""/MisUserManagement/Dashboard/Roles?id={user.Id}"" class=""text-decoration-none"">
                        <div class=""d-flex flex-column"">
                            <span class=""fw-bold text-dark"" style=""font-size: 14px;"">{user.FirstName} {user.LastName}</span>
                            <div class=""mt-1 d-flex flex-wrap gap-1"">{rolesHtml}</div>
                        </div>
                    </a>";

                string contactHtml = $@"
                    <div class=""d-flex flex-column"">
                        <span class=""text-dark"">{user.Email}</span>
                        <span class=""text-muted"" style=""font-size: 12px;"">{user.PhoneNumber ?? "000"}</span>
                    </div>";

                string categoryHtml = user.UserCategory?.Title ?? "None";

                string statusColor = user.UserStatus == UserStatus.Active ? "success" : 
                                     user.UserStatus == UserStatus.Pending ? "warning" : "danger";
                
                string statusHtml = $"<span class=\"badge bg-{statusColor}\" style=\"font-size: 10px; padding: 5px 8px; border-radius: 4px;\">{user.UserStatus}</span>";

                string dobHtml = user.DateOfBirth?.ToString("MMM dd, yyyy") ?? "N/A";
                string dateRegHtml = user.Date.ToString("MMM dd, yyyy");
                string genderHtml = user.Gender.ToString();
                string nationalityHtml = user.Nationality ?? "N/A";
                string positionHtml = string.IsNullOrEmpty(user.PositionOrRank) ? "N/A" : user.PositionOrRank;

                string manageHtml = "";
                if (!isReadOnly)
                {
                    manageHtml = $@"
                        <div class=""dropdown"">
                            <button class=""btn btn-sm btn-light dropdown-toggle shadow-none text-center"" type=""button"" data-bs-toggle=""dropdown"" style=""font-size: 12px; border-radius: 4px;"">
                                Manage
                            </button>
                            <ul class=""dropdown-menu dropdown-menu-end shadow-sm"" style=""font-size: 13px;"">
                                <li>
                                    <a class=""dropdown-item py-2"" href=""/MisUserManagement/Dashboard/Detail?id={user.Id}"">
                                        <i class=""bi bi-file-earmark-person me-2 text-primary""></i> View Detail
                                    </a>
                                </li>
                                <li>
                                    <a class=""dropdown-item py-2"" href=""/MisUserManagement/Dashboard/Edit?id={user.Id}"">
                                        <i class=""bi bi-pencil-square me-2 text-warning""></i> Edit Profile
                                    </a>
                                </li>
                                <li>
                                    <a class=""dropdown-item py-2"" href=""/MisUserManagement/Dashboard/Roles?id={user.Id}"">
                                        <i class=""bi bi-shield-check me-2 text-info""></i> Feature Roles
                                    </a>
                                </li>
                                <li>
                                    <a class=""dropdown-item py-2"" href=""/MisUserManagement/Dashboard/KpiAssign?id={user.Id}"">
                                        <i class=""bi bi-clipboard-check-fill me-2 text-success""></i> Evaluate KPIs
                                    </a>
                                </li>
                            </ul>
                        </div>";
                }
                else
                {
                    manageHtml = "<span class=\"text-muted\"><i class=\"bi bi-lock\"></i></span>";
                }

                data.Add(new {
                    checkbox = checkboxHtml,
                    sn = $"<span class=\"text-muted\">{sn}</span>",
                    name = nameHtml,
                    contact = contactHtml,
                    category = categoryHtml,
                    status = statusHtml,
                    dob = dobHtml,
                    dateRegistered = dateRegHtml,
                    gender = genderHtml,
                    nationality = nationalityHtml,
                    position = positionHtml,
                    manage = manageHtml
                });
            }

            return JurayUniversal.Website.Helpers.DataTablesHelper.CreateResponse(dtParams.Draw, recordsTotal, recordsTotal, data);
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
