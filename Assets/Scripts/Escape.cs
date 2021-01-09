using UnityEngine;

public class Escape : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        GameObject gameObject = collider.gameObject;

        if (gameObject.CompareTag(Tags.SURVIVOR))
        {
            Survivor survivor = gameObject.GetComponent<Survivor>();
            survivor.isInEscapeRoom = true;

            GameObject[] survivors = GameObject.FindGameObjectsWithTag(Tags.SURVIVOR);

            bool canEscape = true;

            for (var i = 0; i < survivors.Length; i++)
            {
                Survivor otherSurvivor = survivors[i].GetComponent<Survivor>();

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
    }

    void OnTriggerExit(Collider collision)
    {
        GameObject gameObject = collision.gameObject;

        if (gameObject.CompareTag(Tags.SURVIVOR))
        {
            Survivor survivor = gameObject.GetComponent<Survivor>();
            survivor.isInEscapeRoom = false;
        }
    }
}
