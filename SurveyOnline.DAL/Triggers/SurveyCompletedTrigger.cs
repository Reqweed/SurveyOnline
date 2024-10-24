using EntityFrameworkCore.Triggered;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.DAL.Triggers;

public class SurveyCompletedTrigger(IRepositoryManager repositoryManager) : IBeforeSaveTrigger<CompletedSurvey>
{
    public async Task BeforeSave(ITriggerContext<CompletedSurvey> context, CancellationToken cancellationToken)
    {
        if (context.ChangeType == ChangeType.Added)
        {
            var survey = await repositoryManager.Survey.GetSurveyByIdAsync(context.Entity.SurveyId, trackChanges: true);
            if (survey is not null)
                survey.CompletedCount += 1;
        }
    }
}