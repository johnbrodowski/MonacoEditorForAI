using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AnthropicMinimal;

/// <summary>
/// Client for interacting with the Anthropic API
/// </summary>
public class AnthropicClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.anthropic.com/v1/messages";
    private const string ApiVersion = "2023-06-01";

    // Events for streaming
    public event EventHandler<MessageStartEvent>? MessageStart;
    public event EventHandler<ContentBlockStartEvent>? ContentBlockStart;
    public event EventHandler<ContentBlockDeltaEvent>? ContentBlockDelta;
    public event EventHandler<ContentBlockStopEvent>? ContentBlockStop;
    public event EventHandler<MessageDeltaEvent>? MessageDelta;
    public event EventHandler<MessageStopEvent>? MessageStop;
    public event EventHandler<ErrorEvent>? Error;
    public event EventHandler<string>? StreamingTextReceived;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
        Converters = { new MessageContentConverter() }
    };

    public AnthropicClient(string apiKey)
    {
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", ApiVersion);
    }

    /// <summary>
    /// Sends a message and receives a complete response
    /// </summary>
    public async Task<MessageResponse?> SendMessageAsync(MessageRequest request)
    {
        request.Stream = false;
        var json = JsonSerializer.Serialize(request, JsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(BaseUrl, content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MessageResponse>(responseJson, JsonOptions);
    }

    /// <summary>
    /// Sends a message with streaming enabled
    /// </summary>
    public async Task SendMessageStreamAsync(MessageRequest request)
    {
        request.Stream = true;
        var json = JsonSerializer.Serialize(request, JsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
        {
            Content = content
        };

        using var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line.StartsWith("data: "))
            {
                var data = line.Substring(6);
                if (data == "[DONE]") break;

                ProcessStreamEvent(data);
            }
        }
    }

    private void ProcessStreamEvent(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("type", out var typeElement))
                return;

            var eventType = typeElement.GetString();

            switch (eventType)
            {
                case "message_start":
                    var msgStart = JsonSerializer.Deserialize<MessageStartEvent>(json, JsonOptions);
                    if (msgStart != null) MessageStart?.Invoke(this, msgStart);
                    break;

                case "content_block_start":
                    var blockStart = JsonSerializer.Deserialize<ContentBlockStartEvent>(json, JsonOptions);
                    if (blockStart != null) ContentBlockStart?.Invoke(this, blockStart);
                    break;

                case "content_block_delta":
                    var blockDelta = JsonSerializer.Deserialize<ContentBlockDeltaEvent>(json, JsonOptions);
                    if (blockDelta != null)
                    {
                        ContentBlockDelta?.Invoke(this, blockDelta);
                        if (!string.IsNullOrEmpty(blockDelta.Delta.Text))
                        {
                            StreamingTextReceived?.Invoke(this, blockDelta.Delta.Text);
                        }
                    }
                    break;

                case "content_block_stop":
                    var blockStop = JsonSerializer.Deserialize<ContentBlockStopEvent>(json, JsonOptions);
                    if (blockStop != null) ContentBlockStop?.Invoke(this, blockStop);
                    break;

                case "message_delta":
                    var msgDelta = JsonSerializer.Deserialize<MessageDeltaEvent>(json, JsonOptions);
                    if (msgDelta != null) MessageDelta?.Invoke(this, msgDelta);
                    break;

                case "message_stop":
                    var msgStop = JsonSerializer.Deserialize<MessageStopEvent>(json, JsonOptions);
                    if (msgStop != null) MessageStop?.Invoke(this, msgStop);
                    break;

                case "error":
                    var error = JsonSerializer.Deserialize<ErrorEvent>(json, JsonOptions);
                    if (error != null) Error?.Invoke(this, error);
                    break;
            }
        }
        catch (JsonException)
        {
            // Ignore malformed JSON
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}

/// <summary>
/// Custom JSON converter for IMessageContent to handle polymorphic serialization
/// </summary>
public class MessageContentConverter : JsonConverter<IMessageContent>
{
    public override IMessageContent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        if (!root.TryGetProperty("type", out var typeProperty))
            return null;

        var type = typeProperty.GetString();
        return type switch
        {
            "text" => JsonSerializer.Deserialize<TextContent>(root.GetRawText(), options),
            "image" => JsonSerializer.Deserialize<ImageContent>(root.GetRawText(), options),
            _ => null
        };
    }

    public override void Write(Utf8JsonWriter writer, IMessageContent value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
