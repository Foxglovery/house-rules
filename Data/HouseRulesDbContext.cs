using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HouseRules.Models;
using Microsoft.AspNetCore.Identity;

namespace HouseRules.Data;
public class HouseRulesDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _configuration;
    
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Chore> Chores { get; set; }
    public DbSet<ChoreAssignment> ChoreAssignments { get; set; }
    public DbSet<ChoreCompletion> ChoreCompletions { get; set; }


    public HouseRulesDbContext(DbContextOptions<HouseRulesDbContext> context, IConfiguration config) : base(context)
    {
        _configuration = config;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //rolls available
        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
            Name = "Admin",
            NormalizedName = "admin"
        });
        //User info with login credentials
        modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
        {
            Id = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
            UserName = "Administrator",
            Email = "admina@strator.comx",
            PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, _configuration["AdminPassword"])
        });
        //join table of user roles with users
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
            UserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f"
        });
        modelBuilder.Entity<UserProfile>().HasData(new UserProfile
        {
            Id = 1,
            IdentityUserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
            FirstName = "Admina",
            LastName = "Strator",
            Address = "101 Main Street",
            UserName = "HootyPoots"
        });
        modelBuilder.Entity<Chore>().HasData(
            new Chore
        {
            Id = 1,
            Name = "Kitchen Floors",
            Difficulty = 2,
            ChoreFrequencyDays = 4
        },
        new Chore
        {
            Id = 2,
            Name = "Clean Bathroom",
            Difficulty = 4,
            ChoreFrequencyDays = 4
        },
        new Chore
        {
            Id = 3,
            Name = "Mow Lawn",
            Difficulty = 5,
            ChoreFrequencyDays = 14
        },
        new Chore
        {
            Id = 4,
            Name = "Clean Fridge",
            Difficulty = 3,
            ChoreFrequencyDays = 14
        },
        new Chore
        {
            Id = 5,
            Name = "Tidy Bedroom",
            Difficulty = 2,
            ChoreFrequencyDays = 2
        });

        modelBuilder.Entity<ChoreAssignment>().HasData(
            new ChoreAssignment
            {
                Id = 1,
                UserProfileId = 1,
                ChoreId = 4
            },
            new ChoreAssignment
            {
                Id = 2,
                UserProfileId = 1,
                ChoreId = 2
            }
            );
        modelBuilder.Entity<ChoreCompletion>().HasData(
            new ChoreCompletion
            {
                Id = 1,
                UserProfileId = 1,
                ChoreId = 2,
                CompletedOn = new DateTime(2024,01,13)
            }
        );

        
        
    }
}