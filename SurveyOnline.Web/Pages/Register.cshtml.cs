using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SurveyOnline.BLL.Entities.DTOs.User;
using SurveyOnline.BLL.Services.Contracts;

namespace SurveyOnline.Web.Pages;

public class Register(IServiceManager serviceManager) : PageModel
{
    [BindProperty]
    public UserForRegistrationDto User { get; set; }

    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            await serviceManager.Authentication.RegisterAsync(User);
 
            return RedirectToPage(nameof(Index));   
        }

        return Page();
    }
}