using SurveyOnline.BLL.Entities.DTOs.User;

namespace SurveyOnline.BLL.Services.Contracts;

public interface IAuthenticationService // TODO
{
    Task RegisterAsync(UserForRegistrationDto userDto);
    Task LoginAsync(UserForLoginDto userDto);
    Task LogoutAsync();
}