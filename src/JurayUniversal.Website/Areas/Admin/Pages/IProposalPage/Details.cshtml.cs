using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using JurayUniversal.Domain.Models;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Application.Dtos.AwsDtos;

namespace JurayUniversal.Website.Areas.Admin.Pages.IProposalPage
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager")]

    public class DetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;

        public DetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _config = config;
            _storageService = storageService;
        }
        [BindProperty]
        public ProposalFile ProposalFile { get; set; }
        public Proposal Proposal { get; set; }
        public List<ProposalFile> ProposalFiles { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Proposal = await _context.Proposals
                .Include(p => p.SubmittedBy).FirstOrDefaultAsync(m => m.Id == id);

            if (Proposal == null)
            {
                return NotFound();
            }

            ProposalFiles = await _context.ProposalFiles.Where(x => x.ProposalId == Proposal.Id).ToListAsync();
            return Page();
        }
        [BindProperty]
        public IFormFile? imagefile { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {

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
                        ProposalFile.Url = xresult.Url;
                        ProposalFile.Key = xresult.Key;
                    }
                    else
                    {
                        TempData["error"] = "unable to upload";
                        //return Page();
                    }
                }
                catch (Exception c)
                {

                }
            }

             
            _context.ProposalFiles.Add(ProposalFile);
            await _context.SaveChangesAsync(); TempData["success"] = "Successful";

            return RedirectToPage("./Details", new { id = ProposalFile.ProposalId });
        }

        
        public async Task<IActionResult> OnPostDocumentDelete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProposalFile = await _context.ProposalFiles.FindAsync(id);

            if (ProposalFile != null)
            {
                try
                {
                    var cred = new AwsCredentials()
                    {
                        AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                        SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                    };

                    var vidresult = await _storageService.DeleteObjectAsync(cred, await _storageService.BucketName(), ProposalFile.Key);
                 }
                catch (Exception c) { }

                _context.ProposalFiles.Remove(ProposalFile);
                await _context.SaveChangesAsync(); TempData["success"] = "Successful";
            }

            return RedirectToPage("./Details", new { id = ProposalFile.ProposalId });
        }

    }
}
