using Microsoft.AspNetCore.Identity;

namespace SurveyOnline.DAL.Entities.Models;

public class User : IdentityUser<Guid>
{
    public ICollection<Survey> OwnSurveys { get; set; }
    public ICollection<CompletedSurvey> CompletedSurveys { get; set; }
    public ICollection<Survey> AccessibleSurveys { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}