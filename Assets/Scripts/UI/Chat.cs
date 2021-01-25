using System.Text;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Chat : MonoBehaviour
{

    [SerializeField]
    private int chatAppearanceLength;

    private int chatAppearanceLengthTimer;

    [SerializeField]
    private Canvas chatCanvas;

    [SerializeField]
    private InputField chatMessageBoxInput;

    [SerializeField]
    private TMP_InputField chatMessageBox;

    [SerializeField]
    private Windows window;

    private string messageText;

    private StringBuilder stringBuilder;


    private void Start()
    {
        chatAppearanceLengthTimer = chatAppearanceLength;
        this.enabled = false;
        stringBuilder = new StringBuilder();
        chatMessageBoxInput.text = string.Empty;
        EventManager.playerRecievedChatMessageEvent.AddListener(OnPlayerRecievedChatMessage);
        EventManager.playerChangedNameEvent.AddListener(OnPlayerChangedProfileNameEvent);
        StartCoroutine(ChatRoutine());
    }

    private IEnumerator ChatRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (chatAppearanceLengthTimer > 0)
            {
                chatAppearanceLengthTimer--;

                if (chatAppearanceLengthTimer <= 0)
                {
                    chatCanvas.enabled = false;
                }
            }
        }
    }

    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            DeselectChatInput();
        }

        // TODO: Change this keybinding to something else. This is just for testing.
        else if (Input.GetKey(KeyCode.Home))
        {
            string text = chatMessageBoxInput.text;

            if (text != string.Empty)
            {
                string playerName = Settings.PROFILE_NAME;
                string message = chatMessageBoxInput.text;
                chatMessageBoxInput.text = string.Empty;
                chatMessageBoxInput.Select();
                EventManager.playerSentChatMessageEvent.Invoke(playerName, message);
            }
        }
    }

    public void Show()
    {
        Cursor.lockState = CursorLockMode.Confined;
        this.enabled = true;
        chatCanvas.enabled = true;
        chatMessageBoxInput.interactable = true;
        chatMessageBoxInput.Select();
    }

    private void DeselectChatInput()
    {
        UnselectChatMessageBox();
        chatMessageBoxInput.interactable = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.enabled = false;
        window.enabled = true;
        chatMessageBoxInput.text = string.Empty;
        chatAppearanceLengthTimer = chatAppearanceLength;
        window.MarkChatWindowClosed();
    }

    private void UnselectChatMessageBox()
    {
        chatMessageBoxInput.OnDeselect(new BaseEventData(EventSystem.current));
    }

    private void UpdateChatMessagesBox(string chatMessageText)
    {
        chatAppearanceLengthTimer = chatAppearanceLength;
        chatCanvas.enabled = true;
        stringBuilder.AppendLine($"{chatMessageText}");
        chatMessageBox.text = stringBuilder.ToString();
    }


    private void OnInsanityCvarChanged(bool newValue)
    {
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

    private void OnPlayerRecievedChatMessage(string playerName, string message)
    {
        string text = $"{playerName}: {message}";
        UpdateChatMessagesBox(text);
    }


    private void OnPlayerChangedProfileNameEvent(string oldName, string newName)
    {
        string text = $"[Server]: {oldName} changed their name to {newName}.";
        UpdateChatMessagesBox(text);
    }
}
