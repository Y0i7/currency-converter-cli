
namespace Services
{
    public record ConvertionResult(decimal Rate, decimal ConvertedAmount);
    
    public class CurrencyService
    {
        private readonly IExchangeClient _client;

        public CurrencyService(IExchangeClient client)
        {
            _client = client;
        }

        public async Task<ConvertionResult> ConvertAsync(string from, string to, decimal amount)
        {
            if (from == to)
                return new ConvertionResult(1m, amount);

            var rate = await _client.GetRateAsync(from, to);
            var converted = decimal.Round(amount * rate,4);
            return new ConvertionResult(rate, converted);
        }

        public async Task<IEnumerable<string>> GetSupportedCurrenciesAsync()
        {
            return await _client.GetSupportedCurrenciesAsync();
        }
        
    }
}

