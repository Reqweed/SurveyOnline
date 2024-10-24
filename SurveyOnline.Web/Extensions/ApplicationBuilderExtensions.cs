using Microsoft.EntityFrameworkCore;
using SurveyOnline.BLL.Helpers.Contracts;
using SurveyOnline.DAL.Contexts;
using SurveyOnline.DAL.Initializers.Contracts;

namespace SurveyOnline.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task MigrateDatabaseAndInitializeDataAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<PostgresDbContext>();
        var dataInitializer = services.GetRequiredService<IDbInitializer>();
        var surveySearchService = services.GetRequiredService<ISurveySearchService>();
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await context.Database.MigrateAsync();
            await dataInitializer.Initialise();
            await surveySearchService.DeleteIndexAsync();
        }
    }
}