using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;
using System.ComponentModel.DataAnnotations;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Application.Dtos.AwsDtos;
using Microsoft.AspNetCore.Identity;

namespace JurayUniversal.Website.Areas.Admin.Pages.IExpenses.MainExpenses
{
    public class UpdateExpensesModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;
        private readonly UserManager<Profile> _userManager;

        public UpdateExpensesModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IConfiguration config, IStorageService storageService, UserManager<Profile> userManager)
        {
            _context = context;
            _config = config;
            _storageService = storageService;
            _userManager = userManager;
        }

        [BindProperty]
        public CompayExpenses CompayExpenses { get; set; }
        private async Task LoadAsync(CompayExpenses CompayExpenses)
        {

            Input = new InputModel
            {
                Note = CompayExpenses.Note,
                Amount = CompayExpenses.Amount,
                ApprovedByManager = CompayExpenses.ApprovedByManager,
                ManagerNote = CompayExpenses.ManagerNote,
                ApprovedByCEO = CompayExpenses.ApprovedByCEO,
                CeoNote = CompayExpenses.CeoNote,
                ApprovedByCFO = CompayExpenses.ApprovedByCFO,
                CfoNote = CompayExpenses.CfoNote,
                Date = CompayExpenses.Date,
                Close = CompayExpenses.Close
    };
        }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CompayExpenses = await _context.CompayExpenses
                .Include(c => c.ExpensesCategory)
                .Include(c => c.User).FirstOrDefaultAsync(m => m.Id == id);

            if (CompayExpenses == null)
            {
                return NotFound();
            }
           ViewData["ExpensesCategoryId"] = new SelectList(_context.ExpensesCategories, "Id", "Description");
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            await LoadAsync(CompayExpenses);
            return Page();
        }
        [BindProperty]
        public InputModel Input { get; set; }
         
        public class InputModel
        {
            public string Note { get; set; }
            public decimal Amount { get; set; }
              
            [Display(Name = "Approved By Manager")]
            public bool ApprovedByManager { get; set; }
            [Display(Name = "Manager Note")]
            public string ManagerNote { get; set; }
            [Display(Name = "Approved By CEO")]
            public bool ApprovedByCEO { get; set; }
            [Display(Name = "CEO Note")]
            public string CeoNote { get; set; }

            [Display(Name = "Approved By CFO")]
            public bool ApprovedByCFO { get; set; }
            [Display(Name = "CFO Note")]
            public string CfoNote { get; set; }
            public DateTime Date { get; set; }

            public bool Close { get; set; }
        }
        [BindProperty]
        public IFormFile? imagefile { get; set; }
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            var expense = await _context.CompayExpenses.FirstOrDefaultAsync(x => x.Id == CompayExpenses.Id);

            //image
            if (imagefile != null)
            {
                try
                {
                    // Process file
                    await using var memoryStream = new MemoryStream();
                    await imagefile.CopyToAsync(memoryStream);

                    var fileExt = Path.GetExtension(imagefile.FileName);
                    var docName = $"{Guid.NewGuid()}{fileExt}";
                    // call server

                    var s3Obj = new S3Object()
                    {
                        BucketName = await _storageService.BucketName(),
                        InputStream = memoryStream,
                        Name = docName
                    };

                    var cred = new AwsCredentials()
                    {
                        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                    };

                    var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, "");
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        expense.ProveImageUrl = xresult.Url;
                        expense.ProveImageKey = xresult.Key;
                    }
                    else
                    {
                        TempData["error"] = "unable to upload image";
                        //return Page();
                    }
                }
                catch (Exception c)
                {

                }
            }
            var user = await _userManager.GetUserAsync(User);
            var cfo = await _userManager.IsInRoleAsync(user, "CFO");
            var ceo = await _userManager.IsInRoleAsync(user, "CEO");
            var man = await _userManager.IsInRoleAsync(user, "Manager");

            if (man.Equals(true))
            {
                expense.Note = Input.Note;
                expense.Amount = Input.Amount;
                expense.ApprovedByManager = Input.ApprovedByManager;
                expense.ManagerNote = Input.ManagerNote;
                expense.Date = Input.Date;
                expense.Close = Input.Close;
            }
            if (cfo.Equals(true))
            {
                expense.ApprovedByCFO = Input.ApprovedByCFO;
                expense.CfoNote = Input.CfoNote;


            }
            if (ceo.Equals(true))
            {
                expense.ApprovedByCEO = Input.ApprovedByCEO;
                expense.CeoNote = Input.CeoNote;

            }


           
           
          
            _context.Attach(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompayExpensesExists(CompayExpenses.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new {id = expense.Id});
        }

        private bool CompayExpensesExists(long id)
        {
            return _context.CompayExpenses.Any(e => e.Id == id);
        }
    }
}
