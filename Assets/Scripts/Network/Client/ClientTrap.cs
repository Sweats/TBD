using Mirror;
using UnityEngine;

public class ClientTrap: MonoBehaviour
{
    private ClientTrap(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGameTrapTriggeredMessage>(OnClientServerGameSurvivorTriggeredTrap);

    }

    private void OnClientServerGameSurvivorTriggeredTrap(NetworkConnection connection,  ClientServerGameTrapTriggeredMessage message)
    {
        uint netid = message.triggeredTrapId;
        Trap trap = NetworkIdentity.spawned[netid].GetComponent<Trap>();
        trap.ClientTrapTriggered();
    }
}