
using System.Text.Json;
using Models;

namespace  Services
{
    public class FrankfurterClient : IExchangeClient
    {
        private readonly HttpClient _http;

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public FrankfurterClient(HttpClient httpClient)
        {
            _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        
        public async Task<decimal> GetRateAsync(string from, string to)
        {
            var url = $"latest?base={from}&symbols={to}";
            var response = await _http.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<FrankfurterResponse>(json, _jsonOptions);

            if (data?.Rates == null || !data.Rates.ContainsKey(to))
                throw new Exception("Invalid rate");
            return data.Rates[to];
        }

        public async Task<IEnumerable<string>> GetSupportedCurrenciesAsync()
        {
            var resp = await _http.GetAsync("currencies");
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();
            var map = JsonSerializer.Deserialize<Dictionary<string,string>>(json, _jsonOptions);
            return (IEnumerable<string>)map?.Keys.OrderBy(k => k) ?? [];
        }
    }
}
