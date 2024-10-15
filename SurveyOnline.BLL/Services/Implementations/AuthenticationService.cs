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
            throw new Exception();

        var user = mapper.Map<UserForRegistrationDto, User>(userDto);

        var result = await signInManager.UserManager.CreateAsync(user, userDto.Password);
        if (!result.Succeeded)
            throw new Exception();
        
        result = await signInManager.UserManager.AddToRoleAsync(user, RoleType.User.ToString());
        if(!result.Succeeded) 
            throw new Exception();

        await signInManager.SignInAsync(user, isPersistent: false);
    }

    public async Task LoginAsync(UserForLoginDto userDto)
    {
        if (userDto is null)
            throw new Exception();

        var user = await signInManager.UserManager.FindByEmailAsync(userDto.Email);
        
        if (user == null)
            throw new Exception();
        
        var result = await signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);
        if (result.IsLockedOut)
            throw new Exception();

        await signInManager.SignInAsync(user, isPersistent: false);
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }
}