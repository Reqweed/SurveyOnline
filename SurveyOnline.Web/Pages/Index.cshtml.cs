using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SurveyOnline.BLL.Services.Contracts;

namespace SurveyOnline.Web.Pages;

public class Index(IServiceManager serviceManager) : PageModel
{
    public void OnGet()
    {
    }
    
    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await serviceManager.Authentication.LogoutAsync();
        return RedirectToPage();
    }
}