using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Amazon.Runtime.Internal;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace JurayUniversal.Website.Areas.Admin.Pages.IFacilityPage
{
    public class CreateModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public CreateModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Facility Facility { get; set; }
        [BindProperty]
        public IFormFile ExcelFile { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            List<Facility> emailLists = new List<Facility>();

            // Set the license context before using EPPlus
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var stream = ExcelFile.OpenReadStream())
            {
                using (var package = new ExcelPackage(stream))
                {
                    // Assuming the first sheet contains the data
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (worksheet != null)
                    {
                        int rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++) // Start from 2 to skip header row
                        {
                            var emailList = new Facility
                            {
                                State = worksheet.Cells[row, 1].Value?.ToString(),
                                LGA = worksheet.Cells[row, 2].Value?.ToString(),
                                Ward = worksheet.Cells[row, 3].Value?.ToString(),
                                FacilityLocation = worksheet.Cells[row, 4].Value?.ToString(),
                               
                                // Assign other properties accordingly
                            };
                            emailLists.Add(emailList);
                        }
                    }
                }
            }
            int gb = 0;
            foreach (var email in emailLists)
            {
                // Check if the email already exists in the database
                //bool isDuplicate = await _context.Facilities.AnyAsync(e => e.State == email.State && e.LGA == email.LGA && e.Ward == email.Ward && e.FacilityLocation == email.FacilityLocation);

                //// If the email is not a duplicate, add it to the database
                //if (!isDuplicate)
                //{
                    await _context.Facilities.AddAsync(email);
                gb++;
              //  }
            }
            await _context.SaveChangesAsync();


            return RedirectToPage("./Index");
        }
    }
}
