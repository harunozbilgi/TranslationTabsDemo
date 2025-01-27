using TranslationTabsDemo.Data.Domain.Attributes;

namespace TranslationTabsDemo.Data.Application.DTOs;

public record BlogDto(
    Guid Id,
    string? Title,
    DateTime PublishedDate
);

public record CreateBlogDto(
    string? SlugUrl,
    DateTime PublishedDate,
    List<BlogTranslationDto> Translations);

public record UpdateBlogDto(
    Guid Id,
    string? SlugUrl,
    DateTime PublishedDate,
    List<BlogTranslationDto> Translations);

public record BlogTranslationDto
{
    [TextType("hidden")] public Guid Id { get; set; }
    [TextType("hidden")] public Guid BlogId { get; set; }
    [TextType("hidden")] public string? LanguageCode { get; set; }

    [TextType("text", LabelName = "Title")]
    public string? Title { get; set; }

    [TextType("textarea", LabelName = "Description", CssClass = "summernote")]
    public string? Content { get; set; }
}