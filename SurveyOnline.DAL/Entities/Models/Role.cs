using Microsoft.AspNetCore.Identity;

namespace SurveyOnline.DAL.Entities.Models;

public class Role : IdentityRole<Guid>
{
    public ICollection<UserRole> UserRoles { get; set; }
}