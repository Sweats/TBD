using UnityEngine;

public class Stage : MonoBehaviour
{

    [SerializeField]
    private Survivor[] survivors;

    public void OnSurviorDeath(Survivor survivor)
    {
        for (var i = 0; i < survivors.Length; i++)
        {
            if (survivors[i].survivorID == survivor.survivorID)
            {
                // We will mark this survivor as dead later.
                survivor.insanity.Reset();
            }

            survivors[i].insanity.insanityValue += survivors[i].insanity.insanitySurvivorDeathAmount;
        }

    }


    public void OnSurvivorTriggeredTrap(Survivor survivor, Trap trap)
    {
        survivor.insanity.insanityValue += survivor.insanity.insanityHitTrapAmount;
        trap.Trigger();
    }

    public void OnSurvivorPickedUpBattery(Survivor survivor, Battery battery)
    {
        survivor.flashlight.Recharge();
        battery.Delete();
    }
    public void OnSurvivorPickedUpKey(Survivor survivor, KeyObject keyObject)
    {
        survivor.inventory.Add(new Key(keyObject.key));
        keyObject.Delete();
    }

    public void OnSurvivorUnlockedDoorEvent(Survivor survivor, Key key, Door door)
    {
        door.Unlock();
    }


    public void OnSurvivorFailedToUnlockDoor(Door door)
    {
        door.PlayLockedSound();
    }


    public void OnSurvivorStartSprintingEvent(Survivor survivor)
    {

    }


    public void OnSurvivorStopSprintingEvent(Survivor survivor)
    {

    }
}
