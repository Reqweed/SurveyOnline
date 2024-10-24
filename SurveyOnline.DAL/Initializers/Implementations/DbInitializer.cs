using Microsoft.AspNetCore.Identity;
using SurveyOnline.DAL.Entities.Enums;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Initializers.Contracts;
using SurveyOnline.DAL.Repositories.Contracts;

namespace SurveyOnline.DAL.Initializers.Implementations;

public class DbInitializer(UserManager<User> userManager, RoleManager<Role> roleManager, IRepositoryManager repositoryManager) : IDbInitializer
{

    public async Task Initialise()
    {
        await InitialiseRoles();
        await InitialiseUsers();
        await InitialiseTopics();
        await InitialiseTags();
    }
    
    private async Task InitialiseRoles()
    {
        await roleManager.CreateAsync(new Role { Name = RoleType.User.ToString() });
        await roleManager.CreateAsync(new Role { Name = RoleType.Admin.ToString() });
    }
    
    private async Task InitialiseUsers()
    {
        var admin = new User { UserName = "admin", Email = "admin@gmail.com" };
        await userManager.CreateAsync(admin, "admin");
        await userManager.AddToRoleAsync(admin, RoleType.Admin.ToString());
    }
    
    private async Task InitialiseTopics()
    {
       await repositoryManager.Topic.CreateTopicAsync(new Topic { Name = "Education" });
       await repositoryManager.Topic.CreateTopicAsync(new Topic { Name = "Health and Well-being" });
       await repositoryManager.Topic.CreateTopicAsync(new Topic { Name = "Technology" });
       await repositoryManager.Topic.CreateTopicAsync(new Topic { Name = "Ecology" });
       await repositoryManager.Topic.CreateTopicAsync(new Topic { Name = "Culture and Arts" });
       await repositoryManager.Topic.CreateTopicAsync(new Topic { Name = "Economics" });
       await repositoryManager.Topic.CreateTopicAsync(new Topic { Name = "Social Issues" });
       await repositoryManager.Topic.CreateTopicAsync(new Topic { Name = "Travel" }); 
       await repositoryManager.Topic.CreateTopicAsync(new Topic { Name = "Personal Development" });
       await repositoryManager.Topic.CreateTopicAsync(new Topic { Name = "Work and Career" });
       await repositoryManager.Topic.CreateTopicAsync(new Topic { Name = "Consumer Habits" });
       await repositoryManager.Topic.CreateTopicAsync(new Topic { Name = "Sports" });
       await repositoryManager.SaveAsync();
    }
    
    private async Task InitialiseTags()
    {
        await repositoryManager.Tag.CreateTagAsync(new Tag { Name = "Titanic" });
        await repositoryManager.Tag.CreateTagAsync(new Tag { Name = "Football" });
        await repositoryManager.Tag.CreateTagAsync(new Tag { Name = "USA" });
        await repositoryManager.Tag.CreateTagAsync(new Tag { Name = "Physics" });
        await repositoryManager.Tag.CreateTagAsync(new Tag { Name = "Maths" });
        await repositoryManager.Tag.CreateTagAsync(new Tag { Name = "Laptop" });
        await repositoryManager.Tag.CreateTagAsync(new Tag { Name = "Wildfires" });
        await repositoryManager.Tag.CreateTagAsync(new Tag { Name = "Job" });
        await repositoryManager.SaveAsync();
    }
}