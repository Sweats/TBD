using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        EventManager.serverAskedYouToPickCharacterEvent.AddListener(OnServerAskedYouToPickCharacter);

        randomCharacterButton.image.sprite = emptyLobbySlotIcon;
        chadButton.image.sprite = emptyLobbySlotIcon;
        aliceButton.image.sprite = emptyLobbySlotIcon;
        jesusButton.image.sprite = emptyLobbySlotIcon;
        jamalButton.image.sprite = emptyLobbySlotIcon;
        lurkerButton.image.sprite = emptyLobbySlotIcon;
        phantomButton.image.sprite = emptyLobbySlotIcon;
        maryButton.image.sprite = emptyLobbySlotIcon;
        fallenButton.image.sprite = emptyLobbySlotIcon;
    }

    public void OnCharacterButtonClicked(int value)
    {
        Debug.Log($"You clicked on a button and the value is {value}");
        Character character = (Character)value;
        NetworkClient.Send(new ServerClientPickedCharacterMessage{pickedCharacter = character });

        Hide();
    }

    public void Show()
    {
        pickCharacterCanvas.enabled = true;
    }

    private void Hide()
    {
        pickCharacterCanvas.enabled = false;

    }

    private void OnServerAskedYouToPickCharacter(Character[] availableCharacters)
    {
        Debug.Log("Server asked you to pick a character!");

        for (var i = 0; i < availableCharacters.Length; i++)
        {
            Character character = availableCharacters[i];

            switch (character)
            {
                case Character.Alice:
                    aliceButton.image.sprite = aliceIcon;
                    aliceButton.interactable = true;
                    aliceButton.enabled = true;
                    break;
                case Character.Jesus:
                    jesusButton.image.sprite = jesusIcon;
                    jesusButton.interactable = true;
                    jesusButton.enabled = true;
                    break;
                case Character.Chad:
                    chadButton.image.sprite = chadIcon;
                    chadButton.interactable = true;
                    chadButton.enabled = true;
                    break;
                case Character.Jamal:
                    jamalButton.image.sprite = jamalIcon;
                    jamalButton.interactable = true;
                    jamalButton.enabled = true;
                    break;
                case Character.Lurker:
                    lurkerButton.image.sprite  = lurkerIcon;
                    lurkerButton.interactable = true;
                    lurkerButton.enabled = true;
                    break;
                case Character.Phantom:
                    phantomButton.image.sprite = phantomIcon;
                    phantomButton.interactable = true;
                    phantomButton.enabled = true;
                    break;
                case Character.Mary:
                    maryButton.image.sprite = maryIcon;
                    maryButton.interactable = true;
                    maryButton.enabled = true;
                    break;
                case Character.Fallen:
                    fallenButton.image.sprite = fallenIcon;
                    fallenButton.interactable = true;
                    fallenButton.enabled = true;
                    break;
                case Character.Random:
                    randomCharacterButton.image.sprite = randomCharacterIcon;
                    randomCharacterButton.interactable = true;
                    randomCharacterButton.enabled = true;
                    break;
            }
        }

        Show();
    }

}
