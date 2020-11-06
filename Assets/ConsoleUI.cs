using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System;

public class ConsoleUI : MonoBehaviour
{
    private StringBuilder stringBuilder;

    [SerializeField]
    private Canvas consoleCanvas;

    [SerializeField]
    private InputField consoleTextField;

    [SerializeField]
    private InputField consoleInputTextField;

    public static bool OPENED;

    private bool pauseWindowOpen;

    [SerializeField]
    private PausedGameInput pausedGameInput;


    // Start is called before the first frame update
    void Start()
    {
        stringBuilder = new StringBuilder();
        EventManager.monsterWonEvent.AddListener(OnMonsterWon);
        EventManager.playerConnectedEvent.AddListener(OnPlayerConnected);
        EventManager.playerDisconnectedEvent.AddListener(OnPlayerDisconnected);
        EventManager.survivorDeathEvent.AddListener(OnSurvivorDeath);
        EventManager.survivorSendChatMessageEvent.AddListener(OnPlayerSentChatEvent);
        EventManager.survivorsEscapedStageEvent.AddListener(OnSurvivorsEscapedStage);
        EventManager.failedToLoadStageEvent.AddListener(OnFailedToLoadStage);
        EventManager.survivorPickedUpKeyEvent.AddListener(OnSurvivorPickedUpKey);
        EventManager.survivorUnlockDoorEvent.AddListener(OnSurvivorUnlockedDoor);
    }


    void Update()
    {
        if (PausedGameInput.GAME_PAUSED)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.BackQuote) && !OPENED)
        {
            OPENED = true;
            Show();
        }

        else if (Input.GetKeyDown(KeyCode.BackQuote) && OPENED)
        {
            OPENED = false;
            Hide();
        }
    }

    private void Show()
    {
        OPENED = true;
        consoleCanvas.enabled = true;
        consoleInputTextField.Select();
        consoleInputTextField.text = string.Empty;
        Cursor.lockState = CursorLockMode.Confined;
        EventManager.playerOpenedConsoleEvent.Invoke();
    }


    private void Hide()
    {
        OPENED = false;
        consoleCanvas.enabled = false;
        EventManager.playerClosedConsoleEvent.Invoke();
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void OnSurvivorsEscapedStage()
    {
        string text = "The survivors escaped.";
        UpdateConsoleText(text);
    }


    private void OnMonsterWon()
    {
        string text = "The monster won!";
        UpdateConsoleText(text);
    }


    private void OnSurvivorDeath(Survivor survivor)
    {
        float x, y, z;
        x = survivor.transform.position.x;
        y = survivor.transform.position.y;
        z = survivor.transform.position.z;
        stringBuilder.AppendLine();
        string text = $"Survivor \"{survivor.name}\" died at {x} {y} {z}!";
        UpdateConsoleText(text);
    }


    private void OnPlayerConnected(Survivor player)
    {
        string text = $"Player \"{player.survivorName}\" has connected!";
        UpdateConsoleText(text);

    }


    private void OnPlayerDisconnected(Survivor player)
    {
        string text = $"Player \"{player.survivorName}\" has disconnected!";
        UpdateConsoleText(text);

    }


    private void OnSurvivorUnlockedDoor(Survivor survivor, Key key, Door door)
    {
        float x, y, z;
        x = survivor.transform.position.x;
        y = survivor.transform.position.y;
        z = survivor.transform.position.z;
        string text = $"Survivor \"{survivor.survivorName}\" has unlocked a door named \"{door.doorName}\" using an item named \"{key.Name()}\" at {x} {y} {z}!";
        UpdateConsoleText(text);
    }


    private void OnPlayerSentChatEvent(ChatMessage chatMessage)
    {
        string message = $"{chatMessage.survivorName}: {chatMessage.text}";
        UpdateConsoleText(message);

    }


    private void UpdateConsoleText(string newText)
    {
        string timestamp = GetTimeStamp();
        stringBuilder.AppendLine($"{timestamp}: {newText}");
        consoleTextField.text = stringBuilder.ToString();
    }

    private void OnFailedToLoadStage(string stageName)
    {
        string text = $"ERROR: Failed to load stage name \"{stageName}\"! Please let us know in the Discord!";
        UpdateConsoleText(text);
    }


    private void OnSurvivorPickedUpKey(Survivor survivor, Key key)
    {
        string keyName = key.Name();
        float x, y, z;
        x = survivor.transform.position.x;
        y = survivor.transform.position.y;
        z = survivor.transform.position.z;
        string survivorName = survivor.survivorName;
        string timestamp = GetTimeStamp();
        string keyTypeName = Enum.GetName(typeof(KeyType), key.Type());
        string text = $"Survivor \"{survivorName}\" picked up an item named \"{key}\" of type \"{keyTypeName}\" at {x} {y} {z}";
        UpdateConsoleText(text);

    }

    private string GetTimeStamp()
    {
        DateTime currentDateTime = DateTime.Now;
        return currentDateTime.ToString("HH:mm:ss");
    }


    public void OnCommandEntered(string command)
    {
        string[] commands = command.Split(new char[] {'_'}, 2);

        if (commands.Length > 0)
        {
            if (String.Equals(commands[0], "darned", StringComparison.OrdinalIgnoreCase))
            {
                string darnedCommand = commands[1];

                if (String.Equals(darnedCommand, "test", StringComparison.OrdinalIgnoreCase))
                {
                    string text = "TEST";
                    UpdateConsoleText(text);
                }
            }
        }

        consoleInputTextField.text = string.Empty;
        //consoleInputTextField.Select();
    }

    private void OnPlayerClosedPauseMenu()
    {
        pauseWindowOpen = false;
    }

    private void OnPlayerOpenedPauseMenu()
    {
        pauseWindowOpen = true;

    }
}
