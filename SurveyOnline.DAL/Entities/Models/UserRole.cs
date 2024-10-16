using Microsoft.AspNetCore.Identity;

namespace SurveyOnline.DAL.Entities.Models;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}