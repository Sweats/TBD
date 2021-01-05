using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct KeyGUI
{
    public RawImage rustyKeyImage;
    public RawImage metalKeyImage;
    public RawImage oldKeyImage;
    public RawImage silverKeyImage;

    public RawImage hammerImage;
    public RawImage crowbarImage;
    public RawImage codeImage;
    public Text rustyKeyCountText;
    public Text metalKeyCountText;
    public Text oldKeyCountText;
    public Text silverKeyCountText;
    public Text hammerCountText;
    public Text crowbarCountText;

    public Text codeCountText;
}

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField]
    private Canvas playerStatsCanvas;

    [SerializeField]
    private KeyGUI[] keyGUI;

    private void Start()
    {
        this.enabled = false;
        EventManager.survivorPickedUpKeyEvent.AddListener(OnSurvivorPickedUpKey);
    }

    // TO DO: Make this a lot better?
    public void OnSurvivorPickedUpKey(Survivor survivor, Key key)
    {
        int id = survivor.survivorID;
        int keyType = (int)key.Type();
        Key[] keys = survivor.inventory.Keys();
        int rustyKeyCount = 0, metalKeyCount = 0, oldKeyCount = 0, silverKeyCount = 0;
        int crowbarCount = 0, hammerCount = 0, codeCount = 0;

        for (var i = 0; i < keys.Length; i++)
        {
            Key currentKey = keys[i];
            int type = (int)currentKey.Type();

            switch (keyType)
            {
                case (int)KeyType.Rusty:
                    rustyKeyCount++;
                    break;
                case (int)KeyType.Old:
                    oldKeyCount++;
                    break;
                case (int)KeyType.Silver:
                    silverKeyCount++;
                    break;
                case (int)KeyType.Metal:
                    metalKeyCount++;
                    break;
                case (int)KeyType.Crowbar:
                    crowbarCount++;
                    break;
                case (int)KeyType.Hammer:
                    hammerCount++;
                    break;
                case (int)KeyType.Code:
                    codeCount++;
                    break;
            }
        }

        if (rustyKeyCount > 0)
        {
            keyGUI[id].rustyKeyImage.enabled = true;
            keyGUI[id].rustyKeyCountText.text = $"{rustyKeyCount}";
            keyGUI[id].rustyKeyCountText.enabled = true;
        }


        if (metalKeyCount > 0)
        {
            keyGUI[id].metalKeyImage.enabled = true;
            keyGUI[id].metalKeyCountText.text = $"{metalKeyCount}";
            keyGUI[id].metalKeyCountText.enabled = true;
        }


        if (oldKeyCount > 0)
        {
            keyGUI[id].oldKeyImage.enabled = true;
            keyGUI[id].oldKeyCountText.text = $"{oldKeyCount}";
            keyGUI[id].oldKeyCountText.enabled = true;
        }

        if (silverKeyCount > 0)
        {
            keyGUI[id].silverKeyImage.enabled = true;
            keyGUI[id].silverKeyCountText.text = $"{silverKeyCount}";
            keyGUI[id].silverKeyCountText.enabled = true;
        }

        if (codeCount > 0)
        {
            keyGUI[id].codeImage.enabled = true;
            keyGUI[id].codeCountText.text = $"{codeCount}";
            keyGUI[id].codeCountText.enabled = true;
        }

        if (hammerCount > 0)
        {
            keyGUI[id].hammerImage.enabled = true;
            keyGUI[id].hammerCountText.text = $"{hammerCount}";
            keyGUI[id].hammerCountText.enabled = true;
        }

        if (crowbarCount > 0)
        {
            keyGUI[id].crowbarImage.enabled = true;
            keyGUI[id].crowbarCountText.text = $"{crowbarCount}";
            keyGUI[id].crowbarCountText.enabled = true;
        }
    }


    public void Show()
    {
        this.enabled = true;
        playerStatsCanvas.enabled = true;
    }


    public void Hide()
    {
        this.enabled = false;
        playerStatsCanvas.enabled = false;
    }
}
