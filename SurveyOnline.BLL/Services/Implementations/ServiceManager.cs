using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SurveyOnline.BLL.Helpers.Contracts;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.BLL.Services.Implementations;

public class ServiceManager(SignInManager<User> signInManager, IRepositoryManager repositoryManager,
        ICloudinaryService cloudinaryService, ISurveySearchService surveySearchService, IMapper mapper)
    : IServiceManager
{
    private readonly Lazy<IAuthenticationService> _authenticationService
        = new(() => new AuthenticationService(signInManager, mapper));
    private readonly Lazy<IAuthorizationService> _authorizationService
        = new(() => new AuthorizationService(signInManager));
    private readonly Lazy<IUserService> _userService
        = new(() => new UserService(signInManager, mapper));
    private readonly Lazy<ISurveyService> _surveyService
        = new(() => new SurveyService(signInManager, repositoryManager, 
            cloudinaryService, surveySearchService, mapper));
    private readonly Lazy<ITopicService> _topicService
        = new(() => new TopicService(repositoryManager, mapper));
    private readonly Lazy<ITagService> _tagService
        = new(() => new TagService(repositoryManager, mapper));
    private readonly Lazy<ICompletedSurveyService> _completedSurveyService
        = new(() => new CompletedSurveyService(signInManager, repositoryManager, mapper));

    public IAuthenticationService Authentication => _authenticationService.Value;
    public IAuthorizationService Authorization => _authorizationService.Value;
    public IUserService User => _userService.Value;
    public ISurveyService Survey => _surveyService.Value;
    public ICompletedSurveyService CompletedSurvey => _completedSurveyService.Value;
    public ITopicService Topic => _topicService.Value;
    public ITagService Tag => _tagService.Value;
}