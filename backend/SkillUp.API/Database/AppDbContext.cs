using Microsoft.EntityFrameworkCore;
using SkillUp.API.Domain;

namespace SkillUp.API.Database;

public class AppDbContext: DbContext
{
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}