using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyOnline.DAL.Entities.Models;
using SurveyOnline.DAL.Initializers;

namespace SurveyOnline.DAL.Contexts;

public class PostgresDbContext
    (DbContextOptions<PostgresDbContext> options) : IdentityDbContext<
        User, Role, Guid,
        IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options)
{
    public DbSet<Answer> Answers { get; set; }
    public DbSet<CompletedSurvey> CompletedSurveys { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(userTable =>
        {
            userTable
                .HasMany(u => u.AccessibleSurveys)
                .WithMany(s => s.AccessibleUsers);
            userTable
                .HasMany(u => u.OwnSurveys)
                .WithOne(s => s.Creator);
            userTable
                .HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId);
        });

        modelBuilder.Entity<Role>()
            .HasMany(e => e.UserRoles)
            .WithOne(e => e.Role)
            .HasForeignKey(ur => ur.RoleId);
        
        modelBuilder.Entity<Survey>()
            .HasMany(s => s.Tags)
            .WithMany(t => t.Surveys);

        modelBuilder.Entity<Role>().HasData(DataInitializer.Roles);
        modelBuilder.Entity<Tag>().HasData(DataInitializer.Tags);
        modelBuilder.Entity<Topic>().HasData(DataInitializer.Topics);
    }
}