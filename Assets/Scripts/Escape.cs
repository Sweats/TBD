using UnityEngine;
using Mirror;

public class Escape : NetworkBehaviour
{
    [Client]
    void OnTriggerEnter(Collider collider)
    {
        GameObject gameObject = collider.gameObject;

        if (gameObject.CompareTag(Tags.SURVIVOR))
        {
            Survivor survivor = gameObject.GetComponent<Survivor>();
            CmdSurvivorEnteredEscapeRoom();
        }
    }

    [Command(requiresAuthority=false)]
    private void CmdSurvivorEnteredEscapeRoom(NetworkConnectionToClient sender = null)
    {
        Survivor survivor = sender.identity.GetComponent<Survivor>();
        GameObject[] survivors = GameObject.FindGameObjectsWithTag(Tags.SURVIVOR);
        survivor.SetEscaped(true);
        int escapeCount = 0;

        for (var i = 0; i < survivors.Length; i++)
        {
            Debug.Log($"Survivor escaped: {survivor.Escaped()} ");
            Survivor otherSurvivor = survivors[i].GetComponent<Survivor>();

            if (survivor.dead)
            {
                continue;
            }

            else if (survivor.Escaped())
            {
                escapeCount++;
            }

        }

        Debug.Log($"{escapeCount}");

        if (escapeCount >= survivors.Length)
        {
            RpcSurvivorsEscapedStageEvent();
        }
    }

    [Command(requiresAuthority=true)]
    private void CmdSurviorLeftEscapeRoom(NetworkConnectionToClient sender = null)
    {
        Survivor survivor = sender.identity.GetComponent<Survivor>();
        survivor.SetEscaped(false);
    }


    [ClientRpc]
    private void RpcSurvivorsEscapedStageEvent()
    {
        EventManager.survivorsEscapedStageEvent.Invoke();
    }

    [Client]
    void OnTriggerExit(Collider collision)
    {
        GameObject gameObject = collision.gameObject;

        if (gameObject.CompareTag(Tags.SURVIVOR))
        {
            Survivor survivor = gameObject.GetComponent<Survivor>();
            CmdSurviorLeftEscapeRoom();
        }
    }
}
