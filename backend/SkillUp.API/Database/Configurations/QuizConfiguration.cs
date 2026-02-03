using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillUp.API.Domain;

namespace SkillUp.API.Database.Configurations;

public class QuizConfiguration: IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quizzes");
        builder.HasKey(q => q.Id);
        
        builder.Property(q => q.Title).IsRequired().HasMaxLength(200);
    }
}