using System.ComponentModel.DataAnnotations;

namespace SurveyOnline.BLL.Entities.DTOs.User;

public record UserForRegistrationDto {
    [Required] 
    public string UserName { get; init; }
    [Required] 
    [EmailAddress] 
    public  string Email { get; init; }
    [Required]
    public string Password { get; init; }
    [Required] 
    [Compare(nameof(Password))] 
    public string ConfirmPassword { get; init; }
}