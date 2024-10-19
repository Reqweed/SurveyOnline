using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SurveyOnline.BLL.Entities.DTOs.Tag;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.BLL.Services.Implementations;

public class TagService(IRepositoryManager repositoryManager, IMapper mapper) : ITagService
{
    public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
    {
        var tags = await repositoryManager.Tag.GetAllTags(false).ToListAsync();

        var tagsDto = mapper.Map<IEnumerable<Tag>, IEnumerable<TagDto>>(tags);

        return tagsDto;
    }

    public async Task<IEnumerable<TagForCloudDto>> GetAllTagsForCloudAsync()
    {
        var tags = await repositoryManager.Tag.GetAllTags(false).ToListAsync();

        var tagsDto = mapper.Map<IEnumerable<Tag>, IEnumerable<TagForCloudDto>>(tags);

        return tagsDto;
    }
}