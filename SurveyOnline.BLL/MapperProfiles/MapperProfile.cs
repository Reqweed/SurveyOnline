using AutoMapper;
using SurveyOnline.BLL.Entities.DTOs.Question;
using SurveyOnline.BLL.Entities.DTOs.Survey;
using SurveyOnline.BLL.Entities.DTOs.Tag;
using SurveyOnline.BLL.Entities.DTOs.Topic;
using SurveyOnline.BLL.Entities.DTOs.User;
using SurveyOnline.BLL.Entities.Enums;
using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.BLL.MapperProfiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserForRegistrationDto, User>();
        CreateMap<UserForLoginDto, User>();
        CreateMap<User, UserForSearchingDto>();
        CreateMap<User, UserForManagementDto>()
            .ConstructUsing(src => new UserForManagementDto(
                src.Id,
                src.UserName,
                src.Email,
                src.UserRoles.First().Role.Name,
                src.LockoutEnd > DateTimeOffset.UtcNow ? Status.Block : Status.Unblock)
            );

        CreateMap<Topic, TopicDto>();

        CreateMap<Tag, TagDto>();
        CreateMap<Tag, TagForCloudDto>()
            .ConstructUsing(src => new TagForCloudDto(
                src.Id,
                src.Name,
                src.UsageCount)
            );

        CreateMap<SurveyForCreatedDto, Survey>();
        CreateMap<Survey, SurveyDto>()
            .ConstructUsing(src => new SurveyDto(
                src.Id,
                src.Title,
                src.UrlImage,
                src.Description,
                src.Creator.UserName)
            );
        CreateMap<Survey, SurveyForCompletedDto>()
            .ConstructUsing((src, context) => new SurveyForCompletedDto(
                src.Id,
                src.Title,
                src.Description,
                src.UrlImage,
                src.Topic.Name,
                src.Creator.UserName,
                context.Mapper.Map<List<QuestionDto>>(src.Questions),
                context.Mapper.Map<List<TagDto>>(src.Tags))
            );

        CreateMap<QuestionForCreatedDto, Question>();
        CreateMap<Question, QuestionDto>();
    }
}