using System.Text.Json;
using System.Text.Json.Nodes;

public class ConfigurationReader
{
    private readonly JsonNode _config;

    public ConfigurationReader(string jsonContent)
    {
        _config = JsonNode.Parse(jsonContent);
    }

    public T GetValue<T>(string key)
    {
        var node = _config[key];
        // 对于值类型,需要判断是否为空并返回默认值
        if (node == null)
        {
            return default;
        }
        return node.Deserialize<T>();
    }
}