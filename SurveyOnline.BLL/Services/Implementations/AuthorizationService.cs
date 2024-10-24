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
            throw new KeyNotFoundException($"User with ID {userId} not found.");

        await RemoveRoleToUserAsync(user);
        await AssignRoleToUserAsync(user, roleType);
    }

    private async Task RemoveRoleToUserAsync(User user)
    {
        var roles = await signInManager.UserManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            var removeResult = await signInManager.UserManager.RemoveFromRoleAsync(user, role);
            if (!removeResult.Succeeded)
                throw new InvalidOperationException($"Failed to remove role {role} from user {user.UserName}.");
        }
    }
    
    private async Task AssignRoleToUserAsync(User user, RoleType roleType)
    {
        var result = await signInManager.UserManager.AddToRoleAsync(user, roleType.ToString());
        if (!result.Succeeded) 
            throw new InvalidOperationException($"Failed to add role {roleType} to user {user.UserName}.");
    }
}