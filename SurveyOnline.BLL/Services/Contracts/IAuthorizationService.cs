using SurveyOnline.DAL.Entities.Enums;

namespace SurveyOnline.BLL.Services.Contracts;

public interface IAuthorizationService // TODO
{
    Task ChangeUserRoleAsync(Guid userId, RoleType roleType);
}