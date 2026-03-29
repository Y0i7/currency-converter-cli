
namespace Services
{
    public record ConvertionResult(decimal Rate, decimal ConvertedAmount);
    
    public class CurrencyService
    {
        private const decimal _MinimumRate = 1m;
        private const int _RequireDecimals = 4;
        
        private readonly IExchangeClient _client;

        public CurrencyService(IExchangeClient client)
        {
            _client = client;
        }

        public async Task<ConvertionResult> ConvertAsync(string from, string to, decimal amount)
        {
            if (from == to)
                return new ConvertionResult(_MinimumRate, amount);

            var rate = await _client.GetRateAsync(from, to);
            var converted = decimal.Round(amount * rate, _RequireDecimals);
            return new ConvertionResult(rate, converted);
        }

        public async Task<IEnumerable<string>> GetSupportedCurrenciesAsync()
        {
            return await _client.GetSupportedCurrenciesAsync();
        }
        
    }
}

