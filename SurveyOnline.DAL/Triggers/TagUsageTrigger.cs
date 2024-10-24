using EntityFrameworkCore.Triggered;
using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.DAL.Triggers;

public class TagUsageTrigger : IBeforeSaveTrigger<Survey>
{
    public Task BeforeSave(ITriggerContext<Survey> context, CancellationToken cancellationToken)
    {
        switch (context.ChangeType)
        {
            case ChangeType.Added: ChangeCount(context.Entity, 1);
                break;
            case ChangeType.Deleted: ChangeCount(context.Entity, -1);
                break;
        }
        
        return Task.CompletedTask;
    }

    private void ChangeCount(Survey survey,int count)
    {
        foreach (var surveyTag in survey.Tags)
        {
            surveyTag.UsageCount += count;
        }
    }
}