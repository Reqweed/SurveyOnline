using System.ComponentModel.DataAnnotations;

namespace SurveyOnline.BLL.Entities.DTOs.User;

public record UserForLoginDto(
    [Required] [EmailAddress] string Email, 
    [Required] string Password);