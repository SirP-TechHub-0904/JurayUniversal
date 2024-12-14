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
using Microsoft.AspNetCore.Identity;
using JurayUniversal.Website.V2.Pages.AuthVadmin;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Application.Dtos.UsersDto;
using JurayUniversal.Application.Repository.GeneralRepository.Projects;
using JurayUniversal.Application.Repository.GeneralRepository.Users;
using System.Data;
using System.Net;

namespace JurayUniversal.Website.Areas.MIS.Pages.IProject
{
    public class IndexModel : PageModel
    {


        private readonly ILogger<IndexModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IRazorRenderService _renderService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;
        //
        //
        //

        private readonly IUserRepositoryAsync _userRepositoryAsync;
        private readonly RoleManager<IdentityRole> _role;
        private readonly UserManager<Profile> _userManager;
        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IRazorRenderService renderService, IUnitOfWork unitOfWork, ILogger<IndexModel> logger, UserManager<Profile> userManager, RoleManager<IdentityRole> role, IConfiguration config, IStorageService storageService, IUserRepositoryAsync userRepositoryAsync)
        {
            _context = context;
            _renderService = renderService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
            _role = role;
            _config = config;
            _storageService = storageService;
            _userRepositoryAsync = userRepositoryAsync;
        }

      public async Task<IActionResult> OnGetAsync()
        {
var setting = await _context.DashboardSettings.FirstOrDefaultAsync();
            if(setting == null)
            {
                return NotFound();
            }
            if(setting.ActivateProject == false)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<PartialViewResult> OnGetViewAllPartial()
        {
            var data = await _userRepositoryAsync.GetDashboardAllUsersAsync();

            return new PartialViewResult
            {
                ViewName = "_ViewAll",
                ViewData = new ViewDataDictionary<IEnumerable<ListUsersDto>>(ViewData, data)
            };
        }
        public async Task<JsonResult> OnGetCreateOrEditAsync(string? id)
        {

            if (id == null)
            {
                var roles = await _userRepositoryAsync.GetDashboardRolesAsync();
                var userDto = new UserDto { Roles = roles.ToList() };
                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CreateOrEdit", userDto) });
            }
            else
            {
                var thisdata = await _userRepositoryAsync.GetByIdAsync(id);

                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CreateOrEdit", thisdata) });
            }
        }

        public async Task<JsonResult> OnGetDetailsAsync(string? id)
        {
            if (id == null)
            {
                var datalist = await _userRepositoryAsync.GetDashboardAllUsersAsync();
                var html = await _renderService.ToStringAsync("_ViewAll", datalist);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                var thisdata = await _userRepositoryAsync.GetUserDetailsByIdAsync(id);

                return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_Details", thisdata) });
            }
        }

        public async Task<JsonResult> OnPostCreateOrEditAsync(string? id, UserDto data, IFormFile? file)
        {

            ModelState.Remove("Id");
            ModelState.Remove("Roles");
            ModelState.Remove("UserRoles");
            ModelState.Remove("Response");
            if (id != null)
            {
                ModelState.Remove("Email");
                ModelState.Remove("Username");
            }
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    var result = await _userRepositoryAsync.AddAsync(data, file);
                    if (!result.Result.Contains("successfull"))
                    {
                        var roles = await _userRepositoryAsync.GetDashboardRolesAsync();
                        data.Roles = roles.ToList();
                        data.Response = result.Result.ToString();
                        var htmlx = await _renderService.ToStringAsync("_CreateOrEdit", data);
                       // return new JsonResult(new { isValid = false, html = htmlx });

                                         return new JsonResult(new { isValid = true, html = await _renderService.ToStringAsync("_CreateOrEdit", data) });

                    }
                }
                else
                {
                    await _userRepositoryAsync.UpdateAsync(data, file);
                }
                var list = await _userRepositoryAsync.GetDashboardAllUsersAsync();
                var html = await _renderService.ToStringAsync("_ViewAll", list);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                var roles = await _userRepositoryAsync.GetDashboardRolesAsync();
                data.Roles = roles.ToList();
                var html = await _renderService.ToStringAsync("_CreateOrEdit", data);
                return new JsonResult(new { isValid = true, html = html });
            }
        }
        public async Task<JsonResult> OnPostDeleteAsync(string id)
        {
            var getuser = await _userManager.FindByIdAsync(id);
            if (getuser != null)
            {
                getuser.UserStatus = Domain.Enum.Enum.UserStatus.Deleted;
                await _userManager.UpdateAsync(getuser);
            }

            var datalist = await _userRepositoryAsync.GetDashboardAllUsersAsync();
            var html = await _renderService.ToStringAsync("_ViewAll", datalist);
            return new JsonResult(new { isValid = true, html = html });
        }
    }
}
