namespace TranslationTabsDemo.Data.Application.DTOs;

public record LanguageDto(
    Guid Id,
    string? Name,
    string? Code,
    bool IsDefault
);

public record CreateLanguageDto(
    string? Name,
    string? Code,
    bool IsDefault
);

public record UpdateLanguageDto(
    Guid Id,
    string? Name,
    string? Code,
    bool IsDefault
);