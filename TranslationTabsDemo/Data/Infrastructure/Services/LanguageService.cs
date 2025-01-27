using TranslationTabsDemo.Data.Application.DTOs;
using TranslationTabsDemo.Data.Application.Mapping;
using TranslationTabsDemo.Data.Application.Services;
using TranslationTabsDemo.Data.Domain.Entities;
using TranslationTabsDemo.Data.Domain.Repositories;
using TranslationTabsDemo.Data.Domain.Result;

namespace TranslationTabsDemo.Data.Infrastructure.Services;

public class LanguageService(IUnitOfWork unitOfWork) : ILanguageService
{
    private IBaseRepository<Language> LanguageRepository 
        => unitOfWork.GetRepository<Language>();

    public async Task<Result<List<LanguageDto>>> GetAllAsync()
    {
        var languages = await LanguageRepository.GetAllAsync();

        var result = languages.OrderBy(x => x.Name)
            .Select(x => x.ToLanguages()).ToList();

        return result;
    }

    public async Task<Result<UpdateLanguageDto>> GetByIdAsync(Guid id)
    {
        var language = await LanguageRepository.GetByIdAsync(id);

        if (language == null)
        {
            return Result.Failure<UpdateLanguageDto>(new Error("Language not found"));
        }

        var result = language.UpdateDto();

        return result;
    }

    public async Task<Result<CreateLanguageDto>> AddAsync(CreateLanguageDto dto)
    {
        var language = dto.ToCreate();

        await LanguageRepository.AddAsync(language);

        await unitOfWork.SaveChangesAsync();

        return dto;
    }

    public async Task<Result<UpdateLanguageDto>> UpdateAsync(UpdateLanguageDto dto)
    {
        var language = dto.ToUpdate();
        await LanguageRepository.UpdateAsync(language);
        await unitOfWork.SaveChangesAsync();
        return dto;
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var language = await LanguageRepository.GetByIdAsync(id);

        if (language == null)
        {
            return Result.Failure<bool>(new Error("Language not found"));
        }

        await LanguageRepository.DeleteAsync(language);

        return await unitOfWork.SaveChangesAsync() > 0;
    }
}