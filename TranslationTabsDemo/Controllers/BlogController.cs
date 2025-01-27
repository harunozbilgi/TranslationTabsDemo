using Microsoft.AspNetCore.Mvc;
using TranslationTabsDemo.Data.Application.DTOs;
using TranslationTabsDemo.Data.Application.Services;

namespace TranslationTabsDemo.Controllers;

public class BlogController(IBlogService blogService) : Controller
{
  
    public async Task<IActionResult> Index()
    {
        var blogs = await blogService.GetAllAsync();
        return View(blogs.Data);
    }
    
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBlogDto blog)
    {
        if (!ModelState.IsValid)
            return View(blog);
        await blogService.AddAsync(blog);
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Edit(Guid id)
    {
        var blog = await blogService.GetByIdAsync(id);
        return View(blog.Data);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateBlogDto blog)
    {
        if (!ModelState.IsValid)
            return View(blog);
        await blogService.UpdateAsync(blog);
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Delete(Guid id)
    {
        await blogService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
    
}