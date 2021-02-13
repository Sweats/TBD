using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class MasterServerConfiguration
{
    public ushort port;

    public static MasterServerConfiguration Load(string configPath)
    {
        string yamlData = File.ReadAllText(configPath);
        var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        MasterServerConfiguration configuration = deserializer.Deserialize<MasterServerConfiguration>(yamlData);
        return configuration;
    }


    public static bool Exists(string configPath)
    {
        if (!File.Exists(configPath))
        {
            DarnedNetworkManager.Log($"The configuration file {configPath} does not exist. Generating a new one...");
            string currentDirectoryName = Directory.GetCurrentDirectory();
            GenerateConfig(configPath);
            DarnedNetworkManager.Log($"Generated new configuration file named {configPath} in the directory {currentDirectoryName}");
            return false;
        }

        return true;
    }

    private static void GenerateConfig(string configPath)
    {
        var config = new MasterServerConfiguration
        {
            port = 7777,
        };

        var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        string yamlData = serializer.Serialize(config);
        File.WriteAllText(configPath, yamlData);
    }


    public ushort Port()
    {
        return port;
    }
}
