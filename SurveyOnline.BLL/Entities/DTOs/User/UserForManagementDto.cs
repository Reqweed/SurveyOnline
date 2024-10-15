using SurveyOnline.BLL.Entities.Enums;

namespace SurveyOnline.BLL.Entities.DTOs.User;

// public record UserForManagementDto(
//     Guid Id, 
//     string UserName, 
//     string Email, 
//     string Role, 
//     Status Status
//     );
public class UserForManagementDto(Guid id, string userName, string email, string role, Status status)
{
    public Guid Id { get; set; } = id;
    public string UserName { get; set; } = userName;
    public string Email { get; set; } = email;
    public string Role { get; set; } = role;
    public Status Status { get; set; } = status;
}