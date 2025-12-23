using System.Text.Json.Serialization;

namespace AnthropicMinimal;

/// <summary>
/// Text content for messages
/// </summary>
public class TextContent : IMessageContent
{
    [JsonPropertyName("type")]
    public string Type => "text";

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    public TextContent() { }

    public TextContent(string text)
    {
        Text = text;
    }
}
