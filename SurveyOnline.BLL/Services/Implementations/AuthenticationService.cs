using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SurveyOnline.BLL.Entities.DTOs.User;
using SurveyOnline.DAL.Entities.Enums;
using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.BLL.Services.Implementations;

public class AuthenticationService(SignInManager<User> signInManager, IMapper mapper) : Contracts.IAuthenticationService
{
    public async Task RegisterAsync(UserForRegistrationDto userDto)
    {
        if (userDto is null)
            throw new ArgumentNullException(nameof(userDto), "User registration data cannot be null.");

        var user = await CreateUserAsync(userDto);

        await AssignRoleToUserAsync(user, RoleType.User);

        await signInManager.SignInAsync(user, isPersistent: false);
    }

    public async Task LoginAsync(UserForLoginDto userDto)
    {
        if (userDto is null)
            throw new ArgumentNullException(nameof(userDto), "User login data cannot be null.");

        var user = await FindUserByEmailAsync(userDto.Email);

        await ValidateUserPasswordAsync(user, userDto.Password);

        await signInManager.SignInAsync(user, isPersistent: false);
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }

    private async Task<User> CreateUserAsync(UserForRegistrationDto userDto)
    {
        var user = mapper.Map<UserForRegistrationDto, User>(userDto);

        var result = await signInManager.UserManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException("Failed to create user.");

        return user;
    }

    private async Task AssignRoleToUserAsync(User user, RoleType roleType)
    {
        var result = await signInManager.UserManager.AddToRoleAsync(user, roleType.ToString());
        if (!result.Succeeded)
            throw new InvalidOperationException($"Failed to add role {roleType} to user {user.UserName}.");
    }

    private async Task<User> FindUserByEmailAsync(string email)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(email);
        if (user == null)
            throw new InvalidOperationException($"No user found with email '{email}'.");

        return user;
    }

    private async Task ValidateUserPasswordAsync(User user, string password)
    {
        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
        if (result.IsLockedOut)
            throw new InvalidOperationException("User account has blocked.");
        if (!result.Succeeded)
            throw new InvalidOperationException("Invalid operation.");
    }
}