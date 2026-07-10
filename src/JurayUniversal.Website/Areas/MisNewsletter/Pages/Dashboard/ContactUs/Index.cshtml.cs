using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using JurayUniversal.Domain.Models;

namespace JurayUniversal.Website.Areas.MisNewsletter.Pages.Dashboard.ContactUs
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,SubAdmin")]
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public bool IsNewsletterEnabled { get; set; }
        public bool EnableListDownload { get; set; } = true;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            bool isNewsletterFeatureActive = await _context.MisFeatures
                .AnyAsync(x => x.ModuleFolder == "MisNewsletter" && x.Status != JurayUniversal.Domain.Enum.Enum.FeatureBillingStatus.Disabled);
            
            IsNewsletterEnabled = isNewsletterFeatureActive;
            ViewData["IsNewsletterEnabled"] = isNewsletterFeatureActive;
        }

        public async Task<IActionResult> OnPostDataAsync()
        {
            var dtParams = JurayUniversal.Website.Helpers.DataTablesHelper.GetParameters(Request);
            var searchValue = dtParams.SearchValue;
            var sortColumnDirection = dtParams.SortColumnDirection;
            var sortColumnIndex = dtParams.SortColumnIndex;
            int pageSize = dtParams.Length;
            int skip = dtParams.Start;

            string searchString = Request.Form["SearchString"].FirstOrDefault();

            var query = _context.ClientRequest.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(r => r.Name.Contains(searchValue) || r.Email.Contains(searchValue) || r.Phone.Contains(searchValue) || r.Subject.Contains(searchValue));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(r => r.Name.Contains(searchString) || r.Email.Contains(searchString) || r.Phone.Contains(searchString) || r.Subject.Contains(searchString));
            }

            int recordsTotal = await query.CountAsync();

            if (!string.IsNullOrEmpty(sortColumnIndex))
            {
                int index = Convert.ToInt32(sortColumnIndex);
                bool isAsc = sortColumnDirection == "asc";
                switch (index)
                {
                    case 2: query = isAsc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name); break;
                    case 3: query = isAsc ? query.OrderBy(x => x.Phone) : query.OrderByDescending(x => x.Phone); break;
                    case 4: query = isAsc ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email); break;
                    case 5: query = isAsc ? query.OrderBy(x => x.Subject) : query.OrderByDescending(x => x.Subject); break;
                    default: query = query.OrderByDescending(x => x.Id); break;
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.Id);
            }

            var requests = await query.Skip(skip).Take(pageSize).ToListAsync();

            // Check exported statuses
            var userId = _userManager.GetUserId(User);
            var nlList = await _context.MisNewsletterLists.FirstOrDefaultAsync(l => l.Name == "Website Contact Us" && l.UserId == userId);
            var pbList = await _context.MisPhoneBooks.FirstOrDefaultAsync(l => l.Name == "Website Contact Us" && l.UserId == userId);

            List<string> exportedEmails = new List<string>();
            List<string> exportedPhones = new List<string>();

            if (nlList != null) exportedEmails = await _context.MisNewsletterContacts.Where(c => c.MisNewsletterListId == nlList.Id).Select(c => c.Email).ToListAsync();
            if (pbList != null) exportedPhones = await _context.MisContacts.Where(c => c.MisPhoneBookId == pbList.Id).Select(c => c.PhoneNumber).ToListAsync();

            var data = new List<object>();
            int sn = skip + 1;

            foreach (var req in requests)
            {
                bool emailExported = !string.IsNullOrEmpty(req.Email) && exportedEmails.Contains(req.Email);
                bool phoneExported = !string.IsNullOrEmpty(req.Phone) && exportedPhones.Contains(req.Phone);

                string checkboxHtml = $"<input type='checkbox' class='row-checkbox form-check-input' value='{req.Id}' />";
                
                string emailHtml = emailExported ? $"{req.Email} <span class='badge bg-success ms-1'>Exported</span>" : req.Email;
                string phoneHtml = phoneExported ? $"{req.Phone} <span class='badge bg-primary ms-1'>Exported</span>" : req.Phone;
                
                string manageHtml = $@"<div class='btn-group'>
                    <button type='button' class='btn btn-success dropdown-toggle dropdown-icon' data-bs-toggle='dropdown' aria-expanded='false'>Action</button>
                    <div class='dropdown-menu'>
                        <a class='dropdown-item' href='/MisNewsletter/Dashboard/ContactUs/Details?id={req.Id}'>Details</a>
                        <a class='dropdown-item' href='/MisNewsletter/Dashboard/ContactUs/Delete?id={req.Id}'>Delete</a>
                    </div>
                </div>";

                data.Add(new
                {
                    DT_RowId = req.Id.ToString(),
                    checkbox = checkboxHtml,
                    sn = sn++,
                    name = req.Name,
                    phone = phoneHtml,
                    email = emailHtml,
                    subject = req.Subject,
                    manage = manageHtml
                });
            }

            return JurayUniversal.Website.Helpers.DataTablesHelper.CreateResponse(dtParams.Draw, recordsTotal, recordsTotal, data);
        }

        public async Task<IActionResult> OnPostAllIdsAsync()
        {
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            string searchString = Request.Form["SearchString"].FirstOrDefault();

            var query = _context.ClientRequest.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(r => r.Name.Contains(searchValue) || r.Email.Contains(searchValue) || r.Phone.Contains(searchValue) || r.Subject.Contains(searchValue));
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(r => r.Name.Contains(searchString) || r.Email.Contains(searchString) || r.Phone.Contains(searchString) || r.Subject.Contains(searchString));
            }

            var allItems = await query.ToListAsync();
            var validIds = allItems.Select(r => r.Id.ToString()).ToList();

            return new JsonResult(validIds);
        }

        public async Task<IActionResult> OnPostBulkActionAsync(string action, List<long> ids)
        {
            if (ids == null || !ids.Any())
            {
                return new JsonResult(new { success = false, message = "No items selected" });
            }

            var userId = _userManager.GetUserId(User);
            var selectedRequests = await _context.ClientRequest.Where(r => ids.Contains(r.Id)).ToListAsync();
            int exportedCount = 0;

            if (action == "Newsletter")
            {
                var nlList = await _context.MisNewsletterLists.FirstOrDefaultAsync(l => l.Name == "Website Contact Us" && l.UserId == userId);
                if (nlList == null)
                {
                    nlList = new MisNewsletterList { Name = "Website Contact Us", UserId = userId, CreatedAt = DateTime.UtcNow };
                    _context.MisNewsletterLists.Add(nlList);
                    await _context.SaveChangesAsync();
                }

                foreach (var req in selectedRequests)
                {
                    if (!string.IsNullOrEmpty(req.Email))
                    {
                        bool exists = await _context.MisNewsletterContacts.AnyAsync(c => c.MisNewsletterListId == nlList.Id && c.Email == req.Email);
                        if (!exists)
                        {
                            _context.MisNewsletterContacts.Add(new MisNewsletterContact
                            {
                                MisNewsletterListId = nlList.Id,
                                Email = req.Email,
                                Name = req.Name,
                                DateAdded = DateTime.UtcNow
                            });
                            exportedCount++;
                        }
                    }
                }
            }
            else if (action == "SMS")
            {
                var pbList = await _context.MisPhoneBooks.FirstOrDefaultAsync(l => l.Name == "Website Contact Us" && l.UserId == userId);
                if (pbList == null)
                {
                    pbList = new MisPhoneBook { Name = "Website Contact Us", UserId = userId, CreatedAt = DateTime.UtcNow };
                    _context.MisPhoneBooks.Add(pbList);
                    await _context.SaveChangesAsync();
                }

                foreach (var req in selectedRequests)
                {
                    if (!string.IsNullOrEmpty(req.Phone))
                    {
                        bool exists = await _context.MisContacts.AnyAsync(c => c.MisPhoneBookId == pbList.Id && c.PhoneNumber == req.Phone);
                        if (!exists)
                        {
                            _context.MisContacts.Add(new MisContact
                            {
                                MisPhoneBookId = pbList.Id,
                                PhoneNumber = req.Phone,
                                Name = req.Name,
                                IsActive = true
                            });
                            exportedCount++;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true, message = $"Successfully exported {exportedCount} contacts." });
        }
    }
}
