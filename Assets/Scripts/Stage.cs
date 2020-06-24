using UnityEngine;

public class Stage : MonoBehaviour
{

    [SerializeField]
    private Survivor[] survivors;
    private Lurker lurker;

    public SurvivorsEscapedStageEvent survivorsEscapedStageEvent;
    public SurvivorUnlockDoorEvent survivorUnlockDoorEvent;
    public SurvivorPickedUpBatteryEvent survivorPickedUpBatteryEvent;
    public SurvivorFailedToPickUpbBatteryEvent survivorFailedToPickUpBatteryEvent;

    public SurviorGrabbedKeyEvent survivorGrabbedKeyEvent;
    public SurvivorAlreadyHaveKeyEvent survivorAlreadyHaveKeyEvent;


    public void OnSurviorDeath(Survivor who)
    {
        for (var i = 0; i < survivors.Length; i++)
        {
            if (survivors[i].survivorID == who.survivorID)
            {
                who.dead = true;
                who.insanity.Reset();
                continue;
            }

            survivors[i].insanity.insanityValue += survivors[i].insanity.insanitySurvivorDeathAmount;
        }
    }


    public void OnSurvivorTriggeredTrap(Survivor who, Trap trap)
    {
        who.insanity.insanityValue += who.insanity.insanityHitTrapAmount;
        trap.Trigger();
    }

    public void OnSurvivorClickedOnBatteryEvent(Survivor who, Battery battery)
    {
        if (who.flashlight.charge <= battery.chargeNeededToGrab)
        {
            survivorPickedUpBatteryEvent.Invoke(who, battery);
            who.flashlight.Recharge();
            battery.Delete();
        }

        else
        {
            survivorFailedToPickUpBatteryEvent.Invoke();

        }
    }

    public void OnSurvivorClickedOnDoorEvent(Survivor who, Door door)
    {
        Key[] keys = who.inventory.Keys();
        bool found = false;

        for (var i = 0; i < keys.Length; i++)
        {
            int unlockMask = keys[i].mask;

            if (door.unlockMask == unlockMask)
            {
                Key key = keys[i];
                survivorUnlockDoorEvent.Invoke(who, key, door);
                door.Unlock();
                found = true;
                break;
            }
        }

        if (!found)
        {
            door.PlayLockedSound();
        }
    }


    public void OnSurvivorClickedOnKeyEvent(Survivor who, KeyObject keyObject)
    {
        Key[] keys = who.inventory.Keys();

        bool found = false;

        for (var i = 0; i < keys.Length; i++)
        {
            Key key = keys[i];
            int foundKeyMask = keyObject.key.mask;
            int currentInventoryKeyMask = key.mask;

            if (foundKeyMask == currentInventoryKeyMask)
            {
                found = true;
                survivorAlreadyHaveKeyEvent.Invoke();
                break;
            }
        }


        if (!found)
        {
            Key newKey = new Key(keyObject.key);
            who.inventory.Add(newKey);
            newKey.pickupSound.Play();
            survivorGrabbedKeyEvent.Invoke(who, newKey);
            keyObject.Delete();
        }
    }


    public void OnSurvivorStartSprintingEvent(Survivor who)
    {

    }


    public void OnSurvivorStopSprintingEvent(Survivor who)
    {

    }


    public void OnSurvivorEnteredExitZone(Survivor who)
    {
        who.isInEscapeRoom = true;
        bool canEscape = true;

        for (var i = 0; i < survivors.Length; i++)
        {
            if (survivors[i].dead)
            {
                continue;
            }

            if (!survivors[i].isInEscapeRoom)
            {
                canEscape = false;
                break;

            }
        }

        if (canEscape)
        {
            for (var i = 0; i < survivors.Length; i++)
            {
                survivors[i].matchOver = true;
            }

            survivorsEscapedStageEvent.Invoke();
        }
    }

    public void OnSurvivorLeftExitZone(Survivor who)
    {
        who.isInEscapeRoom = false;
    }

    public void OnLurkerTransform(bool ghostForm)
    {
        if (ghostForm)
        {
            //lurker.transform.position
        }

        else
        {

        }

    }
}
