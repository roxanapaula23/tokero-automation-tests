using System.Text.Json.Serialization;

namespace tokero_automation_tests.tokero_automation_tests.Models;

public class Properties
{
    [JsonPropertyName("browser")] public string Browser { get; set; }

    [JsonPropertyName("baseUrl")] public string BaseUrl { get; set; }

    [JsonPropertyName("headless")] public bool Headless { get; set; }

    [JsonPropertyName("timeout")] public int Timeout { get; set; }
}