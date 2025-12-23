using System.Text.Json.Serialization;

namespace AnthropicMinimal;

/// <summary>
/// Image content for messages
/// </summary>
public class ImageContent : IMessageContent
{
    [JsonPropertyName("type")]
    public string Type => "image";

    [JsonPropertyName("source")]
    public ImageSource Source { get; set; } = new();
}

public class ImageSource
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "base64";

    [JsonPropertyName("media_type")]
    public string MediaType { get; set; } = "image/jpeg";

    [JsonPropertyName("data")]
    public string Data { get; set; } = string.Empty;
}
