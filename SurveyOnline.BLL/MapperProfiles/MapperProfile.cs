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
        CreateMap<User, UserForManagementDto>()
            .ConstructUsing(src => new UserForManagementDto(
                src.Id, 
                src.UserName, 
                src.Email,
                null, // TODO
                src.LockoutEnd > DateTimeOffset.UtcNow ? Status.Block : Status.Unblock));

        CreateMap<Topic, TopicDto>();
        
        CreateMap<Tag, TagDto>();
        CreateMap<TagDto, Tag>();

        CreateMap<SurveyForCreatedDto, Survey>();
        
        CreateMap<QuestionForCreatedDto, Question>();
    }
}