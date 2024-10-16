using Microsoft.AspNetCore.Identity;

namespace SurveyOnline.DAL.Entities.Models;

public class User : IdentityUser<Guid>
{
    public virtual ICollection<Survey> OwnSurveys { get; set; }
    public virtual ICollection<CompletedSurvey> CompletedSurveys { get; set; }
    public virtual ICollection<Survey> AccessibleSurveys { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
}