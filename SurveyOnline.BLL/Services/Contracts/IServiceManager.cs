namespace SurveyOnline.BLL.Services.Contracts;

public interface IServiceManager
{
    IAuthenticationService Authentication { get; }
    IAuthorizationService Authorization { get;}
    IUserService User { get; }
    ITopicService Topic { get; }
    ISurveyService Survey { get; }
    ITagService Tag { get; }
}