using UnityEngine;

public class Escape : MonoBehaviour
{

    public SurvivorEnteredExitZone survivorEnteredExitZone;
    public SurvivorLeftExitZone survivorLeftExitZone;

    void OnTriggerEnter(Collider collision)
    {
        GameObject gameObject = collision.gameObject;

        if (gameObject.tag == "Survivor")
        {
            Survivor survivor = gameObject.GetComponent<Survivor>();
            survivorEnteredExitZone.Invoke(survivor);

        }
    }


    void OnTriggerExit(Collider collision)
    {
        GameObject gameObject = collision.gameObject;

        if (gameObject.tag == "Survivor")
        {
            Survivor survivor = gameObject.GetComponent<Survivor>();
            survivorLeftExitZone.Invoke(survivor);
        }
    }

}
