using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TranslationTabsDemo.Data.Domain.Entities;

namespace TranslationTabsDemo.Data.Infrastructure.Configurations;

public class LanguageConfiguration : BaseConfiguration<Language>
{
    public override void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.Property(e => e.Name).HasMaxLength(50);
        builder.Property(e => e.Code).HasMaxLength(5);
        
        base.Configure(builder);
    }
}