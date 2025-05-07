using System.Text.Json;
using tokero_automation_tests.tokero_automation_tests.Models;

namespace tokero_automation_tests.tokero_automation_tests.Utils;

public class PropertiesReader
{
    public static Properties? Load(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Properties>(json);
    }
}