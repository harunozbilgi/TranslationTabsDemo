using TranslationTabsDemo.Data.Application.DTOs;
using TranslationTabsDemo.Data.Application.Helpers;
using TranslationTabsDemo.Data.Domain.Entities;
using TranslationTabsDemo.Data.Domain.Entities.Translations;

namespace TranslationTabsDemo.Data.Application.Mapping;

public static class BlogMapper
{
    public static BlogDto ToBlogs(this Blog blog)
    {
        var title = blog.Translations.First(x => x.LanguageCode == "en").Title;

        return new BlogDto(blog.Id, title, blog.PublishedAt);
    }

    public static UpdateBlogDto UpdateDto(this Blog blog)
    {
        return new UpdateBlogDto(blog.Id, blog.SlugUrl, blog.PublishedAt,
            blog.Translations.Select(x => x.ToBlogTranslations()).ToList());
    }

    public static Blog ToCreate(this CreateBlogDto blog)
        => new()
        {
            SlugUrl = SlugUrlHelper.GenerateSlug(blog.Translations.First(x => x.LanguageCode == "en").Title),
            PublishedAt = blog.PublishedDate,
            Translations = blog.Translations.Select(x => new BlogTranslation
            {
                LanguageCode = x.LanguageCode,
                Title = x.Title,
                Content = x.Content
            }).ToList()
        };

    public static Blog ToUpdate(this UpdateBlogDto blog)
        => new()
        {
            Id = blog.Id,
            SlugUrl = SlugUrlHelper.GenerateSlug(blog.Translations.First(x => x.LanguageCode == "en").Title),
            PublishedAt = blog.PublishedDate,
            Translations = blog.Translations.Select(x => new BlogTranslation
            {
                Id = x.Id,
                BlogId = blog.Id,
                LanguageCode = x.LanguageCode,
                Title = x.Title,
                Content = x.Content
            }).ToList()
        };

    private static BlogTranslationDto ToBlogTranslations(this BlogTranslation blogTranslation)
    {
        return new BlogTranslationDto
        {
            Id = blogTranslation.Id,
            BlogId = blogTranslation.BlogId,
            LanguageCode = blogTranslation.LanguageCode,
            Title = blogTranslation.Title,
            Content = blogTranslation.Content
        };
    }
}