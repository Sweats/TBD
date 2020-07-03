using UnityEngine;

public class Escape : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        GameObject gameObject = collider.gameObject;

        if (gameObject.tag == "Survivor")
        {
            Survivor survivor = gameObject.GetComponent<Survivor>();
            survivor.isInEscapeRoom = true;

        }

        GameObject[] survivors = GameObject.FindGameObjectsWithTag("Survivor");

        bool canEscape = true;

        for (var i = 0; i < survivors.Length; i++)
        {
            Survivor survivor = survivors[i].GetComponent<Survivor>();

            if (survivor.dead)
            {
                continue;
            }


            if (!survivor.isInEscapeRoom)
            {
                canEscape = false;
                break;
            }
        }


        if (canEscape)
        {
            EventManager.survivorsEscapedStageEvent.Invoke();
        }
    }

    void OnTriggerExit(Collider collision)
    {
        GameObject gameObject = collision.gameObject;

        if (gameObject.tag == "Survivor")
        {
            Survivor survivor = gameObject.GetComponent<Survivor>();
            survivor.isInEscapeRoom = false;
        }
    }
}
