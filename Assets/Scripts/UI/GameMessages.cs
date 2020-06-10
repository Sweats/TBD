using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameMessages : MonoBehaviour
{

    private List<GameMessage> message;

    // How long should a chat message appear for? 
    [SerializeField]
    private float chatMessageAppearanceLength;

    [SerializeField]
    private InputField messagesBox;

    [SerializeField]
    private Canvas gameMessagesCanvas;

    private int nextMessageID;

    private StringBuilder stringBuilder;

    // Start is called before the first frame update
    void Start()
    {
        stringBuilder = new StringBuilder();
        message = new List<GameMessage>();
    }


    public void HandleChatMessages()
    {
        if (gameMessagesCanvas.enabled)
        {
            bool expiredMessageFound = false;

            for (var i = 0; i < message.Count; i++)
            {
                if (message[i].Expired())
                {
                    expiredMessageFound = true;
                    message.RemoveAt(i);
                }
            }

            UpdateChatText();

            if (!expiredMessageFound)
            {
                Hide();
                nextMessageID = 0;
            }
        }
    }


    public void Hide()
    {
        gameMessagesCanvas.enabled = false;
    }

    public void Show()
    {
        gameMessagesCanvas.enabled = true;
    }

    private void UpdateChatText()
    {
        stringBuilder.Clear();

        for (var i = 0; i < message.Count; i++)
        {
            stringBuilder.AppendLine(message[i].message);
        }

        messagesBox.text = stringBuilder.ToString();
    }

    public void Add(GameMessage message)
    {
        gameMessagesCanvas.enabled = true;
        message.id = nextMessageID;
        this.message.Add(message);
    }
}