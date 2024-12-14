using JurayUniversal.Application.Dtos.Project;
using JurayUniversal.Application.Repository.Base;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Repository.GeneralRepository.ProjectsCategory
{

    public class ProjectCategoryRepositoryAsync : GenericRepositoryAsync<ProjectCategory>, IProjectCategoryRepositoryAsync
    {
        private readonly DashboardDbContext _context;
        private readonly DbSet<ProjectCategory> _projectCatgeory;
        public ProjectCategoryRepositoryAsync(DashboardDbContext dbContext, DashboardDbContext context) : base(dbContext)
        {
            _projectCatgeory = dbContext.Set<ProjectCategory>();
            _context = context;
        }

        public async Task<IReadOnlyList<ProjectCategoryListDto>> GetAllProjectCategoryWithProjectCountAsync()
        {
            var categories = await _context.ProjectCategories.Include(x => x.ProjectMains).OrderBy(x => x.Title).ToListAsync(); // Replace dbContext with your actual database context
            var dtos = categories.Select(ConvertToDto).ToList();
            return dtos.AsReadOnly();
        }

        private ProjectCategoryListDto ConvertToDto(ProjectCategory category)
        {
            return new ProjectCategoryListDto
            {
                Id = category.Id,
                Title = category.Title,
                Description = category.Description,
                Projects = category.ProjectMains.Count.ToString()
            };
        }
    }
}
