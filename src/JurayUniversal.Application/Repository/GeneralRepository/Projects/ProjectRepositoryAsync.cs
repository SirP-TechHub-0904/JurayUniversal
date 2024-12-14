using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Dtos.Project;
using JurayUniversal.Application.Repository.Base;
using JurayUniversal.Application.Services;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using System.Xml.Linq;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Application.Repository.GeneralRepository.Projects
{
    public class ProjectRepositoryAsync : GenericRepositoryAsync<ProjectMain>, IProjectRepositoryAsync
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DashboardDbContext _context;
        private readonly DbSet<ProjectMain> _projects;


        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStorageService _storageService;
        public ProjectRepositoryAsync(DashboardDbContext dbContext, DashboardDbContext context, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IStorageService storageService) : base(dbContext)
        {
            _projects = dbContext.Set<ProjectMain>();
            _context = context;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _storageService = storageService;
        }

        public async Task<ResponseDto> AddGeneralFeatureAsync(GeneralFeature entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                await _context.Set<GeneralFeature>().AddAsync(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> AddProjectAsync(ProjectMain entity, IFormFile? file)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                var fileresponse = await _storageService.MainUploadFileReturnUrlAsync(null, file);
                if (fileresponse.Message.Contains("200"))
                {
                    entity.LogoUrl = fileresponse.Url;
                    entity.LogoKey = fileresponse.Key;
                }
            }
            catch (Exception x)
            {

            }
            try
            {
                entity.Date = DateTime.UtcNow.AddHours(1);
                entity.ProjectStatus = ProjectStatus.Active;
                entity.UniqueId = "0";
                await _context.Set<ProjectMain>().AddAsync(entity);
                await _unitOfWork.Commit();
                var updatenetity = await _context.ProjectMains.FindAsync(entity.Id);

                // Generate the UniqueId using the entity's future ID
                string uniqueId = "00" + entity.Id.ToString("00");
                updatenetity.UniqueId = uniqueId;
                _context.Entry(updatenetity).State = EntityState.Modified;
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> AddProjectFeatureAsync(ProjectFeature entity)
        {

            ResponseDto response = new ResponseDto();
            try
            {
                await _context.Set<ProjectFeature>().AddAsync(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> AddProjectFileAsync(ProjectFile entity, IFormFile? file)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                var fileresponse = await _storageService.MainUploadFileReturnUrlAsync(null, file);
                if (fileresponse.Message.Contains("200"))
                {
                    entity.FileUrl = fileresponse.Url;
                    entity.FileKey = fileresponse.Key;
                }
            }
            catch (Exception x)
            {

            }
            try
            {
                await _context.Set<ProjectFile>().AddAsync(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> AddProjectPositionAsync(ProjectPosition entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                await _context.Set<ProjectPosition>().AddAsync(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> AddProjectTaskAsync(ProjectTask entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                await _context.Set<ProjectTask>().AddAsync(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> AddProjectTeamAsync(ProjectTeam entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                entity.Date = DateTime.UtcNow.AddHours(1);
                await _context.Set<ProjectTeam>().AddAsync(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> AddProjectUserAsync(ProjectUser entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                await _context.Set<ProjectUser>().AddAsync(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> DeleteGeneralFeatureAsync(GeneralFeature entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                _context.Set<GeneralFeature>().Remove(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> DeleteProjectFeatureAsync(ProjectFeature entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                _context.Set<ProjectFeature>().Remove(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> DeleteProjectPositionAsync(ProjectPosition entity)
        {

            ResponseDto response = new ResponseDto();
            try
            {
                _context.Set<ProjectPosition>().Remove(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> DeleteProjectTaskAsync(ProjectTask entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                _context.Set<ProjectTask>().Remove(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> DeleteProjectTeamAsync(ProjectTeam entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                _context.Set<ProjectTeam>().Remove(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> DeleteProjectUserAsync(ProjectUser entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                _context.Set<ProjectUser>().Remove(entity);
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<IReadOnlyList<GeneralFeature>> GetAllGeneralFeatureAsync()
        {
            return await _context.GeneralFeatures.ToListAsync();
        }

        public async Task<IReadOnlyList<ProjectListDto>> GetAllProjectByQuery(long id = 0, string? keyword = null, long RefId = 0, DateTime? startdate = null, DateTime? finishdate = null, DateTime? updatedate = null, ProjectStatus status = 0)
        {
            var listquery = _context.ProjectMains
                .Include(x => x.ProjectCategory)
                .Include(x => x.ProfileReferral)
                .Include(x => x.ProjectTeams)
                .ThenInclude(x => x.ProjectUsers)
                .AsQueryable();
            if (id > 0)
            {
                listquery = listquery.Where(x => x.ProjectCategoryId == id).AsQueryable();
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                listquery = listquery.Where(p =>
                    p.Title.Contains(keyword) ||
                    p.Description.Contains(keyword) ||
                    p.UniqueId.Contains(keyword) ||
                    p.Materials.Contains(keyword) ||
                    p.Note.Contains(keyword) ||
                    p.Urls.Contains(keyword) ||
                    p.Authentications.Contains(keyword));
            }


            if (startdate.HasValue)
            {
                listquery = listquery.Where(p => p.DateStarted >= startdate.Value);
            }

            if (finishdate.HasValue)
            {
                listquery = listquery.Where(p => p.DateFinised <= finishdate.Value);
            }

            if (status > 0)
            {
                listquery = listquery.Where(p => p.ProjectStatus == status);
            }

            var dtos = listquery.Select(ConvertToProductDto).ToList();
            return dtos.AsReadOnly();

        }


        private ProjectListDto ConvertToProductDto(ProjectMain data)
        {
            return new ProjectListDto
            {
                Id = data.Id,
                Title = data.Title,
                ProfileReferral = data.ProfileReferral.UserName,
                ProjectCategory = data.ProjectCategory.Title,
                Urls = data.Urls,
                DateStarted = data.DateStarted,
                DateFinised = data.DateFinised,
                LastUpdateDate = data.LastDateUpdated,
                ProjectStatus = data.ProjectStatus,
                Team = data.ProjectTeams.Count(),
                Users = data.ProjectTeams.Select(x => x.ProjectUsers).Count()

            };
        }
        public async Task<IReadOnlyList<ProjectReportDto>> GetAllProjectChallengesAsync(long projectId)
        {
            var reports = await _context.ProjectMains
        .Where(p => p.Id == projectId)
        .SelectMany(p => p.ProjectFeatures.SelectMany(pf => pf.ProjectTasks))
        .Select(pr => new ProjectReportDto
        {
            ProjectTaskId = pr.Id,
            ProjectId = projectId,
            Report = pr.Challenges,
            Username = pr.Profile.UserName
        })
        .ToListAsync();

            return reports;
        }

        public async Task<IReadOnlyList<ProjectFeature>> GetAllProjectFeatureAsync(long projectId)
        {
            return await _context.ProjectFeatures.Where(x => x.ProjectMainId == projectId).ToListAsync();
        }

        public async Task<IReadOnlyList<ProjectFile>> GetAllProjectFileAsync(long projectId)
        {
            return await _context.ProjectFiles.Where(x => x.ProjectMainId == projectId).ToListAsync();
        }

        public async Task<IReadOnlyList<ProjectPosition>> GetAllProjectPositionAsync()
        {
            return await _context.ProjectPositions.ToListAsync();
        }

        public async Task<IReadOnlyList<ProjectReportDto>> GetAllProjectReportsAsync(long projectId)
        {
            var reports = await _context.ProjectMains
      .Where(p => p.Id == projectId)
      .SelectMany(p => p.ProjectFeatures.SelectMany(pf => pf.ProjectTasks))
      .Select(pr => new ProjectReportDto
      {
          ProjectTaskId = pr.Id,
          ProjectId = projectId,
          Report = pr.Report,
          Username = pr.Profile.UserName
      })
      .ToListAsync();

            return reports;
        }

        public async Task<IReadOnlyList<ProjectTask>> GetAllProjectTaskAsync(long featureId)
        {
            return await _context.ProjectTasks.Where(x => x.ProjectFeatureId == featureId).ToListAsync();
        }

        public async Task<IReadOnlyList<ProjectTeam>> GetAllProjectTeamAsync(long projectId)
        {
            return await _context.ProjectTeams.Include(x => x.ProjectUsers).ThenInclude(x => x.Profile).Where(x => x.ProjectMainId == projectId).ToListAsync();
        }

        public async Task<IReadOnlyList<ProjectUser>> GetAllProjectUserAsync(long projectId)
        {
            var projectUsers = await _context.ProjectUsers
                   .Where(pu => pu.ProjectTeam.ProjectMainId == projectId)
                   .ToListAsync();

            return projectUsers;
        }

        public async Task<GeneralFeature> GetGeneralFeatureByIdAsync(long id)
        {
            return await _context.Set<GeneralFeature>().FindAsync(id);
        }

        public async Task<ProjectFeature> GetProjectFeatureByIdAsync(long id)
        {
            return await _context.Set<ProjectFeature>().FindAsync(id);
        }

        public async Task<ProjectFileDto> GetProjectFileByIdAsync(long id)
        {
            var projectFile = await _context.Set<ProjectFile>().FindAsync(id);

            if (projectFile == null)
            {
                return null; // Or throw an exception, depending on your requirements
            }

            // Map ProjectFile to ProjectFileDto
            var projectFileDto = new ProjectFileDto
            {
                Title = projectFile.Title,
                Url = projectFile.FileUrl
                // Map other properties as needed
            };

            return projectFileDto;
        }

        public async Task<ProjectPosition> GetProjectPositionByIdAsync(long id)
        {
            return await _context.Set<ProjectPosition>().FindAsync(id);
        }

        public async Task<ProjectTask> GetProjectTaskByIdAsync(long id)
        {
            var response = await _context.Set<ProjectTask>().FindAsync(id);
            return response;
        }

        public async Task<ProjectTeam> GetProjectTeamByIdAsync(long id)
        {
            return await _context.Set<ProjectTeam>().FindAsync(id);
        }

        public async Task<ProjectUser> GetProjectUserByIdAsync(long id)
        {
            return await _context.Set<ProjectUser>().FindAsync(id);
        }

        public async Task<ResponseDto> UpdateGeneralFeatureAsync(GeneralFeature entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> UpdateProjectAsync(ProjectEditDto entity, IFormFile? file)
        {
            ResponseDto response = new ResponseDto();
            var updateproject = await GetByIdAsync(entity.Id);
            if (updateproject != null)
            {


                updateproject.Title = entity.Title;
                updateproject.Description = entity.Description;
                updateproject.Materials = entity.Materials;
                updateproject.Note = entity.Note;
                updateproject.Authentications = entity.Authentications;
                updateproject.ProfileReferralId = entity.ProfileReferralId;
                updateproject.ProjectCategoryId = entity.ProjectCategoryId;
                updateproject.Budget = entity.Budget;
                updateproject.FinancialReport = entity.FinancialReport;
                updateproject.AmountSpent = entity.AmountSpent;

                updateproject.Urls = entity.Urls;
                updateproject.LastDateUpdated = entity.LastUpdateDate;
                updateproject.ProjectStatus = entity.ProjectStatus;


            }
            try
            {
                var fileresponse = await _storageService.MainUploadFileReturnUrlAsync(updateproject.LogoKey, file);
                if (fileresponse.Message.Contains("200"))
                {
                    updateproject.LogoUrl = fileresponse.Url;
                    updateproject.LogoKey = fileresponse.Key;
                }
            }
            catch (Exception x)
            {

            }
            try
            {
                updateproject.LastDateUpdated = DateTime.UtcNow.AddHours(1);
                _context.Entry(updateproject).State = EntityState.Modified;
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> UpdateProjectFeatureAsync(ProjectFeature entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> UpdateProjectFileAsync(ProjectFile entity, IFormFile? file)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                var fileresponse = await _storageService.MainUploadFileReturnUrlAsync(entity.FileKey, file);
                if (fileresponse.Message.Contains("200"))
                {
                    entity.FileUrl = fileresponse.Url;
                    entity.FileKey = fileresponse.Key;
                }
            }
            catch (Exception x)
            {

            }
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> UpdateProjectPositionAsync(ProjectPosition entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> UpdateProjectTaskAsync(ProjectTask entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> UpdateProjectTeamAsync(ProjectTeam entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ResponseDto> UpdateProjectUserAsync(ProjectUser entity)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _unitOfWork.Commit();
                response.Result = "Success";
            }
            catch (Exception c)
            {
                response.Result = "error";
            }
            return response;
        }

        public async Task<ProjectDto> GetProjectIdAsync(long id)
        {
            var data = await _context.ProjectMains
                .Include(x => x.ProjectCategory)
                .Include(x => x.ProfileReferral)
                .Include(x => x.ProjectFeatures).ThenInclude(x => x.ProjectTasks).ThenInclude(x => x.Profile)
                .Include(x => x.ProjectFeatures).ThenInclude(x => x.GeneralFeature)

                .Include(x => x.ProjectTeams).ThenInclude(x => x.ProjectUsers)
                .Include(x => x.ProjectFiles)
                .FirstOrDefaultAsync(x => x.Id == id);
            string durationvalue = "";
            CultureInfo nairaCulture = new CultureInfo("en-NG");
            // Retrieve the ProjectUsers from the ProjectTeams 
            var projectUsers = _context.ProjectTeams
    .Where(pt => data.ProjectTeams.Contains(pt))
    .SelectMany(pt => pt.ProjectUsers)
    .Include(pu => pu.Profile)
    .GroupBy(pu => pu.Profile.UserName)
    .Select(g => g.First())
    .ToList();
            if (data.DateFinised != null && data.DateStarted != null)
            {
                TimeSpan duration = data.DateFinised.Value - data.DateStarted.Value;
                int durationValuex = (int)duration.TotalDays;
                durationvalue = durationValuex.ToString();
            }
            else
            {
                durationvalue = "Not Set";
            }
            return new ProjectDto
            {
                Id = data.Id,
                UniqueID = data.UniqueId,
                Title = data.Title,
                Description = data.Description,
                Materials = data.Materials,
                Note = data.Note,
                Authentications = data.Authentications,
                ProfileReferral = data.ProfileReferral.UserName,
                ProjectCategory = data.ProjectCategory.Title,
                LogoPicture = data.LogoUrl,
                Urls = data.Urls,
                DateStarted = data.DateStarted,
                DateFinised = data.DateFinised,
                LastUpdateDate = data.LastDateUpdated,
                ProjectStatus = data.ProjectStatus,
                ProjectFeatures = data.ProjectFeatures,
                ProjectTeams = data.ProjectTeams,
                ProjectFiles = data.ProjectFiles,
                Budget = data.Budget.ToString("C", nairaCulture),
                AmountSpent = data.AmountSpent.ToString("C", nairaCulture),
                FinancialReport = data.FinancialReport,
                Duration = durationvalue,
                ProjectUsers = projectUsers



            };
        }



        public async Task<IReadOnlyList<ProjectFeatureListDto>> GetAllProjectFeatureSpecificListAsync(long projectId)
        {
            var listquery = _context.ProjectFeatures
                .Include(x => x.ProjectTasks)
                .Include(x => x.GeneralFeature).Where(x => x.ProjectMainId == projectId)
                .AsQueryable();


            var dtos = listquery.Select(ConvertToProjectFeatureListDto).ToList();
            return dtos.AsReadOnly();

        }


        private ProjectFeatureListDto ConvertToProjectFeatureListDto(ProjectFeature data)
        {
            return new ProjectFeatureListDto
            {
                Id = data.Id,
                Title = data.GeneralFeature.Title,
                Date = $"{data.DateFinised?.ToString()} {data.DateStarted?.ToString()}",
                TotalTask = data.ProjectTasks.Count(),
                TotalDone = data.ProjectTasks.Where(x => x.ProjectTaskStatus == ProjectTaskStatus.Done).Count(),
                TotalTodo = data.ProjectTasks.Where(x => x.ProjectTaskStatus == ProjectTaskStatus.ToDo).Count(),
                TotalDoing = data.ProjectTasks.Where(x => x.ProjectTaskStatus == ProjectTaskStatus.Doing).Count(),
                TotalProgress = Convert.ToInt32(data.ProjectTasks
        .Where(x => x.ProjectTaskStatus == ProjectTaskStatus.Done)
        .Average(x => x.Percentage) * 100),
                ProjectFeatureStatus = data.ProjectFeatureStatus

            };
        }
    }
}