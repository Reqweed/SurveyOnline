using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SurveyOnline.BLL.Helpers.Contracts;
using SurveyOnline.BLL.Helpers.Implementations;
using SurveyOnline.BLL.Services.Contracts;
using SurveyOnline.BLL.Services.Implementations;
using SurveyOnline.DAL.Contexts;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Repositories.Contracts;
using SurveyOnline.DAL.Repositories.Implementations;
using SurveyOnline.DAL.Triggers;
using SurveyOnline.Web.Middlewares;

namespace SurveyOnline.Web.Extensions;

public static class ServiceExtensions
{
    public static void AddContext(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");
        
        serviceCollection.AddDbContext<PostgresDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseTriggers(triggerOptions => triggerOptions.AddTrigger<SurveyCompletedTrigger>());
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
        
        serviceCollection.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login"; // TODO
                options.LogoutPath = "/Logout";
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
}