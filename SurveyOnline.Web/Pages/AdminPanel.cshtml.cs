using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SurveyOnline.BLL.Entities.DTOs.User;
using SurveyOnline.BLL.Entities.Enums;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Enums;

namespace SurveyOnline.Web.Pages;

public class AdminPanel(IServiceManager serviceManager) : PageModel
{
    [BindProperty] 
    public List<Guid> SelectedUsers { get; set; } = new();
    [BindProperty] 
    public IEnumerable<UserForManagementDto> Users => serviceManager.User.GetAllUser();
    
    public void OnGet()
    {

    }
    
    public async Task<IActionResult> OnPostBlockAsync()
    {
        foreach (var userId in SelectedUsers)
        {
            await serviceManager.User.SetUserStatusAsync(userId, Status.Block);
        }

        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostUnblockAsync()
    {
        foreach (var userId in SelectedUsers)
        {
            await serviceManager.User.SetUserStatusAsync(userId, Status.Unblock);
        }

        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostDeleteAsync()
    {
        foreach (var userId in SelectedUsers)
        {
            await serviceManager.User.DeleteUserAsync(userId);
        }

        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostMakeAdminAsync()
    {
        foreach (var userId in SelectedUsers)
        {
            await serviceManager.Authorization.ChangeUserRoleAsync(userId, RoleType.Admin);
        }

        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostMakeUserAsync()
    {
        foreach (var userId in SelectedUsers)
        {
            await serviceManager.Authorization.ChangeUserRoleAsync(userId, RoleType.User);
        }

        return RedirectToPage();
    }
}