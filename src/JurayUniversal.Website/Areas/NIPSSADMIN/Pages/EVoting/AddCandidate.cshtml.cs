using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Areas.NIPSSADMIN.Pages.ManageCourse;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Application.Dtos.AwsDtos;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.EVoting
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class AddCandidateModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;
        public AddCandidateModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _config = config;
            _storageService = storageService;
        }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var catevote = await _context.VoteCategories
                .Include(x=>x.Evoting)
                .FirstOrDefaultAsync(x=>x.Id == id);
            // Fetch data and map it to DTO
            var listsDS = _context.ParticipantLists
                .Include(x => x.User)

                .Where(x => x.CourseId == catevote.Evoting.CourseId && x.ParticipantStatus == Domain.Enum.Enum.ParticipantStatus.Active)
                .OrderBy(x => x.User.LastName)
                .Select(x => new StaffManagerDto
                {
                    UserId = x.User.Id,
                    Fullname = x.User.FullnameX
                })
                .ToList();
            // Assign data to ViewData
            ViewData["CandidateId"] = new SelectList(listsDS, "UserId", "Fullname");

            VotingId = catevote.EvotingId;
            VotingCategoryId = catevote.Id;

            Title = catevote.Title +" FOR "+ catevote.Evoting.Title;
            return Page();
        }

        [BindProperty]
        public VoteCondidate VoteCondidate { get; set; }
        [BindProperty]
        public IFormFile? imagefile { get; set; }

        [BindProperty]
        public long? VotingId { get; set; }

        public long? VotingCategoryId { get; set; }
        public string Title { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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

                    var s3Obj = new Application.Dtos.AwsDtos.S3Object()
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
                        VoteCondidate.CandidateImageUrl = xresult.Url;
                        VoteCondidate.CandidateImageKey = xresult.Key;
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

            VoteCondidate.Date = DateTime.UtcNow.AddHours(1);
            _context.VoteCondidates.Add(VoteCondidate);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new {id = VotingId });
        }
    }
}
