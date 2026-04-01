using System.Globalization;

namespace CurrencyConverter.Cli.Utils
{
    public static class LanguageService
    {
        public static void SetLanguage(string lang)
        {
            var culture = new CultureInfo(lang);

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
    }
}