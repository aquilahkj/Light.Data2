using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class JsonConfigurationProvider
{
    private readonly JsonSerializerOptions _options;

    public JsonConfigurationProvider()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };
    }

    private JsonNode ParseJson(string json)
    {
        return JsonNode.Parse(json);
    }

    public T DeserializeObject<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _options);
    }

    public string SerializeObject(object value)
    {
        return JsonSerializer.Serialize(value, _options);
    }
} 