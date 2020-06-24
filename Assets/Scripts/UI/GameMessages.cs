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
    // Start is called before the first frame update
    void Start()
    {
        stringBuilder = new StringBuilder();
        messages = new List<string>();
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
    public void OnSurvivorGrabbedKey(Survivor who, Key key)
    {
        string newMessage = $"{who.survivorName} picked up a {key.keyName}!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    public void OnSurvivorUnlockedDoor(Survivor who, Key key, Door door)
    {
        string newMessage = $"{who.survivorName} used a {key.keyName} to unlock a {door.doorName}!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));

    }

    public void OnSurvivorDeath(Survivor who)
    {
        string newMessage = $"{who.survivorName} died!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));

    }


    public void OnFailedToPickUpBatteryEvent()
    {
        string newMessage = "Your flashlight is already charged!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    public void OnPickedUpBatteryEvent()
    {
        string newMessage = "You picked up a battery!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }


    public void OnPickupKeySurvivorAlreadyHasEvent()
    {
        string newMessage = "You already have this key!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    public void OnSurvivorDisconnect(Survivor who)
    {
        string newMessage = $"Player {who.survivorName} has left the game!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    public void OnSurvivorConnect(Survivor who)
    {
        string newMessage = $"Player {who.survivorName} has connected!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));

    }

    public void OnHostStartedTheGame()
    {
        string newMessage = "The host has started the game!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
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