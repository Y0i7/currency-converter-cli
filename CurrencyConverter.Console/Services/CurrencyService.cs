
namespace Services
{
    public record ConvertionResult(decimal Rate, decimal ConvertedRate);
    
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
    }
}

