using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

public class DataConfigurationProvider
{
    public Dictionary<string, object> LoadConfiguration(string jsonContent)
    {
        var jsonNode = JsonNode.Parse(jsonContent);
        return jsonNode.Deserialize<Dictionary<string, object>>();
    }
} 