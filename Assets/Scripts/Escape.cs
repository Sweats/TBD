using UnityEngine;
using Mirror;

public class Escape : NetworkBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        GameObject gameObject = collider.gameObject;

        if (gameObject.CompareTag(Tags.SURVIVOR))
        {
            Survivor survivor = gameObject.GetComponent<Survivor>();
            CmdSurvivorEnteredEscapeRoom(survivor.netIdentity);
        }
    }

    [Command]
    private void CmdSurvivorEnteredEscapeRoom(NetworkIdentity survivorIdentity)
    {
        Survivor survivor = survivorIdentity.gameObject.GetComponent<Survivor>();
        survivor.SetEscaped(true);

        GameObject[] survivors = GameObject.FindGameObjectsWithTag(Tags.SURVIVOR);

        bool canEscape = true;

        for (var i = 0; i < survivors.Length; i++)
        {
            Survivor otherSurvivor = survivors[i].GetComponent<Survivor>();

            if (survivor.dead)
            {
                continue;
            }

            if (!survivor.Escaped())
            {
                canEscape = false;
                break;
            }
        }

        if (canEscape)
        {
            RpcSurvivorsEscapedStageEvent();
        }

    }


    [Command]
    private void CmdSurviorLeftEscapeRoom(NetworkIdentity survivorIdentity)
    {
        Survivor survivor = survivorIdentity.gameObject.GetComponent<Survivor>();
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
            CmdSurviorLeftEscapeRoom(survivor.netIdentity);
        }
    }
}
