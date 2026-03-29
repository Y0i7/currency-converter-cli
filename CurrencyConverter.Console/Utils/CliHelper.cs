using CurrencyConverter.Cli.Resources;
using Services;

namespace Utils
{
    public static class CliHelper
    {
        private const int _MaxConvertLenght = 4;

        public static async Task ReplAsync(CurrencyService service)
        {
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var cmd = parts[0].ToLowerInvariant();
                
                if (cmd is "exit" or "quit") break;
                
                if (cmd == "lang" && parts.Length >= 2)
                {
                    var lang = parts[1];

                    try
                    {
                        LanguageService.SetLanguage(lang);
                        Console.WriteLine($"Language changed to: {lang}");
                    }
                    catch
                    {
                        Console.WriteLine("Invalid language");
                    }

                    continue;
                }

                if (cmd == "help")
                {
                    Console.WriteLine(Messages.HelpHeader);
                    Console.WriteLine(Messages.ConvertUsage);
                    Console.WriteLine(Messages.ListUsage);
                    continue;
                }

                try
                {
                    switch (cmd)
                    {
                        case "convert" when parts.Length == _MaxConvertLenght:
                        {
                            var from = parts[1].ToUpperInvariant();
                            var to = parts[2].ToUpperInvariant();

                            if (!decimal.TryParse(parts[3], out var amount))
                            {
                                Console.WriteLine(Messages.InvalidAmount);
                                continue;
                            }

                            var response = await service.ConvertAsync(from, to, amount);

                            Console.WriteLine($"{amount:C2} {from} = {response.ConvertedAmount:C2} {to} (rate {response.Rate:C2})");
                            break;
                        }

                        case "list":
                        {
                            var codes = await service.GetSupportedCurrenciesAsync();
                            Console.WriteLine(Messages.SupportedCurrencies);
                            Console.WriteLine(string.Join(", ", codes));
                            break;
                        }

                        default:
                            Console.WriteLine(Messages.NoRecognizedCommand);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}