using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PickCharacterUI : MonoBehaviour
{
    [SerializeField]
    private Sprite aliceIcon;

    [SerializeField]
    private Sprite chadIcon;

    [SerializeField]
    private Sprite jesusIcon;

    [SerializeField]
    private Sprite jamalIcon;

    [SerializeField]
    private Sprite randomCharacterIcon;

    [SerializeField]
    private Sprite emptyLobbySlotIcon;

    [SerializeField]
    private Sprite lurkerIcon;

    [SerializeField]
    private Sprite phantomIcon;

    [SerializeField]
    private Sprite maryIcon;

    [SerializeField]
    private Sprite fallenIcon;

    [SerializeField]
    private Sprite hoveredOverCharacterSprite;

    [SerializeField]
    private Button chadButton;

    [SerializeField]
    private Button aliceButton;

    [SerializeField]
    private Button jesusButton;

    [SerializeField]
    private Button jamalButton;

    [SerializeField]
    private Button lurkerButton;

    [SerializeField]
    private Button phantomButton;

    [SerializeField]
    private Button maryButton;

    [SerializeField]
    private Button fallenButton;

    [SerializeField]
    private Button randomCharacterButton;

    [SerializeField]
    private Canvas pickCharacterCanvas;

    public void OnCharacterButtonClicked(int value)
    {
        Character character = (Character)value;
        NetworkClient.Send(new ServerClientGamePlayerPickedCharacterMessage{pickedCharacter = character });
        Hide();
    }

    public void LocalPlayerStart()
    {
        EventManager.clientServerGameAskedYouToPickCharacterEvent.AddListener(OnServerAskedYouToPickCharacter);
    }

    public void Show()
    {
        pickCharacterCanvas.enabled = true;
    }

    private void Hide()
    {
        pickCharacterCanvas.enabled = false;

    }

    public void OnServerAskedYouToPickCharacter(Character[] unavailableCharacters)
    {
        int count = 0;

        for (var i = 0; i < unavailableCharacters.Length; i++)
        {
            Character character = unavailableCharacters[i];

            switch (character)
            {
                case Character.Karen:
                    aliceButton.image.sprite = emptyLobbySlotIcon;
                    aliceButton.interactable = false;
                    aliceButton.enabled = false;
                    count++;
                    break;
                case Character.Jesus:
                    jesusButton.image.sprite = emptyLobbySlotIcon;
                    jesusButton.interactable = false;
                    jesusButton.enabled = false;
                    count++;
                    break;
                case Character.Chad:
                    chadButton.image.sprite = emptyLobbySlotIcon;
                    chadButton.interactable = false;
                    chadButton.enabled = false;
                    count++;
                    break;
                case Character.Jamal:
                    jamalButton.image.sprite = emptyLobbySlotIcon;
                    jamalButton.interactable = false;
                    jamalButton.enabled = false;
                    count++;
                    break;
                case Character.Lurker:
                    lurkerButton.image.sprite  = emptyLobbySlotIcon;
                    lurkerButton.interactable = false;
                    lurkerButton.enabled = false;
                    count++;
                    break;
                case Character.Phantom:
                    phantomButton.image.sprite = emptyLobbySlotIcon;
                    phantomButton.interactable = false;
                    phantomButton.enabled = false;
                    count++;
                    break;
                case Character.Mary:
                    maryButton.image.sprite = emptyLobbySlotIcon;
                    maryButton.interactable = false;
                    maryButton.enabled = false;
                    count++;
                    break;
                case Character.Fallen:
                    fallenButton.image.sprite = emptyLobbySlotIcon;
                    fallenButton.interactable = false;
                    fallenButton.enabled = false;
                    count++;
                    break;
            }
        }

        if (count == 8)
        {
            randomCharacterButton.interactable = false;
            randomCharacterButton.image.sprite = emptyLobbySlotIcon;
            randomCharacterButton.enabled = false; 
        }

        Show();
    }

}
