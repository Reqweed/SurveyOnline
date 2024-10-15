using SurveyOnline.DAL.Entities.Enums;
using SurveyOnline.DAL.Entities.Models;

namespace SurveyOnline.DAL.Initializers;

public static class DataInitializer
{
    public static IEnumerable<Role> Roles { get; set; }
    public static IEnumerable<Tag> Tags { get; set; }
    public static IEnumerable<Topic> Topics { get; set; }

    static DataInitializer()
    {
        GetRoles();
        GetTags();
        GetTopics();
    }

    private static void GetRoles()
    {
        Roles = new List<Role>
        {
            new() { Id = Guid.NewGuid(), Name = RoleType.Admin.ToString(), NormalizedName = RoleType.Admin.ToString().ToUpper() },
            new() { Id = Guid.NewGuid(), Name = RoleType.User.ToString(), NormalizedName = RoleType.User.ToString().ToUpper() }
        };
    }

    private static void GetTags()
    {
        Tags = new List<Tag>
        {
            new() { Id = Guid.NewGuid(), Name = "Education" },
            new() { Id = Guid.NewGuid(), Name = "Films" }
        };
    }

    private static void GetTopics()
    {
        Topics = new List<Topic>
        {
            new() { Id = Guid.NewGuid(), Name = "Linear equations" },
            new() { Id = Guid.NewGuid(), Name = "Titanic" }
        };
    }
}