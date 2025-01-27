using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TranslationTabsDemo.Data.Application.Services;
using TranslationTabsDemo.Data.Domain.Attributes;

namespace TranslationTabsDemo.Helpers;

[HtmlTargetElement("translation-tabs")]
public class TranslationTabsTagHelper(ILanguageService languageService) : TagHelper
{
    public IEnumerable<object?>? Values { get; set; } = new List<object>();
    public string? CreateModel { get; set; }
    public required string NamePrefix { get; set; }
    public required string TabId { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var languages = await languageService.GetAllAsync();
        var tabBuilder = new StringBuilder();
        var translationsList = new List<object>();
        tabBuilder.Append($"<ul class='nav nav-pills mb-3' id='{TabId}-tab' role='tablist'>");

        var count = 0;
        foreach (var language in languages.Data)
        {
            var langCode = language.Code;
            var langName = language.Name;
            var activeClass = count == 0 ? "active" : "";
            var selected = count == 0 ? "true" : "false";

            if (Values != null && !Values.Any())
            {
                if (CreateModel != null)
                {
                    var translationType = FindTypeInAssemblies(CreateModel);

                    if (translationType != null)
                    {
                        var translation = Activator.CreateInstance(translationType);
                        if (translation != null)
                        {
                            var languageCodeProperty = translationType.GetProperty("LanguageCode");
                            if (languageCodeProperty != null && languageCodeProperty.CanWrite)
                            {
                                languageCodeProperty.SetValue(translation, language.Code);
                            }

                            translationsList.Add(translation);
                        }
                    }
                }
            }


            tabBuilder.Append($"""
                                   <li class='nav-item' role='presentation'>
                                       <button class='nav-link {activeClass}' id='{TabId}-lang-tab-{langCode}'
                                               data-bs-toggle='pill' data-bs-target='#{TabId}-lang-{langCode}'
                                               type='button' role='tab' aria-controls='{TabId}-lang-{langCode}'
                                               aria-selected='{selected}'>{langName}</button>
                                   </li>
                               """);

            count++;
        }

        tabBuilder.Append("</ul>");

        var contentBuilder = new StringBuilder();
        contentBuilder.Append($"<div class='tab-content' id='{TabId}-TabContent'>");
        if (Values != null && !Values.Any())
        {
            Values = translationsList;
        }

        count = 0;

        if (Values != null)
        {
            foreach (var property in Values)
            {
                var langCode = property?.GetType().GetProperty("LanguageCode")?.GetValue(property) as string;

                var activeClass = count == 0 ? "active show" : "";

                contentBuilder.Append($"""
                                           <div class='tab-pane fade {activeClass}' id='{TabId}-lang-{langCode}' role='tabpanel' aria-labelledby='{TabId}-lang-tab-{langCode}'>
                                               {GetFieldsHtml(property, langCode, count)}
                                           </div>
                                       """);


                count++;
            }
        }

        contentBuilder.Append("</div>");

        output.TagName = "div";
        output.Attributes.SetAttribute("class", "multi");
        output.Content.SetHtmlContent(tabBuilder.ToString() + contentBuilder.ToString());
        output.TagMode = TagMode.StartTagAndEndTag;
    }

    private string GetFieldsHtml(object? translation, string? language, int count)
    {
        var stringBuilder = new StringBuilder();

        var properties = translation?.GetType().GetProperties();
        foreach (var property in properties!)
        {
            var attribute = property.GetCustomAttribute<TextTypeAttribute>();
            if (attribute == null)
                continue;

            var labelText = attribute.LabelName;
            var value = property.GetValue(translation) ?? string.Empty;
            var placeholder = attribute.Placeholder;
            var cssClass = attribute.CssClass;
            var requiredAttribute = attribute.Required ? "required" : "";

            if (value is Guid guidValue && guidValue == Guid.Empty)
            {
                continue;
            }

            stringBuilder.Append("<div class='mb-3'>");

            if (!string.IsNullOrEmpty(labelText))
            {
                stringBuilder.Append(
                    $"<label class='form-label' for='{property.Name}'>{labelText} - {language}</label>");
            }


            switch (attribute.InputType)
            {
                case "text":
                    stringBuilder.Append(
                        $"<input type='text' {requiredAttribute} class='form-control {cssClass}' id='{property.Name}' name='{NamePrefix}[{count}].{property.Name}' value='{value}' placeholder='{placeholder}' />");
                    break;
                case "textarea":
                    stringBuilder.Append(
                        $"<textarea rows='5' cols='5' {requiredAttribute} class='form-control {cssClass}' id='{property.Name}' name='{NamePrefix}[{count}].{property.Name}' placeholder='{placeholder}'>{value}</textarea>");
                    break;

                case "hidden":
                    stringBuilder.Append(
                        $"<input type='hidden' id='{property.Name}' name='{NamePrefix}[{count}].{property.Name}' value='{value}' />");
                    break;
            }

            stringBuilder.Append("</div>");
        }

        return stringBuilder.ToString();
    }

    private static Type? FindTypeInAssemblies(string modelName)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .Select(assembly => assembly.GetTypes().FirstOrDefault(t => t.Name == modelName || t.FullName == modelName))
            .OfType<Type>().FirstOrDefault();
    }
}