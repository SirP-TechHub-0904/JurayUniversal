using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Dtos.Project;
using JurayUniversal.Application.Repository.Base;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Application.Repository.GeneralRepository.Projects
{
    public interface IProjectRepositoryAsync : IGenericRepositoryAsync<ProjectMain>
    {
        Task<IReadOnlyList<ProjectListDto>> GetAllProjectByQuery(long id = 0, string? keyword = null, long RefId = 0, DateTime? startdate = null, DateTime? finishdate = null, DateTime? updatedate = null, ProjectStatus status = 0);
         Task<ResponseDto> AddProjectAsync(ProjectMain entity, IFormFile? file);
        Task<ResponseDto> UpdateProjectAsync(ProjectEditDto entity, IFormFile? file);
           Task<ProjectDto> GetProjectIdAsync(long id);

        //Project Features
        Task<ProjectFeature> GetProjectFeatureByIdAsync(long id);
        Task<IReadOnlyList<ProjectFeature>> GetAllProjectFeatureAsync(long projectId);
        Task<IReadOnlyList<ProjectFeatureListDto>> GetAllProjectFeatureSpecificListAsync(long projectId);
        Task<ResponseDto> AddProjectFeatureAsync(ProjectFeature entity);
        Task<ResponseDto> UpdateProjectFeatureAsync(ProjectFeature entity);
        Task<ResponseDto> DeleteProjectFeatureAsync(ProjectFeature entity);

        //Project Team
        Task<ProjectTeam> GetProjectTeamByIdAsync(long id);
        Task<IReadOnlyList<ProjectTeam>> GetAllProjectTeamAsync(long projectId);
        Task<ResponseDto> AddProjectTeamAsync(ProjectTeam entity);
        Task<ResponseDto> UpdateProjectTeamAsync(ProjectTeam entity);
        Task<ResponseDto> DeleteProjectTeamAsync(ProjectTeam entity);

        //Project User
        Task<ProjectUser> GetProjectUserByIdAsync(long id);
        Task<IReadOnlyList<ProjectUser>> GetAllProjectUserAsync(long projectId);
        Task<ResponseDto> AddProjectUserAsync(ProjectUser entity);
        Task<ResponseDto> UpdateProjectUserAsync(ProjectUser entity);
        Task<ResponseDto> DeleteProjectUserAsync(ProjectUser entity);


         //Project Position
        Task<ProjectPosition> GetProjectPositionByIdAsync(long id);
        Task<IReadOnlyList<ProjectPosition>> GetAllProjectPositionAsync();
        Task<ResponseDto> AddProjectPositionAsync(ProjectPosition entity);
        Task<ResponseDto> UpdateProjectPositionAsync(ProjectPosition entity);
        Task<ResponseDto> DeleteProjectPositionAsync(ProjectPosition entity);

        //General Feature
        Task<GeneralFeature> GetGeneralFeatureByIdAsync(long id);
        Task<IReadOnlyList<GeneralFeature>> GetAllGeneralFeatureAsync();
        Task<ResponseDto> AddGeneralFeatureAsync(GeneralFeature entity);
        Task<ResponseDto> UpdateGeneralFeatureAsync(GeneralFeature entity);
        Task<ResponseDto> DeleteGeneralFeatureAsync(GeneralFeature entity);

         //Project Task
        Task<ProjectTask> GetProjectTaskByIdAsync(long id);
        Task<IReadOnlyList<ProjectTask>> GetAllProjectTaskAsync(long featureId);
        Task<ResponseDto> AddProjectTaskAsync(ProjectTask entity);
        Task<ResponseDto> UpdateProjectTaskAsync(ProjectTask entity);
        Task<ResponseDto> DeleteProjectTaskAsync(ProjectTask entity);


        //Project File
        Task<ProjectFileDto> GetProjectFileByIdAsync(long id);
        Task<IReadOnlyList<ProjectFile>> GetAllProjectFileAsync(long projectId);
        Task<ResponseDto> AddProjectFileAsync(ProjectFile entity, IFormFile? file);
        Task<ResponseDto> UpdateProjectFileAsync(ProjectFile entity, IFormFile? file);


        Task<IReadOnlyList<ProjectReportDto>> GetAllProjectReportsAsync(long projectId);
        Task<IReadOnlyList<ProjectReportDto>> GetAllProjectChallengesAsync(long projectId);

    }
}
