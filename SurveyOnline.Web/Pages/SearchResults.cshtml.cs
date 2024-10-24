using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SurveyOnline.BLL.Entities.DTOs.Survey;
using SurveyOnline.BLL.Services.Contracts;

namespace SurveyOnline.Web.Pages;

public class SearchResults(IServiceManager serviceManager) : PageModel
{
    [BindProperty] public List<SurveyDto> Surveys { get; set; } = new();

    public async Task OnGet([FromQuery] string tag)
    {
        if (!string.IsNullOrEmpty(tag))
        {
            Surveys = (await serviceManager.Survey.GetSurveysByTag(tag)).ToList();
        }
    }
    
    public async Task<IActionResult> OnPostSearchByTerm([FromForm] string search)
    {
        if (!string.IsNullOrEmpty(search))
        {
            Surveys = (await serviceManager.Survey.GetSurveysByTerm(search)).ToList();
        }

        return Page();
    }
}