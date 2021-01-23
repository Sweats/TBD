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
    private float gameMessageAppearanceLength;

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
        EventManager.playerPickedUpKeyEvent.AddListener(OnSurvivorGrabbedKey);
        EventManager.survivorPickedUpBatteryEvent.AddListener(OnPickedUpBatteryEvent);
        EventManager.survivorUnlockDoorEvent.AddListener(OnSurvivorUnlockedDoor);
        EventManager.playerConnectedEvent.AddListener(OnPlayerConnect);
        EventManager.playerDisconnectedEvent.AddListener(OnPlayerDisconnect);
        EventManager.lurkerReadyToGoIntoPhysicalFormEvent.AddListener(OnLurkerReadyToGoIntoPhysicalForm);
        EventManager.maryReadyToFrenzyEvent.AddListener(OnMaryReadyToFrenzy);
        EventManager.maryReadyToTeleportEvent.AddListener(OnMaryReadyToTeleport);
        EventManager.lobbyOtherPlayerConnectEvent.AddListener(OnPlayerJoinedLobby);
        EventManager.lobbyOtherPlayerDisconnectedEvent.AddListener(OnPlayerLeftLobby);
        EventManager.lobbyClientFailedToConnectToHostEvent.AddListener(OnPlayerFailedToConnectToRemoteHost);
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

    private void OnSurvivorGrabbedKey(string survivorName, string keyName)
    {
        string newMessage = $"{survivorName} picked up a {keyName}!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    private void OnSurvivorUnlockedDoor(string survivorName, string doorName, string keyName)
    {
        string newMessage = $"{survivorName} used a {keyName} to unlock a {doorName}!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));

    }

    private void OnSurvivorDeath(string playerName)
    {
        string newMessage = playerName;
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

    private void OnPlayerDisconnect(string playerName)
    {
        string newMessage = $"Player {playerName} has disconnected!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    private void OnPlayerConnect(string playerName)
    {
        string newMessage = $"Player {playerName} has connected!";
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

    private void OnMaryReadyToFrenzy()
    {
        string newMessage = $"Click {Keybinds.GetKey(Action.Transform)} to enter frenzied mode.";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    private void OnMaryReadyToTeleport()
    {
        string newMessage = $"Click {Keybinds.GetKey(Action.Teleport)} to teleport.";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    private void OnPlayerJoinedLobby(string playerName)
    {
        string newMessage = $"Player {playerName} connected!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    private void OnPlayerLeftLobby(string playerName, int netId)
    {
        string newMessage = $"Player {playerName} disconnected!";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    private void OnPlayerFailedToConnectToRemoteHost(int errorCode)
    {
        string newMessage = $"Failed to connect to the remote host. Error code = {errorCode}";
        StartCoroutine(AddAndRemoveGameMessage(newMessage));
    }

    private IEnumerator AddAndRemoveGameMessage(string newMessage)
    {
        messages.Add(newMessage);
        UpdateChatText();
        yield return new WaitForSeconds(gameMessageAppearanceLength);
        messages.Remove(newMessage);
        UpdateChatText();
    }

}
