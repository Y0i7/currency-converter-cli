
using Services;
using Utils;

class Program
{ 
    static async Task<int> Main(string[] args)
    {
        using var http = new System.Net.Http.HttpClient
        {
            BaseAddress = new Uri("https://api.frankfurter.dev/v1/")
        };

        IExchangeClient client = new FrankfurterClient(http);
        var currencyService = new CurrencyService(client);

        if (args.Length == 0)
        {
            Console.WriteLine("Currency Converter CLI\\nType 'help' for commands.");
            await CliHelper.ReplAsync(currencyService);
            return 0;
        }
        
        var cmd = args[0].ToLowerInvariant();
        try
        {
            switch (cmd)
            {
                case "convert" when args.Length == 4 :
                    var from = args[1].ToUpperInvariant();
                    var to = args[2].ToUpperInvariant();
                    if (!decimal.TryParse(args[3], out var amount))
                    {
                        Console.WriteLine("Invalid amount specified.");
                        return 1;
                    }

                    var result = await currencyService.ConvertAsync(from, to, amount);
                    Console.WriteLine($"{amount} {from} = {result.ConvertedAmount} {to} (rate {result.Rate})");
                    break;
                
                case "list":
                    var codes = await client.GetSupportedCurrenciesAsync();
                    Console.WriteLine("Monedas soportadas:");
                    Console.WriteLine(string.Join(", ", codes));
                    break;
                
                case "help":
                default:
                    Console.WriteLine("Comandos:");
                    Console.WriteLine("  convert <FROM> <TO> <AMOUNT>   - convertir monto");
                    Console.WriteLine("  list                           - listar monedas soportadas");
                    Console.WriteLine("  (sin args)                     - modo interactivo");
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