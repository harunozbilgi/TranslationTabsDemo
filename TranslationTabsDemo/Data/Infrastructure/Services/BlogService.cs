using Microsoft.EntityFrameworkCore;
using TranslationTabsDemo.Data.Application.DTOs;
using TranslationTabsDemo.Data.Application.Mapping;
using TranslationTabsDemo.Data.Application.Services;
using TranslationTabsDemo.Data.Domain.Entities;
using TranslationTabsDemo.Data.Domain.Repositories;
using TranslationTabsDemo.Data.Domain.Result;

namespace TranslationTabsDemo.Data.Infrastructure.Services;

public class BlogService(IUnitOfWork unitOfWork) : IBlogService
{
    private IBaseRepository<Blog> BlogRepository => unitOfWork.GetRepository<Blog>();

    public async Task<Result<List<BlogDto>>> GetAllAsync()
    {
        var blogs = await BlogRepository
            .GetAllAsync(includes: x => x.Include(t => t.Translations));

        var result = blogs.OrderByDescending(x => x.PublishedAt)
            .Select(x => x.ToBlogs()).ToList();

        return result;
    }

    public async Task<Result<UpdateBlogDto>> GetByIdAsync(Guid id)
    {
        var blog = await BlogRepository.GetFirstOrDefaultAsync(x => x.Id == id,
            includes: x => x.Include(t => t.Translations));

        if (blog == null)
        {
            return Result.Failure<UpdateBlogDto>(new Error("Blog not found"));
        }

        var result = blog.UpdateDto();

        return result;
    }

    public async Task<Result<CreateBlogDto>> AddAsync(CreateBlogDto dto)
    {
        var blog = dto.ToCreate();

        await BlogRepository.AddAsync(blog);

        await unitOfWork.SaveChangesAsync();

        return dto;
    }

    public async Task<Result<UpdateBlogDto>> UpdateAsync(UpdateBlogDto dto)
    {
        var blog = dto.ToUpdate();
        await BlogRepository.UpdateAsync(blog);
        await unitOfWork.SaveChangesAsync();
        return dto;
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        var blog = await BlogRepository.GetByIdAsync(id);

        if (blog == null)
        {
            return Result.Failure<bool>(new Error("Blog not found"));
        }

        await BlogRepository.DeleteAsync(blog);

        return await unitOfWork.SaveChangesAsync() > 0;
    }
}