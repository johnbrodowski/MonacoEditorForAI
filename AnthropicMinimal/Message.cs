using System.Text.Json.Serialization;

namespace AnthropicMinimal;

/// <summary>
/// Represents a message in a conversation
/// </summary>
public class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public List<IMessageContent> Content { get; set; } = new();

    public Message() { }

    public Message(string role)
    {
        Role = role;
    }

    /// <summary>
    /// Creates a user message with text content
    /// </summary>
    public static Message CreateUserMessage(string text)
    {
        var message = new Message(Roles.User);
        message.Content.Add(new TextContent(text));
        return message;
    }

    /// <summary>
    /// Creates an assistant message with text content
    /// </summary>
    public static Message CreateAssistantMessage(string text)
    {
        var message = new Message(Roles.Assistant);
        message.Content.Add(new TextContent(text));
        return message;
    }
}

public static class Roles
{
    public const string User = "user";
    public const string Assistant = "assistant";
}
