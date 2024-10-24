using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SurveyOnline.BLL.Entities.DTOs.Survey;
using SurveyOnline.BLL.Entities.DTOs.Tag;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.Web.Pages;

public class Index(IServiceManager serviceManager, SignInManager<User> signInManager) : PageModel
{
    public IEnumerable<SurveyDto> SurveysTop { get; set; }
    public IEnumerable<TagForCloudDto> Tags { get; set; }
    public const int PageSize = 3;

    public async Task OnGetAsync()
    {
        SurveysTop = await serviceManager.Survey.GetTopCompletedSurveysAsync(5);
        Tags = await serviceManager.Tag.GetAllTagsForCloudAsync();
    }
    
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> OnPostLoadSurveys([FromBody] int page)
    {
        var surveys = await serviceManager.Survey.GetPagedAccessibleSurveysAsync(page, PageSize);
        
        return new JsonResult(surveys);
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await serviceManager.Authentication.LogoutAsync();
        
        return RedirectToPage();
    }
    
    public IActionResult OnPostSetLanguage(string culture)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return Redirect(Request.Headers["Referer"].ToString());
    }
}