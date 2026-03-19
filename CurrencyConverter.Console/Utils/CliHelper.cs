using System;
using System.Threading.Tasks;
using Services;

namespace Utils
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

                if (cmd == "exit" || cmd == "quit") break;
                if (cmd == "help")
                {
                    Console.WriteLine("Commands: convert <FROM> <TO> <AMOUNT>, list, exit");
                    continue;
                }

                try
                {
                    if (cmd == "convert" && parts.Length == 4)
                    {
                        string from = parts[1].ToUpperInvariant();
                        string to = parts[2].ToUpperInvariant();
                        if (!decimal.TryParse(parts[3], out var amount))
                        {
                            Console.WriteLine("Amount inválido.");
                            continue;
                        }

                        var res = await service.ConvertAsync(from, to, amount);
                        Console.WriteLine($"{amount} {from} = {res.ConvertedAmount} {to} (rate {res.Rate})");
                    }
                    else if (cmd == "list")
                    {
                        var codes = await service._client.GetSupportedCurrenciesAsync();
                        Console.WriteLine(string.Join(", ", codes));
                    }
                    else
                    {
                        Console.WriteLine("Comando no reconocido. Escribe 'help'.");
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