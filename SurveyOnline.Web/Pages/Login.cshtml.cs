using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SurveyOnline.BLL.Entities.DTOs.User;
using SurveyOnline.BLL.Services.Contracts;

namespace SurveyOnline.Web.Pages;

public class Login(IServiceManager serviceManager) : PageModel
{
    [BindProperty]
    public UserForLoginDto User { get; set; }
    
    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPost()
    {
       await serviceManager.Authentication.LoginAsync(User);

       return RedirectToPage(nameof(Index));
    }
}