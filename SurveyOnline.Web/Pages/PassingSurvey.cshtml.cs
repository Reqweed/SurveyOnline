using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SurveyOnline.BLL.Entities.DTOs.Survey;
using SurveyOnline.BLL.Services.Contracts;

namespace SurveyOnline.Web.Pages;

[Authorize]
public class PassingSurvey(IServiceManager serviceManager) : PageModel
{
    [BindProperty]
    public SurveyForCompletedDto Survey { get; set; }

    [BindProperty]
    public List<string> Answers { get; set; }
    
    public async Task OnGet([FromQuery] Guid id)
    {
        Survey = await serviceManager.Survey.GetSurveyAsync(id);
        Answers = new List<string>(Survey.Questions.Count);
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        await serviceManager.CompletedSurvey.AddCompletedSurveyAsync(Survey, Answers);
        
        return RedirectToPage(nameof(Index));
    }
}