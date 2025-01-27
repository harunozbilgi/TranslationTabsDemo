namespace TranslationTabsDemo.Data.Application.Helpers;

public static class SlugUrlHelper
{
    public static string GenerateSlug(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return string.Empty;
        }

        var strReturn = title.Trim();
        strReturn = strReturn.Replace("ğ", "g");
        strReturn = strReturn.Replace("Ğ", "G");
        strReturn = strReturn.Replace("ü", "u");
        strReturn = strReturn.Replace("Ü", "U");
        strReturn = strReturn.Replace("ş", "s");
        strReturn = strReturn.Replace("Ş", "S");
        strReturn = strReturn.Replace("ı", "i");
        strReturn = strReturn.Replace("İ", "I");
        strReturn = strReturn.Replace("ö", "o");
        strReturn = strReturn.Replace("Ö", "O");
        strReturn = strReturn.Replace("ç", "c");
        strReturn = strReturn.Replace("Ç", "C");
        strReturn = strReturn.Replace("-", "+");
        strReturn = strReturn.Replace(" ", "+");
        strReturn = strReturn.Replace("ə", "e");
        strReturn = strReturn.Replace("Ə", "E");

        strReturn = strReturn.Trim();
        strReturn = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9+]").Replace(strReturn, "");
        strReturn = strReturn.Trim();
        strReturn = strReturn.Replace("+", "-");
        return strReturn;
    }
}