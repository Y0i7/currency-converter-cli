namespace CurrencyConverter.Cli.Utils
{
    public class HttpClientProvider
    {
        private static HttpClient? _httpClient;

        public static HttpClient GetHttpClient(string baseUrl)
        {
            if(_httpClient is not null)
                return _httpClient;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            
            return _httpClient;
        }
    }
}

