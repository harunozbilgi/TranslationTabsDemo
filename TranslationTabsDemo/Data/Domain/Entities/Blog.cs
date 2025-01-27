using TranslationTabsDemo.Data.Domain.Common;
using TranslationTabsDemo.Data.Domain.Entities.Translations;

namespace TranslationTabsDemo.Data.Domain.Entities;

public class Blog : BaseEntity
{
    
    public DateTime PublishedAt { get; set; }
    public string? SlugUrl { get; set; }
    public ICollection<BlogTranslation> Translations { get; set; } = new List<BlogTranslation>();
}