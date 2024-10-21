using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyOnline.BLL.Entities.DTOs.User;
using SurveyOnline.BLL.Entities.Enums;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.BLL.Services.Implementations;

public class UserService(SignInManager<User> signInManager, IMapper mapper) : IUserService
{
    public async Task<IEnumerable<UserForManagementDto>> GetAllUsersAsync()
    {
        var users = await signInManager.UserManager.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ToListAsync();
        
        var usersDto = mapper.Map<IEnumerable<User>, IEnumerable<UserForManagementDto>>(users);
        
        return usersDto;
    }

    public async Task<IEnumerable<UserForSearchingDto>> GetUsersByQueryAsync(string query, int usersCount)
    {
        var queryLower = query.ToLower();
        var users = await signInManager.UserManager.Users.
            Where(u => u.Email.ToLower().Contains(queryLower) 
                          || u.UserName.ToLower().Contains(queryLower))
            .Take(usersCount)
            .ToListAsync();
        
        var usersDto = mapper.Map<IEnumerable<User>, IEnumerable<UserForSearchingDto>>(users);
        
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