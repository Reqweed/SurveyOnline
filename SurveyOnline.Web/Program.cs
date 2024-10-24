using SurveyOnline.Web.Extensions;
using SurveyOnline.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddContext(builder.Configuration);
builder.Services.AddIdentity();
builder.Services.AddMapper();
builder.Services.AddRepositoryManager();
builder.Services.AddServiceManger();
builder.Services.AddHelpers();
builder.Services.AddMiddlewares();
builder.Services.AddElasticSearch();
builder.Services.AddDbInitializer();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<CheckUserLockoutMiddleware>();

app.MapRazorPages();

await app.MigrateDatabaseAndInitializeDataAsync();

app.Run();