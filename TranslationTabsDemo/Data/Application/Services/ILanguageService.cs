using TranslationTabsDemo.Data.Application.DTOs;
using TranslationTabsDemo.Data.Domain.Result;

namespace TranslationTabsDemo.Data.Application.Services;

public interface ILanguageService
{
    Task<Result<List<LanguageDto>>> GetAllAsync();
    Task<Result<UpdateLanguageDto>> GetByIdAsync(Guid id);
    Task<Result<CreateLanguageDto>> AddAsync(CreateLanguageDto dto);
    Task<Result<UpdateLanguageDto>> UpdateAsync(UpdateLanguageDto dto);
    Task<Result<bool>> DeleteAsync(Guid id);
}