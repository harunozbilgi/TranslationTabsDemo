using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TranslationTabsDemo.Data.Domain.Common;

namespace TranslationTabsDemo.Data.Infrastructure.Configurations;

public class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.CreatedAt).ValueGeneratedOnAdd();
        builder.Property(e => e.UpdatedAt).ValueGeneratedOnAdd();
    }
}
