using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillUp.API.Domain;

namespace SkillUp.API.Database.Configurations;

public class AnswerConfiguration: IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable("Answers");
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Text).IsRequired().HasMaxLength(255);
        builder.Property(a => a.IsCorrect).IsRequired();
        
        builder.HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}