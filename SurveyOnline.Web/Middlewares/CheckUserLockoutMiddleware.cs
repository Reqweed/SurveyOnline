using Microsoft.AspNetCore.Identity;
using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.Web.Middlewares;

public class CheckUserLockoutMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var signInManager = context.RequestServices.GetService<SignInManager<User>>();
        if (context.User.Identity is { IsAuthenticated: true })
        {
            var user = await signInManager?.UserManager.GetUserAsync(context.User);
            if (user != null)
            {
                if (await signInManager.UserManager.IsLockedOutAsync(user))
                {
                    await Logout(context, signInManager);
                    return;
                }
            }
            else
            {
                await Logout(context, signInManager);
                return;
            }
        }
        
        await next(context);
    }

    private async Task Logout(HttpContext context, SignInManager<User> signInManager)
    {
        await signInManager.SignOutAsync();
        context.Response.Redirect($"/{nameof(Index)}");
    }
}