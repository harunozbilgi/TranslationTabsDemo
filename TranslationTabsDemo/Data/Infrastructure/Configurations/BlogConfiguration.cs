using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TranslationTabsDemo.Data.Domain.Entities;
using TranslationTabsDemo.Data.Domain.Entities.Translations;

namespace TranslationTabsDemo.Data.Infrastructure.Configurations;

public class BlogConfiguration : BaseConfiguration<Blog>
{
    public override void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.Property(e => e.SlugUrl).HasMaxLength(350);

        base.Configure(builder);
    }
}

public class BlogTranslationConfiguration : BaseConfiguration<BlogTranslation>
{
    public override void Configure(EntityTypeBuilder<BlogTranslation> builder)
    {
        builder.Property(e => e.LanguageCode).HasMaxLength(10);
        builder.Property(e => e.Title).HasMaxLength(250);
        builder.Property(e => e.Content).HasColumnType("text");

        builder
            .HasOne(e => e.Blog)
            .WithMany(x => x.Translations)
            .HasForeignKey(e => e.BlogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}