using TranslationTabsDemo.Data.Application.DTOs;
using TranslationTabsDemo.Data.Domain.Entities;

namespace TranslationTabsDemo.Data.Application.Mapping;

public static class LanguageMapper
{
    public static LanguageDto ToLanguages(this Language language) =>
        new(language.Id, language.Name, language.Code, language.IsDefault);


    public static UpdateLanguageDto UpdateDto(this Language language) =>
        new(language.Id, language.Name, language.Code, language.IsDefault);

    public static Language ToCreate(this CreateLanguageDto dto) =>
        new()
        {
            Name = dto.Name,
            Code = dto.Code,
            IsDefault = dto.IsDefault
        };

    public static Language ToUpdate(this UpdateLanguageDto dto) =>
        new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Code = dto.Code,
            IsDefault = dto.IsDefault
        };
}