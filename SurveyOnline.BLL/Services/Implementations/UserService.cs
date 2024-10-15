using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SurveyOnline.BLL.Entities.DTOs.User;
using SurveyOnline.BLL.Entities.Enums;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.BLL.Services.Implementations;

public class UserService(SignInManager<User> signInManager, IMapper mapper) : IUserService
{
    public IEnumerable<UserForManagementDto> GetAllUser() // TODO
    {
        var usersDto = signInManager.UserManager.Users.ToList().Select(u =>
        {
            var userDto = mapper.Map<User, UserForManagementDto>(u);
            userDto.Role = signInManager.UserManager.GetRolesAsync(u).Result[0];
            return userDto;
        });
        
        return usersDto;
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await signInManager.UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new Exception();
        
        var result = await signInManager.UserManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new Exception();
    }

    public async Task SetUserStatusAsync(Guid userId, Status status)
    {
        var user = await signInManager.UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new Exception();

        user.LockoutEnd = status switch
        {
            Status.Block => DateTimeOffset.MaxValue,
            Status.Unblock => null,
            _ => throw new Exception()
        };

        var result = await signInManager.UserManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new Exception();
    }
}