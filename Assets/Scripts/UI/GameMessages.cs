using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameMessages : MonoBehaviour
{

    private List<string> messages;

    // How long should a chat message appear for? 
    [SerializeField]
    private float chatMessageAppearanceLength;

    [SerializeField]
    private InputField messagesBox;

    [SerializeField]
    private Canvas gameMessagesCanvas;
    private StringBuilder stringBuilder;
    
    void Start()
    {
        stringBuilder = new StringBuilder();
        messages = new List<string>();

        EventManager.survivorDeathEvent.AddListener(OnSurvivorDeath);
        EventManager.survivorFailedToPickUpBatteryEvent.AddListener(OnFailedToPickUpBatteryEvent);
        EventManager.survivorPickedUpKeyEvent.AddListener(OnSurvivorGrabbedKey);
        EventManager.survivorPickedUpBatteryEvent.AddListener(OnPickedUpBatteryEvent);
        EventManager.survivorUnlockDoorEvent.AddListener(OnSurvivorUnlockedDoor);
        EventManager.playerConnectedEvent.AddListener(OnPlayerConnect);
        EventManager.playerDisconnectedEvent.AddListener(OnPlayerDisconnect);
	EventManager.lurkerReadyToGoIntoPhysicalFormEvent.AddListener(OnLurkerReadyToGoIntoPhysicalForm);

    }

    private void UpdateChatText()
    {
        stringBuilder.Clear();

        for (var i = 0; i < messages.Count; i++)
        {
            stringBuilder.AppendLine(messages[i]);
        }

        messagesBox.text = stringBuilder.ToString();
    }
    private void OnSurvivorGrabbedKey(Survivor who, Key key)
    {
        string newMessage = $"{who.survivorName} picked up a {key.keyName}!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }
    private void OnSurvivorUnlockedDoor(Survivor who, Key key, Door door)
    {
        string newMessage = $"{who.survivorName} used a {key.keyName} to unlock a {door.doorName}!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));

    }

    private void OnSurvivorDeath(Survivor who)
    {
        string newMessage = $"{who.survivorName} died!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));

    }


    private void OnFailedToPickUpBatteryEvent()
    {
        string newMessage = "Your flashlight is already charged!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    private void OnPickedUpBatteryEvent(Survivor survivor, Battery battery)
    {
        string newMessage = "You picked up a battery!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }


    private void OnPickupKeySurvivorAlreadyHasEvent()
    {
        string newMessage = "You already have this key!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    private void OnPlayerDisconnect(Survivor who)
    {
        string newMessage = $"Player {who.survivorName} has left the game!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    private void OnPlayerConnect(Survivor who)
    {
        string newMessage = $"Player {who.survivorName} has connected!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));

    }

    private void OnHostStartedTheGame()
    {
        string newMessage = "The host has started the game!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }


    private void OnLurkerReadyToGoIntoPhysicalForm()
    {
	    string newMesasge = "You may now transform into physical form.";
	    StartCoroutine(AddAndRemoveGameMessage(newMesasge));
    }


    private IEnumerator AddAndRemoveGameMessage(string newMessage)
    {
        messages.Add(newMessage);
        UpdateChatText();
        yield return new WaitForSeconds(chatMessageAppearanceLength);
        messages.Remove(newMessage);
        UpdateChatText();
    }


}
