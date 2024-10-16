using SurveyOnline.BLL.Entities.DTOs.User;
using SurveyOnline.BLL.Entities.Enums;

namespace SurveyOnline.BLL.Services.Contracts;

public interface IUserService // TODO
{
    Task<IEnumerable<UserForManagementDto>> GetAllUserAsync();
    Task DeleteUserAsync(Guid userId);
    Task SetUserStatusAsync(Guid userId, Status status);
}