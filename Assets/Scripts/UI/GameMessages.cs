using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameMessages : MonoBehaviour
{

    private List<GameMessage> messages;

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
        messages = new List<GameMessage>();
    }

    void Update()
    {
        if (gameMessagesCanvas.enabled)
        {
            bool expiredMessageFound = false;

            for (var i = 0; i < messages.Count; i++)
            {
                if (messages[i].length <= 0)
                {
                    expiredMessageFound = true;
                    messages.RemoveAt(i);
                }

                else
                {
                    messages[i].length -= Time.deltaTime;
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

        for (var i = 0; i < messages.Count; i++)
        {
            stringBuilder.AppendLine(messages[i].message);
        }

        messagesBox.text = stringBuilder.ToString();
    }

    private void Add(GameMessage message)
    {
        gameMessagesCanvas.enabled = true;
        this.messages.Add(message);
    }
    public void OnSurvivorGrabbedKey(Survivor survivor, KeyObject key)
    {
        GameMessage newMessage = new GameMessage()
        {
            message = $"{survivor.name} picked up a {key.key.name}!",
            id = nextMessageID,
            length = chatMessageAppearanceLength
        };

        messages.Add(newMessage);
    }

    public void OnSurvivorUnlockedDoor(Survivor survivor, Key key, Door door)
    {
        GameMessage newMessage = new GameMessage()
        {
            message = $"{survivor.name} used a {key.name} to unlock a {door.name}!",
            id = nextMessageID,
            length = chatMessageAppearanceLength
        };

        messages.Add(newMessage);

    }

    public void OnSurvivorDeath(Survivor survivor)
    {
        GameMessage newMessage = new GameMessage()
        {
            message = $"{survivor.name} died!",
            id = nextMessageID,
            length = chatMessageAppearanceLength
        };

        messages.Add(newMessage);

    }


    public void OnFailedToPickUpBatteryEvent()
    {
        GameMessage newMessage = new GameMessage()
        {
            message = "Your flashlight is already charged!",
            id = nextMessageID,
            length = chatMessageAppearanceLength 
        };

        messages.Add(newMessage);
    }

    public void OnPickedUpBatteryEvent()
    {
        GameMessage newMessage = new GameMessage()
        {
            message = "You picked up a battery!",
            id = nextMessageID,
            length = chatMessageAppearanceLength
        };

        messages.Add(newMessage);
    }


    public void OnPickupKeySurvivorAlreadyHasEvent()
    {
        GameMessage newMesasge = new GameMessage()
        {
            message = "You already have this key!",
            id = nextMessageID,
            length = chatMessageAppearanceLength
        };


        messages.Add(newMesasge);
    }

    public void OnSurvivorDisconnect(Survivor suvivor)
    {

    }

    public void OnSurvivorConnect(Survivor survivor)
    {

    }

    public void OnHostStartedTheGame()
    {

    }

}