using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

public class DynamicObjectHandler
{
    public object ConvertDynamicValue(object value)
    {
        if (value is JsonObject jsonObj)
        {
            return jsonObj.Deserialize<Dictionary<string, object>>();
        }
        if (value is JsonArray jsonArr)
        {
            return jsonArr.Deserialize<List<object>>();
        }
        return value;
    }
} 