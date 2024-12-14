using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users
{
     [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager,Admin")]

    public class CourseModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<CourseModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public CourseModel(SignInManager<Profile> signInManager,
            ILogger<CourseModel> logger,
            UserManager<Profile> userManager,
            Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }
        public Profile UserDatas { get; set; }

        [BindProperty]
        public long CourseId { get; set; }

        [BindProperty]
        public string UserId { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDatas = await _userManager.FindByIdAsync(id);

            if (UserDatas == null)
            {
                return NotFound();
            }
            var courseList = await _context.Courses.Include(x=>x.CourseCategory).ToListAsync();
            var oulist = courseList.Select(x=> new DropDownListCourse
            {
                Id = x.Id,
                Title = x.CourseCategory.Name +" (" +x.CourseCategory.Abbreviation +" " +x.SecNumber + " - "+x.Year.ToString("yyyy")+")",
            }).ToList();

            ViewData["CourseId"] = new SelectList(oulist, "Id", "Title");

            return Page();
        }

        public JsonResult OnGetCourseInformation(int courseId)
        {
            // Fetch course information based on courseId (replace this with your actual logic)
            var courseInformation = _context.Courses.Include(x => x.CourseCategory).FirstOrDefault(x => x.Id == courseId);

            string outcome = $"<dl class=\"row\">" +
                $"<dt class=\"col-sm-2\">Course</dt>\r\n " +
                $"<dd class=\"col-sm-10\">{courseInformation.CourseCategory.Name} ({courseInformation.CourseCategory.Abbreviation} {courseInformation.SecNumber} - {courseInformation.Year.ToString("yyyy")})</dd>\r\n  " +
                $"<dt class=\"col-sm-2\">\r\nTheme</dt>" +
                $"<dd class=\"col-sm-10\">   {courseInformation.Theme}</dd>" + 
                $"</dl>";
            return new JsonResult(outcome);
        }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // Get the last participant for the specific CourseId
            var lastParticipant = await _context.ParticipantLists
                .Where(p => p.CourseId == CourseId)
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            int newnumber = 0;

            if (lastParticipant == null)
            {
                newnumber = 1;

            }
            else
            {
                newnumber = lastParticipant.SerialNumber + 1;

            }
            var getcourse = await _context.Courses.Include(x => x.CourseCategory).FirstOrDefaultAsync(x => x.Id == CourseId);



            ParticipantList newParticipant = new ParticipantList();
            newParticipant.IdNumber = $"NIPSS/{getcourse.CourseCategory.Abbreviation}/{getcourse.SecNumber}/{getcourse.Year.ToString("yy")}/{newnumber.ToString("000")}";
            newParticipant.SerialNumber = newnumber;
            newParticipant.CourseId = CourseId;
            newParticipant.UserId = UserId; 
            newParticipant.ParticipantStatus = Domain.Enum.Enum.ParticipantStatus.Active;
            _context.ParticipantLists.Add(newParticipant);
            await _context.SaveChangesAsync();

            

            var getparticipant = await _context.ParticipantLists.FirstOrDefaultAsync(x=>x.Id == newParticipant.Id);

            //// Increment the last registration number
            //int lastRegNumber = int.Parse(lastParticipant.IdNumber.Split('/').Last());
            //string newRegNumber = $"{lastParticipant.IdNumber.Split('/')[0]}/{lastParticipant.IdNumber.Split('/')[1]}/{lastParticipant.IdNumber.Split('/')[2]}/{lastParticipant.IdNumber.Split('/')[3]}/{(lastRegNumber + 1):000}";
            //newParticipant.IdNumber = newRegNumber;
            


           
            //_context.Attach(getparticipant).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            //send email


            //send sms
            TempData["success"] = "Successful";
            return RedirectToPage("./RegistrationStatus", new {id = newParticipant.Id});
        }

    }

    public class DropDownListCourse
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
    }
}
