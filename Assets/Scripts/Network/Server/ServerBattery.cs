using Mirror;
using UnityEngine;
public class ServerBattery: MonoBehaviour
{
    private ServerBattery(){}

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGameClickedOnBatteryMessage>(OnServerClientGameClickedOnBattery);
    }


    private void OnServerClientGameClickedOnBattery(NetworkConnection connection, ServerClientGameClickedOnBatteryMessage message)
    {
        Survivor survivor = connection.identity.GetComponent<Survivor>();

        if (survivor == null)
        {
            return;
        }

        if (!NetworkIdentity.spawned.ContainsKey(message.requestedBatteryId))
        {
            return;
        }

        Battery battery = NetworkIdentity.spawned[message.requestedBatteryId].GetComponent<Battery>();

        if (battery == null)
        {
            return;
        }

        Vector3 batteryPos = battery.transform.position;
        Vector3 survivorPos = survivor.transform.position;

        float distance = Vector3.Distance(batteryPos, survivorPos);

        if (distance > survivor.GrabDistance())
        {
            return;
        }

        if (survivor.FlashlightCharge() <= battery.ChargeNeededToGrab())
        {
            // NOTE: No need to send a message here because this function updates a syncvar variable.
            survivor.RechargeFlashlight();
            NetworkServer.Destroy(battery.gameObject);
        }

        else
        {
            ClientServerGameRejectedBatteryPickupMessage clientServerGameRejectedBatteryPickupMessage = new ClientServerGameRejectedBatteryPickupMessage{};
            connection.identity.connectionToClient.Send(clientServerGameRejectedBatteryPickupMessage);

        }
    }
}