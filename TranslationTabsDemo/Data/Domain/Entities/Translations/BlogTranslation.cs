using TranslationTabsDemo.Data.Domain.Common;

namespace TranslationTabsDemo.Data.Domain.Entities.Translations;

public class BlogTranslation : BaseEntity
{
    public Guid BlogId { get; set; }
    public string? LanguageCode { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public Blog Blog { get; set; } = null!;
}