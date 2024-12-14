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
using JurayUniversal.Application.Repository.GeneralRepository.ProjectsCategory;

namespace JurayUniversal.Website.Areas.MIS.Pages.IProject
{
    public class PcategoryListModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IRazorRenderService _renderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectCategoryRepositoryAsync _projectCategory;
        private readonly ILogger<PcategoryListModel> _logger;
        public PcategoryListModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IRazorRenderService renderService, IUnitOfWork unitOfWork, IProjectCategoryRepositoryAsync projectCategory, ILogger<PcategoryListModel> logger)
        {
            _context = context;
            _renderService = renderService;
            _unitOfWork = unitOfWork;
            _projectCategory = projectCategory;
            _logger = logger;
        }

        public IEnumerable<ProjectCategory> ProjectCategory { get; set; }

        public async Task OnGetAsync()
        {
            //ProjectCategory = await _context.ProjectCategories.Include(x=>x.ProjectMains).OrderBy(x=>x.Title).ToListAsync();
        }

        public async Task<PartialViewResult> OnGetViewAllPartial()
        {
            var data = await _projectCategory.GetAllProjectCategoryWithProjectCountAsync();

            return new PartialViewResult
            {
                ViewName = "_ViewAll",
                ViewData = new ViewDataDictionary<IEnumerable<ProjectCategoryListDto>>(ViewData, data)
            };
        }
        public async Task<JsonResult> OnGetCreateOrEditAsync(long id = 0)
        {
            if (id == 0)
                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CreateOrEdit", new ProjectCategoryDto()) });
            else
            {
                var thisdata = await _projectCategory.GetByIdAsync(id);
                var getData = new ProjectCategoryDto
                {
                    Id = thisdata.Id,
                    Title = thisdata.Title,
                    Description = thisdata.Description,

                };
                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CreateOrEdit", getData) });
            }
        }

        public async Task<JsonResult> OnGetDetailsAsync(long id = 0)
        {
            if (id == 0)
            {
                var datalist = await _projectCategory.GetAllProjectCategoryWithProjectCountAsync();
                var html = await _renderService.ToStringAsync("_ViewAll", datalist);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                var thisdata = await _projectCategory.GetByIdAsync(id);
                var getData = new ProjectCategoryDto
                {
                    Id = thisdata.Id,
                    Title = thisdata.Title,
                    Description = thisdata.Description,

                };
                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Details", getData) });
            }
        }

        public async Task<JsonResult> OnPostCreateOrEditAsync(long id, ProjectCategoryDto data)
        {

            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    var create = new ProjectCategory
                    {
                        Title = data.Title,
                        Description = data.Description,

                    };
                    await _projectCategory.AddAsync(create);
                    await _unitOfWork.Commit();
                }
                else
                {
                    var update = new ProjectCategory
                    {
                        Id = data.Id,
                        Title = data.Title,
                        Description = data.Description,

                    };
                    await _projectCategory.UpdateAsync(update);
                    await _unitOfWork.Commit();
                }
                var ProjectCategoryList = await _projectCategory.GetAllProjectCategoryWithProjectCountAsync();
                var html = await _renderService.ToStringAsync("_ViewAll", ProjectCategoryList);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                var html = await _renderService.ToStringAsync("_CreateOrEdit", data);
                return new JsonResult(new { isValid = false, html = html });
            }
        }
        public async Task<JsonResult> OnPostDeleteAsync(long id)
        {
            var data = await _projectCategory.GetByIdAsync(id);
            await _projectCategory.DeleteAsync(data);
            await _unitOfWork.Commit();
            var datalist = await _projectCategory.GetAllProjectCategoryWithProjectCountAsync();
            var html = await _renderService.ToStringAsync("_ViewAll", datalist);
            return new JsonResult(new { isValid = true, html = html });
        }
    }
}
