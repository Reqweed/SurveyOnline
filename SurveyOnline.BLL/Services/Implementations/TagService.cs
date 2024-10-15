using AutoMapper;
using SurveyOnline.BLL.Entities.DTOs.Tag;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.BLL.Services.Implementations;

public class TagService(IRepositoryManager repositoryManager, IMapper mapper) : ITagService
{
    public IEnumerable<TagDto> GetAllTags()
    {
        var tags = repositoryManager.Tag.GetAllTags(false);

        var tagsDto = mapper.Map<IEnumerable<Tag>, IEnumerable<TagDto>>(tags);

        return tagsDto;
    }
}