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


    [SerializeField]
    private Windows window;

    // Start is called before the first frame update
    private void Start()
    {
        this.enabled = false;
        stringBuilder = new StringBuilder();
        EventManager.monsterWonEvent.AddListener(OnMonsterWon);
        EventManager.playerConnectedEvent.AddListener(OnPlayerConnected);
        EventManager.playerDisconnectedEvent.AddListener(OnPlayerDisconnected);
        EventManager.survivorDeathEvent.AddListener(OnSurvivorDeath);
        EventManager.playerSentChatMessageEvent.AddListener(OnPlayerSentChatMessage);
        EventManager.playerRecievedChatMessageEvent.AddListener(OnPlayerRecievedChatMessage);
        EventManager.survivorsEscapedStageEvent.AddListener(OnSurvivorsEscapedStage);
        EventManager.failedToLoadStageEvent.AddListener(OnFailedToLoadStage);
        EventManager.playerPickedUpKeyEvent.AddListener(OnSurvivorPickedUpKey);
        EventManager.survivorUnlockDoorEvent.AddListener(OnSurvivorUnlockedDoor);
        EventManager.playerChangedNameEvent.AddListener(OnPlayerChangedProfileName);
    }


    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            Hide();
        }

    }

    public void Show()
    {
        consoleCanvas.enabled = true;
        consoleInputTextField.Select();
        consoleInputTextField.text = string.Empty;
        this.enabled = true;
        window.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Hide()
    {
        this.enabled = false;
        consoleCanvas.enabled = false;
        window.MarkConsoleWindowClosed();
        window.enabled = true;
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


    private void OnSurvivorDeath(string playerName)
    {
        stringBuilder.AppendLine();
        string text = string.Empty;
        UpdateConsoleText(text);
    }


    private void OnPlayerConnected(string playerName)
    {
        string text = $"Player {playerName} has connected!";
        UpdateConsoleText(text);
    }

    private void OnPlayerDisconnected(string playerName)
    {
        string text = $"Player {playerName} has disconnected!";
        UpdateConsoleText(text);
    }

    private void OnSurvivorUnlockedDoor(string survivorName, string doorName, string keyName)
    {
        string text = $"Player {survivorName} used a {keyName} to unlock a {doorName}!";
        UpdateConsoleText(text);
    }

    public void UpdateConsoleText(string newText)
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


    private void OnSurvivorPickedUpKey(string survivorName, string keyName)
    {
        string text = $"Player {survivorName} picked up a {keyName}!";
        UpdateConsoleText(text);

    }

    private string GetTimeStamp()
    {
        DateTime currentDateTime = DateTime.Now;
        return currentDateTime.ToString("HH:mm:ss");
    }

    private void OnPlayerRecievedChatMessage(string playerName, string message)
    {
        string text = $"{playerName}: {message}";
        UpdateConsoleText(text);
    }

    private void OnPlayerSentChatMessage(string playerName, string message)
    {
        string text = $"{playerName}: {message}";
        UpdateConsoleText(text);
    }

    private void OnPlayerChangedProfileName(string oldName, string newName)
    {
        string text = $"[Server]: {oldName} changed their name to {newName}.";
        UpdateConsoleText(text);
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
}

