using Microsoft.AspNetCore.Identity;

namespace SurveyOnline.DAL.Entities.Models;

public class Role : IdentityRole<Guid>
{
    public virtual ICollection<UserRole> UserRoles { get; set; }
}