namespace TranslationTabsDemo.Data.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class TextTypeAttribute(string inputType) : Attribute
{
    public string InputType { get; set; } = inputType;
    public string? LabelName { get; set; }
    public string? Placeholder { get; set; } = string.Empty;
    public bool Required { get; set; } = false;
    public string? CssClass { get; set; }
}