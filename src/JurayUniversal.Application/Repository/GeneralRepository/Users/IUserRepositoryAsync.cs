using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Dtos.Project;
using JurayUniversal.Application.Dtos.UsersDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Application.Repository.GeneralRepository.Users
{
    public interface IUserRepositoryAsync
    {
        Task<IReadOnlyList<ListUsersDto>> GetAllDeletedUsersAsync();
        Task<IReadOnlyList<ListUsersDto>> GetAllUsersAsync();

         Task<IReadOnlyList<ListUsersDto>> GetDashboardAllDeletedUsersAsync();
        Task<IReadOnlyList<ListUsersDto>> GetDashboardAllUsersAsync();


        Task<IList<string>> GetDashboardRolesAsync();
        Task UpdateAsync(UserDto entity, IFormFile? file);
        Task<ResponseDto> AddAsync(UserDto entity, IFormFile? file);
        
        Task<UserDto> GetByIdAsync(string id);
        Task<UserDetailsDto> GetUserDetailsByIdAsync(string id);
        Task DeleteAsync(string id);

        Task<ResponseDto> ChangeEmailAsync(ChangeEmailDto entity);
        Task<ResponseDto> SendEmailAsync(string id, EmailType emailType, string message);

         Task<FullProfileDto> GetFullUserDetailsByIdAsync(string id);
    }
}
