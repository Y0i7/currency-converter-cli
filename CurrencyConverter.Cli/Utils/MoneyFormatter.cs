using System.Globalization;

namespace CurrencyConverter.Cli.Helpers
{
    public static class MoneyFormatter
    {
        public static string Format(decimal amount, string currencyCode, CultureInfo culture, int decimals = 2)
        {
            var symbol = culture.NumberFormat.CurrencySymbol;
            var value = amount.ToString($"N{decimals}", culture);
            return $"{symbol} {value} {currencyCode}";
        }
    }
}