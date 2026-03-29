using System.Text.Json.Serialization;

namespace Models
{
    public class FrankfurterResponse
    {
        [JsonPropertyName("base")]
        public required string Base { get; init; }

        [JsonPropertyName("date")]
        public required string Date { get; init; }

        [JsonPropertyName("rates")]
        public required Dictionary<string, decimal> Rates { get; init; }
    }
}