using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillUp.API.Domain;

namespace SkillUp.API.Database.Configurations;

public class QuestionConfiguration: IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");
        builder.HasKey(q => q.Id);
        
        builder.Property(q => q.Text).IsRequired().HasMaxLength(255);
        builder.Property(q => q.Type).HasConversion<string>().IsRequired();

        builder.HasOne(q => q.Quiz)
            .WithMany(q => q.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}