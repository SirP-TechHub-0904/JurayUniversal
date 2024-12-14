using JurayUniversal.Application.Dtos.Project;
using JurayUniversal.Application.Repository.Base;
using JurayUniversal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayUniversal.Application.Repository.GeneralRepository.ProjectsCategory
{
    public interface IProjectCategoryRepositoryAsync : IGenericRepositoryAsync<ProjectCategory>
    {
        Task<IReadOnlyList<ProjectCategoryListDto>> GetAllProjectCategoryWithProjectCountAsync();
    }
}
