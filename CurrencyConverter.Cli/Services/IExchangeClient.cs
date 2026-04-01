
namespace CurrencyConverter.Cli.Services
{
    public interface IExchangeClient
    {
        Task<decimal> GetRateAsync(string from, string to);
        Task<IEnumerable<string>> GetSupportedCurrenciesAsync();
    }
}