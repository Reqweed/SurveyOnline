using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SurveyOnline.BLL.Entities.DTOs.Question;
using SurveyOnline.BLL.Entities.DTOs.Survey;
using SurveyOnline.BLL.Entities.DTOs.Tag;
using SurveyOnline.BLL.Entities.DTOs.Topic;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Enums;

namespace SurveyOnline.Web.Pages;

public class SurveyConstructor(IServiceManager serviceManager) : PageModel
{
    public IEnumerable<TagDto> Tags { get; private set; }
    public IEnumerable<TopicDto> Topics { get; private set; }
    [BindProperty] public SurveyForCreatedDto Survey { get; set; }
    [BindProperty] public List<QuestionForCreatedDto> Questions { get; set; } = new();
    [BindProperty] public List<Guid> SelectedTags { get; set; } = new();
    [BindProperty] public List<Guid> SelectedUsers { get; set; } = new();
    
    public async Task OnGetAsync()
    {
        await LoadInitialDataAsync();
        InitializeDefaultQuestions();
    }

    public async Task<IActionResult> OnPostSubmitFormAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadInitialDataAsync();
            return Page();
        }
        
        await serviceManager.Survey.CreateSurveyAsync(Survey, Questions, SelectedTags, SelectedUsers);

        return RedirectToPage(nameof(SurveyConstructor));
    }

    public IActionResult OnPostAddQuestion()
    {
        Questions.Add(new QuestionForCreatedDto());
        ModelState.Clear();
        
        return Page();
    }

    public IActionResult OnPostRemoveQuestion(int questionIndex)
    {
        if (IsValidQuestionIndex(questionIndex))
        {
            Questions.RemoveAt(questionIndex);
        }

        ModelState.Clear();
        return Page();
    }

    [ValidateAntiForgeryToken]
    public async Task<JsonResult> OnPostSearchUsersAsync([FromBody] string query)
    {
        var users = await serviceManager.User.GetUsersByQueryAsync(query, 10);
        
        return new JsonResult(users);
    }

    private bool IsValidQuestionIndex(int index)
    {
        return index >= 0 && index < Questions.Count;
    }

    private void InitializeDefaultQuestions()
    {
        if (Questions.Count == 0)
        {
            Questions = new List<QuestionForCreatedDto>
            {
                new()
                {
                    Title = "What is your name?", 
                    Description = "Please enter your name.",
                    Type = QuestionType.SingleLine,
                    IsVisible = true
                },
                new()
                {
                    Title = "How old are you?",
                    Description = "Please enter your age.", 
                    Type = QuestionType.Integer,
                    IsVisible = true
                }
            };
        }
    }

    private async Task LoadInitialDataAsync()
    {
        Tags = await serviceManager.Tag.GetAllTagsAsync();
        Topics = await serviceManager.Topic.GetAllTopicsAsync();
    }
}