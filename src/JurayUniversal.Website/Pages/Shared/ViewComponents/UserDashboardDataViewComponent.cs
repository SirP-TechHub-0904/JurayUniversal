using Amazon.S3;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Pages.Shared.ViewComponents
{
    public class UserDashboardDataViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        public UserDashboardDataViewComponent(
            DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Find the user by their Id
            var currentUser = HttpContext.User;
            if (currentUser.Identity.IsAuthenticated)
            {
                var user = _userManager.FindByNameAsync(currentUser.Identity.Name).Result;

                if (user != null)
                {
                    ViewBag.fullname1 = user.UserName;
                    var userRoles = _userManager.GetRolesAsync(user).Result;
                   if (userRoles.Contains("mSuperAdmin"))
                    {
                        ViewBag.userRole = "m Admin";
                    }
                    else if (userRoles.Contains("Participant"))
                    {
                        ViewBag.userRole = "Participant";
                    }
                    else if (userRoles.Contains("Staff"))
                    {
                        ViewBag.userRole = "Staff";
                    }
                    else if (userRoles.Contains("Directing"))
                    {
                        ViewBag.userRole = "Directing";
                    }
                    else if (userRoles.Contains("Management"))
                    {
                        ViewBag.userRole = "Management";
                    }
                    else if (userRoles.Contains("Manager"))
                    {
                        ViewBag.userRole = "Manager";
                    }
                    else if (userRoles.Contains("Admin"))
                    {
                        ViewBag.userRole = "Admin";
                    }
                    else if (userRoles.Contains("Editor"))
                    {
                        ViewBag.userRole = "Editor";
                    }
                    
                    else
                    {
                        ViewBag.userRole = "User";
                    }
                }

                // Get a list of roles for the specified user

                var dataitem = await _context.DashboardSettings.FirstOrDefaultAsync();
                if(dataitem != null) { 
                ViewBag.dataitem = dataitem.UserCustomDashboard;

                }
            }
            var dashmenu = await _context.CompanyProgramCategories.Where(x=>x.ShowInMenu == true).ToListAsync();
            ViewBag.Cmp = dashmenu;
            return View();

        }
    }
}