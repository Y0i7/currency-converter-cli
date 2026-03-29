using CurrencyConverter.Cli.Resources;
using Microsoft.Extensions.Configuration;
using Services;
using Utils;

namespace CurrencyConverter.Cli
{
    static class Program
    {
        private const string _JsonConfigFile = "appsettings.json";
        private const int _MaxConvertLenght = 4;

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
                .AddJsonFile(_JsonConfigFile, optional: false, reloadOnChange: true)
                .Build();

            var baseUrl = config["ApiSettings:BaseUrl"]
                ?? throw new Exception("BaseUrl not configured");

            using var http = new System.Net.Http.HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            IExchangeClient client = new FrankfurterClient(http);
            var currencyService = new CurrencyService(client);
            
            if (args.Length == 0 || args.Length <= startIndex)
            {
                Console.WriteLine($"{Messages.ApplicationName}\n\n{Messages.ApplicationHelpMessage}");
                await CliHelper.ReplAsync(currencyService);
                return 0;
            }

            var cmd = args[startIndex].ToLowerInvariant();

            try
            {
                switch (cmd)
                {
                    case "convert" when args.Length - startIndex == _MaxConvertLenght:
                    {
                        var from = args[startIndex + 1].ToUpperInvariant();
                        var to = args[startIndex + 2].ToUpperInvariant();

                        if (!decimal.TryParse(args[startIndex + 3], out var amount))
                        {
                            Console.WriteLine(Messages.InvalidAmount);
                            return 1;
                        }

                        var result = await currencyService.ConvertAsync(from, to, amount);

                        Console.WriteLine($"{amount:C} {from} = {result.ConvertedAmount:C2} {to} (rate {result.Rate:C2})");
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
                Console.WriteLine($"Error: {ex.Message}");
                return 1;
            }

            return 0;
        }
    }
}