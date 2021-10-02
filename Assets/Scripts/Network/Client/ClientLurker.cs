using UnityEngine;
using Mirror;
public class ClientLurker: MonoBehaviour
{
    private ClientLurker(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGameLurkerArmableTrapsMessage>(OnClientServerGameArmableTraps);
        NetworkClient.RegisterHandler<ClientServerGameLurkerReadyToGoIntoPhysicalFormMessage>(OnClientServerGameLurkerReadyToGoIntoPhysicalForm);
        NetworkClient.RegisterHandler<ClientServerGameLurkerTrapArmedMessage>(OnClientServerGameLurkerTrapArmed);
        NetworkClient.RegisterHandler<ClientServerGameLurkerAttackedMessage>(OnClientServerGameLurkerAttacked);

    }

    private void OnClientServerGameArmableTraps(NetworkConnection connection, ClientServerGameLurkerArmableTrapsMessage message)
    {
        EventManager.clientServerGameLurkerArmableTrapsEvent.Invoke(message.armableTraps);

    }

    private void OnClientServerGameLurkerReadyToGoIntoPhysicalForm(NetworkConnection connection, ClientServerGameLurkerReadyToGoIntoPhysicalFormMessage message)
    {
        EventManager.clientServerGameLurkerReadyToGoIntoPhysicalFormEvent.Invoke();

    }

    private void OnClientServerGameLurkerTrapArmed(NetworkConnection connection, ClientServerGameLurkerTrapArmedMessage message)
    {
        EventManager.clientServerGameLurkerTrapArmedEvent.Invoke();


    }

    private void OnClientServerGameLurkerAttacked(NetworkConnection connection, ClientServerGameLurkerAttackedMessage message)
    {
        uint netId = message.lurkerId;

        Lurker lurker = NetworkIdentity.spawned[netId].GetComponent<Lurker>();

        lurker.ClientPlayAttackSound();
    }
}

