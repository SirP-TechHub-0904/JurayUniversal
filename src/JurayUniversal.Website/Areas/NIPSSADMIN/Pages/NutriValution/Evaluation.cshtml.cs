using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Application.Dtos.UsersDto;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.NutriValution
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Evaluator")]

    public class EvaluationModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public EvaluationModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IList<UserDto> UserDto { get;set; }

        public async Task OnGetAsync()
        {

            //            UserDto = await _context.UserNutritionEvaluations
            //    .Include(u => u.NutritionCategory)
            //    .Include(u => u.NutritionCategoryList)
            //    .Include(u => u.User)
            //   .Select(u => new UserDto
            //   {
            //       Id = u.User.Id,
            //       FullName = u.User.FullnameX,
            //       Email = u.User.Email,
            //       Phone = u.User.PhoneNumber,
            //    })
            //.DistinctBy(u => u.Email)
            //.ToListAsync();

            var usersWithoutDuplicates = await _context.UserNutritionEvaluations
    .Include(u => u.NutritionCategory)
    .Include(u => u.NutritionCategoryList)
    .Include(u => u.User)
    .Select(u => new UserDto
    {
        Id = u.User.Id,
        FullName = u.User.FullnameX,
        Email = u.User.Email,
        Phone = u.User.PhoneNumber,
    })
    .ToListAsync();

            // Remove duplicates based on email
            UserDto = usersWithoutDuplicates
                .GroupBy(u => u.Email)
                .Select(g => g.First())
                .ToList();
        }
    }
}
