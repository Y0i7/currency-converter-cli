using System.Globalization;
using CurrencyConverter.Cli.Helpers;
using CurrencyConverter.Cli.Resources;
using Microsoft.Extensions.Configuration;
using CurrencyConverter.Cli.Services;
using CurrencyConverter.Cli.Utils;

namespace CurrencyConverter.Cli
{
    public static class Program
    {
        private const string JsonConfigFile = "appsettings.json";
        private const int MaxConvertLength = 4;

        private static async Task<int> Main(string[] args)
        {
            var startIndex = 0;
            
            if (args.Length >= 2 && args[0] == "--lang")
            {
                var language = args[1];
                LanguageService.SetLanguage(language);
                startIndex = 2;
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(JsonConfigFile, optional: false, reloadOnChange: true)
                .Build();

            var baseUrl = config["ApiSettings:BaseUrl"]
                ?? throw new InvalidOperationException("BaseUrl not configured");

            using var http = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            IExchangeClient client = new FrankfurterClient(http);
            var currencyService = new CurrencyService(client);
            
            if (args.Length == 0 || args.Length <= startIndex)
            {
                Console.WriteLine($"{Messages.ApplicationName}{Messages.ApplicationHelpMessage}");
                await CliHelper.ReplAsync(currencyService);
                return 0;
            }

            var cmd = args[startIndex].ToLowerInvariant();

            try
            {
                switch (cmd)
                {
                    case "convert" when args.Length - startIndex == MaxConvertLength:
                    {
                        var from = args[startIndex + 1].ToUpperInvariant();
                        var to = args[startIndex + 2].ToUpperInvariant();

                        if (!decimal.TryParse(args[startIndex + 3], out var amount))
                        {
                            Console.WriteLine(Messages.InvalidAmount);
                            return 1;
                        }

                        var result = await currencyService.ConvertAsync(from, to, amount);

                        var fromFormatted = MoneyFormatter.Format(amount, from, CultureInfo.CurrentCulture);
                        var toFormatted = MoneyFormatter.Format(result.ConvertedAmount, to, CultureInfo.CurrentCulture);

                        Console.WriteLine(
                            Messages.ConversionResult,
                            fromFormatted,
                            toFormatted,
                            result.Rate.ToString("N4")
                        );
                        break;
                    }

                    case "list":
                    {
                        var codes = await client.GetSupportedCurrenciesAsync();
                        Console.WriteLine(Messages.SupportedCurrencies);
                        Console.WriteLine(string.Join(", ", codes));
                        break;
                    }

                    case "help":
                    {
                        Console.WriteLine(Messages.HelpHeader);
                        Console.WriteLine(Messages.ConvertUsage);
                        Console.WriteLine(Messages.ListUsage);
                        Console.WriteLine(Messages.LanguagesUsage);
                        Console.WriteLine(Messages.LangUsage);
                        Console.WriteLine(Messages.NoArgs);
                        break;
                    }

                    default:
                        Console.WriteLine(Messages.NoRecognizedCommand);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(Messages.ErrorMessage + ex.Message);
                return 1;
            }

            return 0;
        }
    }
}