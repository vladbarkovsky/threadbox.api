using System.Text.Json.Serialization;

namespace ThreadboxApi.Application.Csp.Models
{
    public class CspReport
    {
        [JsonPropertyName("document-uri")]
        public string DocumentUri { get; init; }

        [JsonPropertyName("referrer")]
        public string Referrer { get; init; }

        [JsonPropertyName("violated-directive")]
        public string ViolatedDirective { get; init; }

        [JsonPropertyName("effective-directive")]
        public string EffectiveDirective { get; init; }

        [JsonPropertyName("original-policy")]
        public string OriginalPolicy { get; init; }

        [JsonPropertyName("blocked-uri")]
        public string BlockedUri { get; init; }

        [JsonPropertyName("status-code")]
        public int StatusCode { get; init; }
    }
}