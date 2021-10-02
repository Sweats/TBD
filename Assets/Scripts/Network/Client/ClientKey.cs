using Mirror;
using UnityEngine;
public class ClientKey: MonoBehaviour
{
    private ClientKey(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGameAlreadyHaveKeyMessage>(OnClientServerGameAlreadyHaveKey);
        NetworkClient.RegisterHandler<ClientServerGameSurvivorPickedUpKeyMessage>(OnClientServerGameSurvivorPickedUpKey);

    }


    private void OnClientServerGameAlreadyHaveKey(NetworkConnection connection, ClientServerGameAlreadyHaveKeyMessage message)
    {
        EventManager.clientServerGameAlreadyHaveKeyEvent.Invoke();

    }

    private void OnClientServerGameSurvivorPickedUpKey(NetworkConnection connection, ClientServerGameSurvivorPickedUpKeyMessage message)
    {
        uint keyId = message.keyId;
        uint survivorId = message.survivorId;

        KeyObject keyObject = NetworkIdentity.spawned[keyId].GetComponent<KeyObject>();

        if (keyObject != null)
        {
            string keyName = keyObject.Name();
            string survivorName = NetworkIdentity.spawned[survivorId].GetComponent<Survivor>().Name();
            keyObject.ClientPlayPickupSound();
            keyObject.Hide();
            EventManager.clientServerGameSurvivorPickedUpKeyEvent.Invoke(survivorName, keyName);
        }
    }
}