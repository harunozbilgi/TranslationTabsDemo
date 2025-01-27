using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TranslationTabsDemo.Data.Application.DTOs;
using TranslationTabsDemo.Data.Application.Services;
using TranslationTabsDemo.Models;

namespace TranslationTabsDemo.Controllers;

public class HomeController(ILanguageService languageService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var languages = await languageService.GetAllAsync();
        return View(languages.Data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLanguageDto language)
    {
        if (!ModelState.IsValid)
            return View(language);
        await languageService.AddAsync(language);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var language = await languageService.GetByIdAsync(id);
        return View(language.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateLanguageDto language)
    {
        if (!ModelState.IsValid)
            return View(language);
        await languageService.UpdateAsync(language);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        await languageService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}