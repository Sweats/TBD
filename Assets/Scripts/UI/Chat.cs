using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Chat : MonoBehaviour
{

    [SerializeField]
    private float chatAppearanceLength;
    private float currentChatApperanceLength;

    [SerializeField]
    private Canvas chatCanvas;

    [SerializeField]
    private InputField chatMessageBoxInput;

    [SerializeField]
    private InputField chatMessageBox;
    private string messageText;
    private StringBuilder stringBuilder;

    public static bool OPENED;


    void Start()
    {
        stringBuilder = new StringBuilder();
        currentChatApperanceLength = 0f;
        chatMessageBoxInput.text = string.Empty;
        EventManager.survivorSendChatMessageEvent.AddListener(OnSurviorRecievedChat);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !OPENED && !ConsoleUI.OPENED)
        {
            ShowChatMessageInputBox();
        }

        else if (Keybinds.GetKey(Action.GuiReturn) && OPENED)
        {
            HideChatMessageInputBox();
        }

        if (currentChatApperanceLength >= 0f)
        {
            currentChatApperanceLength -= Time.deltaTime;

            if (currentChatApperanceLength <= 0)
            {
                HideChatMessageBox();
            }
        }
    }
    public void OnSurvivorSendChat(string chatMessageText)
    {
        if (chatMessageBoxInput.text != string.Empty && OPENED)
        {
            ChatMessage chatMessage = new ChatMessage()
            {
                // TO DO: Figure out the best way to get the player name. Maybe use a global variable?
                survivorName = "player",
                text = chatMessageText,
                // TO DO: Figure out how we can get the survivor ID in the lobby.
                survivorID = 0
            };

            chatMessageBoxInput.text = string.Empty;
            EventManager.survivorSendChatMessageEvent.Invoke(chatMessage);
            chatMessageBoxInput.Select();
        }
    }

    private void UnselectChatMessageBox()
    {
        chatMessageBoxInput.OnDeselect(new BaseEventData(EventSystem.current));
    }


    private void OnSurviorRecievedChat(ChatMessage chatMessage)
    {
        currentChatApperanceLength = chatAppearanceLength;

        if (!PausedGameInput.GAME_PAUSED && !ConsoleUI.OPENED)
        {
            UpdateChatMessagesBox(chatMessage.text);
            ShowChatMessageBox();
        }
    }


    private void ShowChatMessageInputBox()
    {
        chatMessageBoxInput.interactable = true;
        OPENED = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void HideChatMessageInputBox()
    {
        chatMessageBoxInput.interactable = false;
        OPENED = false;
        chatMessageBoxInput.text = string.Empty;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void HideChatMessageBox()
    {
        chatMessageBox.enabled = false;
        chatMessageBox.image.enabled = false;
        chatMessageBox.textComponent.enabled = false;
        currentChatApperanceLength = 0f;
    }

    private void ShowChatMessageBox()
    {
        currentChatApperanceLength = chatAppearanceLength;
        chatMessageBox.image.enabled = true;
        chatMessageBox.textComponent.enabled = true;
        chatMessageBox.enabled = true;
    }

    private void UpdateChatMessagesBox(string chatMessageText)
    {
        currentChatApperanceLength = chatAppearanceLength;
        string timestamp = GetTimestamp();
        stringBuilder.AppendLine($"{timestamp}: {chatMessageText}");
        chatMessageBox.text = stringBuilder.ToString();
    }


    private void OnInsanityCvarChanged(bool newValue)
    {
        string timestamp = GetTimestamp();
        string text = $"[SERVER] Cvar \"insanity\" was set to \"{newValue}\"";
        UpdateChatMessagesBox(text);
    }


    private void OnVoiceChatCvarChanged()
    {

    }


    private void OnLobbyPasswordCvarChanged()
    {
        string text = $"[SERVER] Cvar \"lobby_password\" has been changed.";
        UpdateChatMessagesBox(text);
    }


    private void OnLobbyNameCvarChanged(string newLobbyName)
    {
        string text = $"[SERVER] Cvar \"lobby_name\" has been changed to \"{newLobbyName}\"";
        UpdateChatMessagesBox(text);
    }

    private string GetTimestamp()
    {
        DateTime currentDateTime = DateTime.Now;
        return currentDateTime.ToString("HH:mm:ss");
    }
    
}
