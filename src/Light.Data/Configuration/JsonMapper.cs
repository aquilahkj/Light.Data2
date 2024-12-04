using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

public class JsonMapper
{
    private readonly JsonSerializerOptions _options;

    public JsonMapper()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public T MapFromJson<T>(string json) where T : class
    {
        return JsonSerializer.Deserialize<T>(json, _options);
    }

    public string MapToJson(object value)
    {
        return JsonSerializer.Serialize(value, _options);
    }

    public Dictionary<string, object> ParseJsonObject(string json)
    {
        var jsonNode = JsonNode.Parse(json);
        return jsonNode.Deserialize<Dictionary<string, object>>(_options);
    }
} 