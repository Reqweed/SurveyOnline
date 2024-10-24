using Elastic.Clients.Elasticsearch;
using SurveyOnline.BLL.Entities.DTOs.Survey;
using SurveyOnline.BLL.Helpers.Contracts;

namespace SurveyOnline.BLL.Helpers.Implementations;

public class SurveySearchService(ElasticsearchClient client) : ISurveySearchService
{
    private const string Index = "survey";
    
    public async Task AddIndexAsync(SurveyForIndexDto surveyDto)
    {
        var resp = await client.IndexAsync(surveyDto);
        if (!resp.IsValidResponse)
            throw new Exception();
    }

    public async Task<IEnumerable<SurveyForIndexDto>> SearchSurveyByTagAsync(string tagName)
    {
        if (string.IsNullOrEmpty(tagName))
            throw new Exception();
        
        var response = await client.SearchAsync<SurveyForIndexDto>(s => s
            .Index(Index)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.Tags.First().Name)
                    .Query(tagName)
                )
            )
        );
        
        if (!response.IsValidResponse)
            throw new Exception();

        return response.Documents.AsEnumerable();
    }

    public async Task<IEnumerable<SurveyForIndexDto>> SearchSurveysAsync(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            throw new Exception();
        
        var response = await client.SearchAsync<SurveyForIndexDto>(s => s
            .Index(Index)
            .Query(q => q
                .Bool(b => b
                    .Should(
                        m => m.Match(mf => mf
                            .Field(f => f.Tags.First().Name)
                            .Query(searchTerm)
                        ),
                        m => m.Match(mf => mf
                            .Field(f => f.Questions.First().Description)
                            .Query(searchTerm)
                        ),
                        m => m.Match(mf => mf
                            .Field(f => f.Questions.First().Title)
                            .Query(searchTerm)
                        ),
                        m => m.Match(mf => mf
                            .Field(f => f.Description)
                            .Query(searchTerm)
                        ),
                        m => m.Match(mf => mf
                            .Field(f => f.Title)
                            .Query(searchTerm)
                        ),
                        m => m.Match(mf => mf
                            .Field(f => f.Topic)
                            .Query(searchTerm)
                        )
                    )
                )
            )
        );

        if (!response.IsValidResponse)
            throw new Exception();

        return response.Documents.AsEnumerable();
    }
}