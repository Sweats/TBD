using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Chat : MonoBehaviour
{

    [SerializeField]
    private float chatAppearanceLength;

    [SerializeField]
    private int amountOfChatMessages;

    [SerializeField]
    private Canvas chatCanvas;
    private List<ChatMessage> chatMessages;

    [SerializeField]
    private InputField chatMessageBox;

    private bool isChatOpened;

    private string messageText;


    void Start()
    {
        chatMessages = new List<ChatMessage>();
        chatMessageBox.text = string.Empty;
        EventManager.survivorClosedChatEvent.AddListener(OnSurvivorClosedChat);
        EventManager.survivorOpenedChatEvent.AddListener(OnSurvivorOpenedChat);
    }

    void Update()
    {
        if (!isChatOpened)
        {
            if (chatCanvas.enabled)
            {
                chatAppearanceLength -= Time.deltaTime;

                if (chatAppearanceLength <= 0)
                {
                    Hide();
                }
            }
        }
    }

    private void OnSurvivorOpenedChat()
    {
        Show();
        isChatOpened = true;
        chatMessageBox.enabled = true;
        chatMessageBox.Select();
        chatMessageBox.ActivateInputField();
    }

    private void OnSurvivorClosedChat()
    {
        isChatOpened = false;
        UnselectChatMessageBox();
        chatMessageBox.enabled = false;
        chatMessageBox.text = string.Empty;
    }

    public void OnSurvivorSendChat()
    {
        if (chatMessageBox.text != string.Empty && isChatOpened)
        {
            ChatMessage chatMessage = new ChatMessage()
            {
                // TO DO: Figure out the best way to get the player name. Maybe use a global variable?
                survivorName = "player",
                text = chatMessageBox.text,
                // TO DO: Figure out how we can get the survivor ID in the lobby.
                survivorID = 0
            };

            chatMessages.Add(chatMessage);
            chatMessageBox.text = string.Empty;
        }
    }

    private void UnselectChatMessageBox()
    {
        chatMessageBox.OnDeselect(new BaseEventData(EventSystem.current));
    }


    private void OnSurviorRecievedChat(Survivor who, string messageText)
    {
        ChatMessage chatMessage = new ChatMessage
        {
            survivorName = who.survivorName,
            survivorID = who.survivorID,
            text = $"{who.survivorName}: {messageText}"
        };

        chatMessages.Add(chatMessage);
        Show();
    }


    private void Show()
    {
        chatCanvas.enabled = true;

    }

    private void Hide()
    {
        chatCanvas.enabled = false;
    }
}
