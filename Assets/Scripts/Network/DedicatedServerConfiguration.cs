using UnityEngine;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class DedicatedServerConfiguration
{
    public string serverName;
    public ushort port;
    public string lobbyPassword;

    public static DedicatedServerConfiguration Load(string configPath)
    {
        string yamlData = File.ReadAllText(configPath);
        var deserializer = new Deserializer();
        DedicatedServerConfiguration configuration = deserializer.Deserialize<DedicatedServerConfiguration>(yamlData);
        return configuration;
    }


    public static bool Exists(string configPath)
    {
        if (!File.Exists(configPath))
        {
            //DarnedNetworkManager.Log($"The configuration file {configPath} does not exist. Generating a new one...");
            string currentDirectoryName = Directory.GetCurrentDirectory();
            GenerateConfig(configPath);
            //DarnedNetworkManager.Log($"Generated new configuration file named {configPath} in the directory {currentDirectoryName}");
            return false;
        }

        return true;
    }

    public static void GenerateConfig(string configPath)
    {
        var config = new DedicatedServerConfiguration
        {
            serverName = "New Darned Server",
            port = 7777,
            lobbyPassword = string.Empty

        };

        var serializer = new Serializer();
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
