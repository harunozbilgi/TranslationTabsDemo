using TranslationTabsDemo.Data.Domain.Common;

namespace TranslationTabsDemo.Data.Domain.Entities;

public class Language : BaseEntity
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public bool IsDefault { get; set; }
}