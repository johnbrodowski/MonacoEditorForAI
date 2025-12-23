using System.Text.Json.Serialization;

namespace AnthropicMinimal;

/// <summary>
/// Request to create a message
/// </summary>
public class MessageRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = ClaudeModels.Claude45Haiku;

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; } = 1024;

    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; } = new();

    [JsonPropertyName("system")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? System { get; set; }

    [JsonPropertyName("temperature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Temperature { get; set; }

    [JsonPropertyName("stream")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Stream { get; set; }
}

public static class ClaudeModels
{
    public const string Claude45Sonnet = "claude-sonnet-4-5-20250929";
    public const string Claude45Haiku = "claude-haiku-4-5-20251001";
    public const string Claude41Opus = "claude-opus-4-1-20250805";
}
 