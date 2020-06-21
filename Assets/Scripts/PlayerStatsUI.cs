﻿using System.Collections;
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
    public void OnSurvivorOpenPlayerStats()
    {
        Show();
    }

    public void OnSurvivorClosePlayerStats()
    {
        Hide();
    }


    // TO DO: Make this a lot better?
    public void OnSurvivorPickedUpKey(Survivor survivor, KeyObject key)
    {
        int id = survivor.survivorID;
        int keyType = (int)key.key.type;
        Key[] keys = survivor.inventory.Keys();
        int rustyKeyCount = 0, metalKeyCount = 0, oldKeyCount = 0, silverKeyCount = 0;
        int crowbarCount = 0, hammerCount = 0, codeCount = 0;

        for (var i = 0; i < keys.Length; i++)
        {
            Key currentKey = keys[i];
            int type = (int)currentKey.type;

            switch (keyType)
            {
                case (int)Key.KeyType.Rusty:
                rustyKeyCount++;
                break;
                case (int)Key.KeyType.Old:
                oldKeyCount++;
                break;
                case (int)Key.KeyType.Silver:
                silverKeyCount++;
                break;
                case (int)Key.KeyType.Metal:
                metalKeyCount++;
                break;
                case (int)Key.KeyType.Crowbar:
                crowbarCount++;
                break;
                case (int)Key.KeyType.Hammer:
                hammerCount++;
                break;
                case (int)Key.KeyType.Code:
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


    private void Show()
    {
        playerStatsCanvas.enabled = true;

    }


    private void Hide()
    {
        playerStatsCanvas.enabled = false;

    }
}