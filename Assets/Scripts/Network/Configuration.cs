using UnityEngine;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class Configuration
{
    public string serverName;
    public ushort port;
    public string lobbyPassword;

    public static Configuration Load(string configPath)
    {
        string yamlData = File.ReadAllText(configPath);
        var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        Configuration configuration = deserializer.Deserialize<Configuration>(yamlData);
        return configuration;
    }


    public static bool Exists(string configPath)
    {
        if (!File.Exists(configPath))
        {
            Debug.Log($"[Darned]: The configuration file {configPath} does not exist. Generating a new one...");
            string currentDirectoryName = Directory.GetCurrentDirectory();
            GenerateConfig(configPath);
            Debug.Log($"[Darned]: Generated new configuration file named {configPath} in the directory {currentDirectoryName}");
            return false;
        }

        return true;
    }

    private static void GenerateConfig(string configPath)
    {
        var config = new Configuration
        {
            serverName = "New Darned Server",
            port = 7777,
            lobbyPassword = string.Empty

        };

        var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
        string yamlData = serializer.Serialize(config);
        File.WriteAllText(configPath, yamlData);
    }

    public string Name()
    {
        return serverName;
    }

    public ushort Port()
    {
        return port;
    }

    public string Password()
    {
        return lobbyPassword;
    }
}
