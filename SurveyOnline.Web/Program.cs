using Microsoft.AspNetCore.Mvc.Razor;
using SurveyOnline.Web.Extensions;
using SurveyOnline.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();
builder.Services.AddContext(builder.Configuration);
builder.Services.AddIdentity();
builder.Services.AddMapper();
builder.Services.AddRepositoryManager();
builder.Services.AddServiceManger();
builder.Services.AddHelpers();
builder.Services.AddMiddlewares();
builder.Services.AddElasticSearch();
builder.Services.AddDbInitializer();
builder.Services.AddLocalizations();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

var supportedCultures = new[] { "en", "ru" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<CheckUserLockoutMiddleware>();

app.MapRazorPages();

await app.MigrateDatabaseAndInitializeDataAsync();

app.Run();