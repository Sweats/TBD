using UnityEngine;
using Mirror;

public class ClientMary: MonoBehaviour
{
    private ClientMary(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGameMaryFrenziedMessage>(OnClientServerGameMaryFrenzied);
        NetworkClient.RegisterHandler<ClientServerGameMaryAllowTeleportMessage>(OnClientServerGameAllowTelportationRequest);
        NetworkClient.RegisterHandler<ClientServerGameMaryReadyToTeleportMessage>(OnClientServerGameMaryReadyToTeleport);
        NetworkClient.RegisterHandler<ClientServerGameMaryReadyToFrenzyMessage>(OnClientServerGameMaryReadyToFrenzy);
        NetworkClient.RegisterHandler<ClientServerGameMaryAttackedMessage>(OnClientServerGameMaryAttacked);
        NetworkClient.RegisterHandler<ClientServerGameMaryAutoTeleportMessage>(OnClientServerGameMaryAutoTeleport);
        NetworkClient.RegisterHandler<ClientServerGameMaryFrenzyOverMessage>(OnClientServerGameMaryFrenzyOver);

    }

    private void OnClientServerGameAllowTelportationRequest(NetworkConnection connection, ClientServerGameMaryAllowTeleportMessage message)
    {
        float x = message.newPosition.x;
        float y = message.newPosition.y;
        float z = message.newPosition.z;
        EventManager.clientServerGameMaryServerTeleportedYouEvent.Invoke(x, y, z);
    }

    private void OnClientServerGameMaryFrenzyOver(NetworkConnection connection, ClientServerGameMaryFrenzyOverMessage message)
    {
        uint id = message.maryId;
        Mary mary = NetworkIdentity.spawned[id].GetComponent<Mary>();
        mary.ClientPlayMaryCryingSound();
        // NOTE: Only the local player for Mary will get this event.
        EventManager.clientServerGameMaryFrenzyOverEvent.Invoke();


    }

    private void OnClientServerGameMaryFrenzied(NetworkConnection connection, ClientServerGameMaryFrenziedMessage message)
    {
        Mary mary = NetworkIdentity.spawned[message.maryId].GetComponent<Mary>();
        mary.ClientPlayFrenzySound();
        mary.ClientStopCryingSound();
        // NOTE: Only the local player for Mary will get this event.
        EventManager.clientServerGameMaryFrenziedEvent.Invoke();

    }

    private void OnClientServerGameMaryAttacked(NetworkConnection connection, ClientServerGameMaryAttackedMessage message)
    {
        Mary mary = NetworkIdentity.spawned[message.maryId].GetComponent<Mary>();
        mary.ClientPlayAttackSound();

    }

    private void OnClientServerGameMaryReadyToFrenzy(NetworkConnection connection, ClientServerGameMaryReadyToFrenzyMessage message)
    {
        EventManager.clientServerGameMaryReadyToFrenzyEvent.Invoke();
    }

    private void OnClientServerGameMaryReadyToTeleport(NetworkConnection connection, ClientServerGameMaryReadyToTeleportMessage message)
    {
        EventManager.clientServerGameMaryReadyToTeleportEvent.Invoke();

    }

    private void OnClientServerGameMaryAutoTeleport(NetworkConnection connection, ClientServerGameMaryAutoTeleportMessage message)
    {
        float x = message.newPosition.x;
        float y = message.newPosition.y;
        float z = message.newPosition.z;
        EventManager.clientServerGameMaryServerTeleportedYouEvent.Invoke(x, y, z);

    }

}

