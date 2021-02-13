using UnityEngine;
using Mirror;

public class DarnedMasterServerStart : MonoBehaviour
{
    private void Start()
    {
        string configName = "darned_master_server_configuration.yml";

        if (!MasterServerConfiguration.Exists(configName))
        {
            DarnedMasterServerManager.Log("Exiting...");
            Application.Quit();
            return;
        }

        if (NetworkManager.singleton == null)
        {
            GameObject masterServerObject = (GameObject)Resources.Load("Darned Master Server Manager");
            GameObject spawnedObject = Instantiate(masterServerObject);
            DontDestroyOnLoad(spawnedObject);
        }

        MasterServerConfiguration serverConfiguration = MasterServerConfiguration.Load(configName);
        ushort port = serverConfiguration.Port();
        DarnedMasterServerManager.Log($"Starting the master server on port {port}...");
        DarnedMasterServerManager.PORT = port;
        NetworkManager.singleton.StartServer();
    }
}
