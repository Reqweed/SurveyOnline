using Microsoft.AspNetCore.Identity;

namespace SurveyOnline.DAL.Entities.Models;

public class UserRole : IdentityUserRole<Guid>
{
    public User User { get; set; }
    public Role Role { get; set; }
}