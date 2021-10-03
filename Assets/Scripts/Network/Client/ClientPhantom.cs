using Mirror;
using UnityEngine;

public class ClientPhantom: MonoBehaviour
{
    private ClientPhantom(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGamePhantomAttackedMessage>(OnClientServerGamePhantomAttacked);
        NetworkClient.RegisterHandler<ClientServerGamePhantomSurvivorDetectedMesasge>(OnClientServerGamePhantomSurvivorDetected);
        NetworkClient.RegisterHandler<ClientServerGamePhantomSurvivorNoLongerDetected>(OnClientServerGamePhantomSurvivorNoLongerDetected);

    }

    private void OnClientServerGamePhantomAttacked(NetworkConnection connection, ClientServerGamePhantomAttackedMessage message)
    {
        NetworkIdentity.spawned[message.phantomId].GetComponent<Phantom>().ClientPlayAttackSound();

    }

    private void OnClientServerGamePhantomSurvivorDetected(NetworkConnection connection, ClientServerGamePhantomSurvivorDetectedMesasge message)
    {
        uint survivorId = message.detectedSurvivorId;

        Survivor survivor = NetworkIdentity.spawned[survivorId].GetComponent<Survivor>();

        survivor.ClientShow();
    }

    private void OnClientServerGamePhantomSurvivorNoLongerDetected(NetworkConnection connection, ClientServerGamePhantomSurvivorNoLongerDetected message)
    {
        uint survivorId = message.noLongerDetectedSurvivorId;

        Survivor survivor = NetworkIdentity.spawned[survivorId].GetComponent<Survivor>();

        survivor.ClientHide();
    }
}