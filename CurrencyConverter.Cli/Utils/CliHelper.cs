using System.Globalization;
using CurrencyConverter.Cli.Resources;
using CurrencyConverter.Cli.Services;
using CurrencyConverter.Cli.Helpers;

namespace CurrencyConverter.Cli.Utils
{
    public static class CliHelper
    {
        public static async Task ReplAsync(CurrencyService service)
        {
            while (true)
            {
                Console.Write("> ");
                var commandLine = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(commandLine)) continue;

                var commandParts = commandLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var cmd = commandParts[0].ToLowerInvariant();

                if (HandleExit(cmd)) break;

                if (HandleSpecialCommands(cmd, commandParts)) continue;

                await HandleCommandAsync(cmd, commandParts, service);
            }
        }

        private static bool HandleExit(string cmd)
        {
            return cmd is "exit" or "quit";
        }

        private static bool HandleSpecialCommands(string cmd, string[] commandParts)
        {
            switch (cmd)
            {
                case "help":
                    ShowHelp();
                    return true;

                case "languages":
                    foreach (var language in LanguageService.Languages)
                        Console.WriteLine($"{language.Key,-6} -> lang {language.Key}");
                    return true;

                case "lang" when commandParts.Length >= 2:
                    ChangeLanguage(commandParts[1]);
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

            var fromFormatted = MoneyFormatter.Format(amount, from, CultureInfo.CurrentCulture);
            var toFormatted = MoneyFormatter.Format(result.ConvertedAmount, to, CultureInfo.CurrentCulture);

            Console.WriteLine(
                $"{fromFormatted} = {toFormatted} (rate {result.Rate.ToString("N4")})"
            );
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
            Console.WriteLine(Messages.LanguagesUsage);
            Console.WriteLine(Messages.LangUsage);
            Console.WriteLine(Messages.NoArgs);
        }

        private static void ChangeLanguage(string lang)
        {
            try
            {
                LanguageService.SetLanguage(lang);
                Console.WriteLine($"{Messages.ChangedLanguage} {lang}");
            }
            catch
            {
                Console.WriteLine($"{Messages.UnsupportedCultureMessage} {lang}");
            }
        }
    }
}