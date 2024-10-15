using Microsoft.AspNetCore.Identity;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Enums;
using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.BLL.Services.Implementations;

public class AuthorizationService(SignInManager<User> signInManager) : IAuthorizationService
{
    public async Task ChangeUserRoleAsync(Guid userId, RoleType roleType)
    {
        var user = await signInManager.UserManager.FindByIdAsync(userId.ToString());
        if (user is null)
            throw new Exception();
        
        var roles = await signInManager.UserManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            await signInManager.UserManager.RemoveFromRoleAsync(user, role);
        }
        
        Console.WriteLine(roleType.ToString());
        var result = await signInManager.UserManager.AddToRoleAsync(user, roleType.ToString());
        if(!result.Succeeded) 
            throw new Exception();
    }
}