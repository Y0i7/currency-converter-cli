using CurrencyConverter.Cli.Resources;
using CurrencyConverter.Cli.Services;

namespace CurrencyConverter.Cli.Utils
{
    public static class CliHelper
    {
        public static async Task ReplAsync(CurrencyService service)
        {
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var cmd = parts[0].ToLowerInvariant();

                if (HandleExit(cmd)) break;

                if (await HandleSpecialCommands(cmd, parts)) continue;

                await HandleCommandAsync(cmd, parts, service);
            }
        }

        private static bool HandleExit(string cmd)
        {
            return cmd is "exit" or "quit";
        }

        private static async Task<bool> HandleSpecialCommands(string cmd, string[] parts)
        {
            switch (cmd)
            {
                case "help":
                    ShowHelp();
                    return true;
                case "lang" when parts.Length >= 2:
                    ChangeLanguage(parts[1]);
                    return true;
                default:
                    return false;
            }
        }

        private static async Task HandleCommandAsync(string cmd, string[] parts, CurrencyService service)
        {
            try
            {
                switch (cmd)
                {
                    case "convert":
                        await HandleConvert(parts, service);
                        break;

                    case "list":
                        await HandleList(service);
                        break;

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

        private static async Task HandleConvert(string[] parts, CurrencyService service)
        {
            if (parts.Length != 4)
            {
                Console.WriteLine(Messages.ConvertUsage);
                return;
            }

            var from = parts[1].ToUpperInvariant();
            var to = parts[2].ToUpperInvariant();

            if (!decimal.TryParse(parts[3], out var amount))
            {
                Console.WriteLine(Messages.InvalidAmount);
                return;
            }

            var result = await service.ConvertAsync(from, to, amount);

            Console.WriteLine($"{amount:C2} {from} = {result.ConvertedAmount:C2} {to} (rate {result.Rate:C2})");
        }

        private static async Task HandleList(CurrencyService service)
        {
            var codes = await service.GetSupportedCurrenciesAsync();

            Console.WriteLine(Messages.SupportedCurrencies);
            Console.WriteLine(string.Join(", ", codes));
        }

        private static void ShowHelp()
        {
            Console.WriteLine(Messages.HelpHeader);
            Console.WriteLine(Messages.ConvertUsage);
            Console.WriteLine(Messages.ListUsage);
        }

        private static void ChangeLanguage(string lang)
        {
            try
            {
                LanguageService.SetLanguage(lang);
                Console.WriteLine($"Language changed to: {lang}");
            }
            catch
            {
                Console.WriteLine("Invalid language");
            }
        }
    }
}