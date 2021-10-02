using UnityEngine;
using UnityEngine.EventSystems;

public class Windows : MonoBehaviour
{
    [SerializeField]
    private PauseUI pauseUI;

    [SerializeField]
    private ConsoleUI consoleUI;

    [SerializeField]
    private Chat chatUI;

    [SerializeField]
    private GameMessages gameMessagesUI;

    [SerializeField]
    private PlayerStatsUI playerStatsUI;

    [SerializeField]
    private GameOverUI gameOverUI;

    [SerializeField]
    private SoundUI soundUI;

    [SerializeField]
    private Texture crosshair;

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private StandaloneInputModule inputModule;

    private bool consoleWindowOpened;

    private bool pauseWindowOpened;

    private bool chatWindowOpened;

    private bool playerStatsWindowOpened;

    // NOTE: We made this instead of using update because if multiple players have this script enabled, then the menu's get messed up. We only want this to tick for the local player.
    public void Tick()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            this.enabled = false;
            pauseUI.Show();
            pauseWindowOpened = true;
        }

        // NOTE: Hardcoded it because it seems to be the standard in other games.
        else if (Input.GetKey(KeyCode.BackQuote))
        {
            this.enabled = false;
            consoleUI.Show();
            consoleWindowOpened = true;
        }


        //else if (Keybinds.GetKey(Action.Start))
        else if (Input.GetKey(KeyCode.End))
        {
            chatUI.Show();
            this.enabled = false;
            chatWindowOpened = true;

        }

        else if (Keybinds.GetKey(Action.PlayerStats) && !IsWindowOpen())
        {
            playerStatsWindowOpened = true;
            playerStatsUI.Show();
        }

        else if (Keybinds.GetKey(Action.PlayerStats, true) && playerStatsWindowOpened)
        {
            playerStatsUI.Hide();
            playerStatsWindowOpened = false;

        }
    }

    public void LocalPlayerStart()
    {
        gameMessagesUI.enabled = true;
        eventSystem.enabled = true;
        inputModule.enabled = true;
        gameMessagesUI.LocalPlayerStart();
        consoleUI.LocalPlayerStart();
        gameOverUI.LocalPlayerStart();
        soundUI.LocalPlayerStart();
    }

    private void OnGUI()
    {
        if (!this.enabled)
        {
            return;
        }

        // TO DO: Optimize this!
        GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 2, 2), crosshair);
    }


    public void MarkPauseWindowClosed()
    {
        pauseWindowOpened = false;
    }

    public void MarkConsoleWindowClosed()
    {
        consoleWindowOpened = false;

    }

    public void MarkChatWindowClosed()
    {
        chatWindowOpened = false;

    }

    public Chat ChatUI()
    {
        return chatUI;
    }

    public ConsoleUI ConsoleUI()
    {
        return consoleUI;
    }

    public bool IsWindowOpen()
    {
        return pauseWindowOpened || consoleWindowOpened ||  chatWindowOpened;
    }
}
