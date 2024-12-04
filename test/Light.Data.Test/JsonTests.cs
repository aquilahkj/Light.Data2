using System;
using System.Text.Json;
using Xunit;

public class TestClass
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class JsonTests
{
    [Fact]
    public void TestJsonSerialization()
    {
        var obj = new TestClass { Id = 1, Name = "test" };
        var json = JsonSerializer.Serialize(obj);
        var result = JsonSerializer.Deserialize<TestClass>(json);

        Assert.Equal(obj.Id, result.Id);
        Assert.Equal(obj.Name, result.Name);
    }
} 