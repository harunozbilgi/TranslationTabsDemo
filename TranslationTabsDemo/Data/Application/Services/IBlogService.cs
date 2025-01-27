using TranslationTabsDemo.Data.Application.DTOs;
using TranslationTabsDemo.Data.Domain.Result;

namespace TranslationTabsDemo.Data.Application.Services;

public interface IBlogService
{
    Task<Result<List<BlogDto>>> GetAllAsync();
    Task<Result<UpdateBlogDto>> GetByIdAsync(Guid id);
    Task<Result<CreateBlogDto>> AddAsync(CreateBlogDto dto);
    Task<Result<UpdateBlogDto>> UpdateAsync(UpdateBlogDto dto);
    Task<Result<bool>> DeleteAsync(Guid id);
}