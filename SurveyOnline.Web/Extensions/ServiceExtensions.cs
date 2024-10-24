using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.EntityFrameworkCore;
using SurveyOnline.BLL.Helpers.Contracts;
using SurveyOnline.BLL.Helpers.Implementations;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.BLL.Services.Implementations;
using SurveyOnline.DAL.Contexts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Initializers.Contracts;
using SurveyOnline.DAL.Initializers.Implementations;
using SurveyOnline.DAL.Repositories.Contracts;
using SurveyOnline.DAL.Repositories.Implementations;
using SurveyOnline.DAL.Triggers;
using SurveyOnline.Web.Middlewares;
using SurveyOnline.Web.Pages;
using Index = SurveyOnline.Web.Pages.Index;

namespace SurveyOnline.Web.Extensions;

public static class ServiceExtensions
{
    public static void AddContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
        
        serviceCollection.AddDbContext<PostgresDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseTriggers(triggerOptions =>
            {
                triggerOptions.AddTrigger<SurveyCompletedTrigger>();
                triggerOptions.AddTrigger<TagUsageTrigger>();
            });
        });
    }
    
    public static void AddIdentity(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            })
            .AddEntityFrameworkStores<PostgresDbContext>();
        
        serviceCollection.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = $"/{nameof(Register)}"; 
            options.LogoutPath = $"/{nameof(Index)}";
        });
    }
    
    public static void AddRepositoryManager(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IRepositoryManager, RepositoryManager>();
    }
    
    public static void AddMapper(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(typeof(SurveyOnline.BLL.MapperProfiles.MapperProfile).Assembly); // TODO
    }
    
    public static void AddHelpers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICloudinaryService, CloudinaryService>();
    }
    
    public static void AddServiceManger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IServiceManager, ServiceManager>();
    }
    
    public static void AddMiddlewares(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<CheckUserLockoutMiddleware>();
    }

    public static void AddElasticSearch(this IServiceCollection serviceCollection)
    {
        var apiKey = Environment.GetEnvironmentVariable("ELASTIC_API_KEY");
        var url = Environment.GetEnvironmentVariable("ELASTIC_URL");
        var settings =
            new ElasticsearchClientSettings(
                    new Uri(url))
                .DefaultIndex("survey")
                .Authentication(new ApiKey(apiKey))
                .EnableDebugMode()
                .PrettyJson()
                .DisableDirectStreaming();

        var client = new ElasticsearchClient(settings);
        
        serviceCollection.AddSingleton<ISurveySearchService>(new SurveySearchService(client));
    }

    public static void AddDbInitializer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IDbInitializer, DbInitializer>();
    }

    public static void AddLocalizations(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddLocalization(options => options.ResourcesPath = "Resources");
    }
}