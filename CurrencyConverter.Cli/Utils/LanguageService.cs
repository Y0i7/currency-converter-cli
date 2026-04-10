using System.Globalization;
using CurrencyConverter.Cli.Resources;

namespace CurrencyConverter.Cli.Utils;

public static class LanguageService
{
    private static readonly Dictionary<string, CultureInfo> SupportedCultures = new(StringComparer.OrdinalIgnoreCase)
    {
        ["es-CO"] = CultureInfo.GetCultureInfo("es-CO"),
        ["es"]    = CultureInfo.GetCultureInfo("es"),
        ["es-ES"] = CultureInfo.GetCultureInfo("es-ES"),
        ["en-US"] = CultureInfo.GetCultureInfo("en-US"),
        ["fr-FR"] = CultureInfo.GetCultureInfo("fr-FR"),
        ["zh-CN"] = CultureInfo.GetCultureInfo("zh-CN"),
    };

    public static IReadOnlyDictionary<string, CultureInfo> Languages => SupportedCultures;

    public static void SetLanguage(string lang)
    {
        if (!SupportedCultures.TryGetValue(lang, out var culture))
            throw new CultureNotFoundException($"{Messages.UnsupportedCultureMessage} {lang}");

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }
}