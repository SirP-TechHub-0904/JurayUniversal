using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using JurayUniversal.Persistence.EF.SQL.Migrations;
using NuGet.Configuration;
using System.Web.Helpers;
using JurayUniversal.Application.Repository.NotifyRegister;
using JurayUniversal.Application.Services;

namespace JurayUniversal.Website.Areas.UserBudget.Pages.BD
{
    public class EditModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IEmailSendService _email;
         private readonly LoggerLibrary _logger;
        public EditModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager, IEmailSendService email, LoggerLibrary logger)
        {
            _context = context;
            _userManager = userManager;
            _email = email;
            _logger = logger;
        }

        [BindProperty]
        public BudgetMainCategory BudgetMainCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BudgetMainCategory = await _context.BudgetMainCategories
                .Include(b => b.User).FirstOrDefaultAsync(m => m.Id == id);

            if (BudgetMainCategory == null)
            {
                return NotFound();
            }
             return Page();
        }
        [BindProperty]
        public string? BudgetEmails { get; set; }
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            

            _context.Attach(BudgetMainCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BudgetMainCategoryExists(BudgetMainCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["success"] = "success";
            ///
            try
            {
                var settings = await _context.SuperSettings.FirstOrDefaultAsync();

                //cc emails
                string[] bemails = BudgetEmails.Split(',');

                // Add CC recipients
                foreach (var xmails in bemails)
                {
                  
                    //send to user
                    try
                    {
                        var mainuser = await _userManager.FindByEmailAsync(xmails);
                        if(mainuser != null) { 
                        string message = $"You have been assined to view a budget";
                        await _email.SendEmail(mainuser.FirstName, xmails.Trim(), null, null, $"BUDGET PREVIEW", message);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Log($" {settings.CompanyName} -  {ex.ToString()}");
                    }
                }

            }
            catch (Exception c)
            {
                 
            }
            
            return RedirectToPage("./Index");
        }

        private bool BudgetMainCategoryExists(long id)
        {
            return _context.BudgetMainCategories.Any(e => e.Id == id);
        }
    }
}
