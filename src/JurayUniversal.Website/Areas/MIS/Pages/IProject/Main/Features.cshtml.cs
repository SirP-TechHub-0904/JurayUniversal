using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using JurayUniversal.Website.Middlewares;
using JurayUniversal.Application.Repository.Base;
using JurayUniversal.Application.Dtos.Project;
using JurayUniversal.Application.Repository.GeneralRepository.Projects;
using Microsoft.CodeAnalysis;
using static JurayUniversal.Domain.Enum.Enum;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Microsoft.AspNetCore.Identity;
using JurayUniversal.Application.Repository.GeneralRepository.Users;
using JurayUniversal.Application.Repository.GeneralRepository.ProjectsCategory;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JurayUniversal.Website.Areas.MIS.Pages.Main
{
    public class FeaturesModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IRazorRenderService _renderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepositoryAsync _project;
        private readonly IUserRepositoryAsync _userRepositoryAsync;
        private readonly UserManager<Profile> _userManager;
        private readonly IProjectCategoryRepositoryAsync _projectCategory;
        private readonly ILogger<FeaturesModel> _logger;

        public FeaturesModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IRazorRenderService renderService, IUnitOfWork unitOfWork, IProjectRepositoryAsync project, UserManager<Profile> userManager, IUserRepositoryAsync userRepositoryAsync, IProjectCategoryRepositoryAsync projectCategory, ILogger<FeaturesModel> logger)
        {
            _context = context;
            _renderService = renderService;
            _unitOfWork = unitOfWork;
            _project = project;
            _userManager = userManager;
            _userRepositoryAsync = userRepositoryAsync;
            _projectCategory = projectCategory;
            _logger = logger;
        }

        public IEnumerable<ProjectMain> Project { get; set; }
        [BindProperty]
        public ProjectFeature Data { get; set; }

        [BindProperty]
        public long Id { get; set; }
        public string ProjectTitle { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            var setting = await _context.DashboardSettings.FirstOrDefaultAsync();
            if (setting == null)
            {
                return NotFound();
            }
            if (setting.ActivateProject == false)
            {
                return NotFound();
            }
            var project = await _project.GetByIdAsync(id);
            if (project != null)
            {
                Id = id;
                ProjectTitle = project.Title;
                ViewData["GeneralFeatureId"] = new SelectList(_context.GeneralFeatures, "Id", "Title");
                return Page();
            }
            else
            {
                TempData["error"] = "Project Not Found";
                return RedirectToAction("/Index");
            }


        }

        public async Task<IActionResult> OnPostAsync()
        {

            

            try
            {
               await _project.AddProjectFeatureAsync(Data);
                TempData["success"] = "Successful";
            }
            catch (Exception f)
            {

            }

            return RedirectToPage("./Details", new { id = Data.ProjectMainId });
        }






        public async Task<PartialViewResult> OnGetViewAllPartial()
        {
            var data = await _project.GetAllProjectByQuery();

            return new PartialViewResult
            {
                ViewName = "_ViewAll",
                ViewData = new ViewDataDictionary<IEnumerable<ProjectListDto>>(ViewData, data)
            };
        }
        public async Task<JsonResult> OnGetAddGeneralFeatureAsync()
        {


            return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_AddGeneralFeature", new GeneralFeature()) });

        }
        public async Task<IActionResult> OnPostAddGeneralFeatureAsync(long id, GeneralFeature data)
        {
            if (ModelState.IsValid)
            {


                await _project.AddGeneralFeatureAsync(data);
                return new JsonResult(new { isValid = true, html = "<script>location.reload();</script>" });
                 
            }
            else
            {

                var html = await _renderService.ToStringAsync("_AddGeneralFeature", new GeneralFeature());
                return new JsonResult(new { isValid = false, html = html });
            }
        }

        //edit
        public async Task<JsonResult> OnGetEditAsync(long id = 0)
        {
            if (id == 0)
            {
                var datalist = await _project.GetAllProjectByQuery();
                var html = await _renderService.ToStringAsync("_ViewAll", datalist);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                var thisdata = await _project.GetByIdAsync(id);


                var referralOptions = (await _userRepositoryAsync.GetDashboardAllUsersAsync())
        .Select(u => new System.Web.Mvc.SelectListItem
        {
            Value = u.Id.ToString(),
            Text = u.Username
        })
        .ToList();

                var categoryOptions = (await _projectCategory.GetAllAsync())
                    .Select(c => new System.Web.Mvc.SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Title
                    })
                    .ToList();
                // Create a new instance of ProjectCreateDto and set the dropdown options
                var model = new ProjectEditDto
                {
                    Id = thisdata.Id,
                    Title = thisdata.Title,
                    Description = thisdata.Description,
                    Materials = thisdata.Materials,
                    Note = thisdata.Note,
                    Authentications = thisdata.Authentications,
                    ProfileReferralId = thisdata.ProfileReferralId,
                    ProjectCategoryId = thisdata.ProjectCategoryId,
                    LogoPicture = thisdata.LogoUrl,
                    Urls = thisdata.Urls,
                    LastUpdateDate = thisdata.LastDateUpdated,
                    ProjectStatus = thisdata.ProjectStatus,

                    ReferralOptions = referralOptions,
                    CategoryOptions = categoryOptions
                };

                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Edit", model) });
            }
        }
        public async Task<JsonResult> OnPostEditAsync(long id, ProjectEditDto thisdata, IFormFile? file)
        {
            ModelState.Remove("ReferralOptions");
            ModelState.Remove("CategoryOptions");
            ModelState.Remove("LogoPicture");
            if (ModelState.IsValid)
            {


                await _project.UpdateProjectAsync(thisdata, file);



                var datalist = await _project.GetAllProjectByQuery();
                var html = await _renderService.ToStringAsync("_ViewAll", datalist);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                var html = await _renderService.ToStringAsync("_Edit", thisdata);
                return new JsonResult(new { isValid = false, html = html });
            }
        }

        public async Task<JsonResult> OnGetDetailsAsync(long id = 0)
        {
            if (id == 0)
            {
                var datalist = await _project.GetAllProjectByQuery();
                var html = await _renderService.ToStringAsync("_ViewAll", datalist);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                var thisdata = await _project.GetByIdAsync(id);

                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Details", thisdata) });
            }
        }

        //public async Task<JsonResult> OnPostDeleteAsync(long id)
        //{
        //    var data = await _project.GetByIdAsync(id);
        //    await _project.DeleteAsync(data);
        //    await _unitOfWork.Commit();
        //    var datalist = await _project.GetAllProjectByQuery();
        //    var html = await _renderService.ToStringAsync("_ViewAll", datalist);
        //    return new JsonResult(new { isValid = true, html = html });
        //}
    }
}
