using UnityEngine;
using Mirror;

public class Escape : NetworkBehaviour
{
    [Client]
    private void OnTriggerEnter(Collider collider)
    {
        GameObject gameObject = collider.gameObject;

        if (gameObject.CompareTag(Tags.SURVIVOR))
        {
            NetworkClient.Send(new ServerClientGameSurvivorEscapedMessage{});
        }
    }

    [Client]
    private void OnTriggerExit(Collider collision)
    {
        GameObject gameObject = collision.gameObject;

        if (gameObject.CompareTag(Tags.SURVIVOR))
        {
            NetworkClient.Send(new ServerClientGameSurvivorNoLongerEscapedMessage{});
        }
    }
}
