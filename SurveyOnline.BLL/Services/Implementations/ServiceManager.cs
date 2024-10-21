using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SurveyOnline.BLL.Helpers.Contracts;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.BLL.Services.Implementations;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IAuthorizationService> _authorizationService;
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<ISurveyService> _surveyService;
    private readonly Lazy<ITopicService> _topicService;
    private readonly Lazy<ITagService> _tagService;
    private readonly Lazy<ICompletedSurveyService> _completedSurveyService;

    public ServiceManager(SignInManager<User> signInManager, IRepositoryManager repositoryManager,
        ICloudinaryService cloudinaryService, IMapper mapper)
    {
        _authenticationService =
            new Lazy<IAuthenticationService>(() => new AuthenticationService(signInManager, mapper));
        _authorizationService = new Lazy<IAuthorizationService>(() => new AuthorizationService(signInManager));
        _userService = new Lazy<IUserService>(() => new UserService(signInManager, mapper));
        _surveyService = new Lazy<ISurveyService>(() =>
            new SurveyService(signInManager, repositoryManager, cloudinaryService, mapper));
        _topicService = new Lazy<ITopicService>(() => new TopicService(repositoryManager, mapper));
        _tagService = new Lazy<ITagService>(() => new TagService(repositoryManager, mapper));
        _completedSurveyService =
            new Lazy<ICompletedSurveyService>(
                () => new CompletedSurveyService(signInManager, repositoryManager, mapper));
    }

    public IAuthenticationService Authentication => _authenticationService.Value;
    public IAuthorizationService Authorization => _authorizationService.Value;
    public IUserService User => _userService.Value;
    public ISurveyService Survey => _surveyService.Value;
    public ICompletedSurveyService CompletedSurvey => _completedSurveyService.Value;
    public ITopicService Topic => _topicService.Value;
    public ITagService Tag => _tagService.Value;
}