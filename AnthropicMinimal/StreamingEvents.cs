using System.Text.Json.Serialization;

namespace AnthropicMinimal;

/// <summary>
/// Base class for all streaming events
/// </summary>
public class StreamEvent
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
}

/// <summary>
/// Event when message starts
/// </summary>
public class MessageStartEvent : StreamEvent
{
    [JsonPropertyName("message")]
    public MessageResponse Message { get; set; } = new();
}

/// <summary>
/// Event when content block starts
/// </summary>
public class ContentBlockStartEvent : StreamEvent
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("content_block")]
    public ContentBlock ContentBlock { get; set; } = new();
}

/// <summary>
/// Event for content block delta (streaming text)
/// </summary>
public class ContentBlockDeltaEvent : StreamEvent
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("delta")]
    public Delta Delta { get; set; } = new();
}

public class Delta
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

/// <summary>
/// Event when content block stops
/// </summary>
public class ContentBlockStopEvent : StreamEvent
{
    [JsonPropertyName("index")]
    public int Index { get; set; }
}

/// <summary>
/// Event for message delta (final usage info, etc)
/// </summary>
public class MessageDeltaEvent : StreamEvent
{
    [JsonPropertyName("delta")]
    public MessageDelta Delta { get; set; } = new();

    [JsonPropertyName("usage")]
    public OutputUsage Usage { get; set; } = new();
}

public class MessageDelta
{
    [JsonPropertyName("stop_reason")]
    public string? StopReason { get; set; }

    [JsonPropertyName("stop_sequence")]
    public string? StopSequence { get; set; }
}

public class OutputUsage
{
    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }
}

/// <summary>
/// Event when message stops
/// </summary>
public class MessageStopEvent : StreamEvent
{
}

/// <summary>
/// Event when an error occurs
/// </summary>
public class ErrorEvent : StreamEvent
{
    [JsonPropertyName("error")]
    public ErrorInfo Error { get; set; } = new();
}

public class ErrorInfo
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}
