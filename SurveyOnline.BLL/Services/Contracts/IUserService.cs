using SurveyOnline.BLL.Entities.DTOs.User;
using SurveyOnline.BLL.Entities.Enums;

namespace SurveyOnline.BLL.Services.Contracts;

public interface IUserService
{
    Task<IEnumerable<UserForManagementDto>> GetAllUsersAsync();
    Task<IEnumerable<UserForSearchingDto>> GetUsersByQueryAsync(string query, int usersCount);
    Task DeleteUserAsync(Guid userId);
    Task SetUserStatusAsync(Guid userId, Status status);
}