﻿using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<Key> keys;
    private Rect currentPosition;


    void Start()
    {
        // TO DO: Get the size of the Keylist and 
        keys = new List<Key>();

        currentPosition = new Rect
        {
            height = 50,
            width = 50
        };


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Add(Key key)
    {
        keys.Add(key);
    }

    public bool HasKey(Key key)
    {
        bool found = false;

        for (var i = 0; i < keys.Count; i++)
        {
            if (keys[i].ID() == key.ID())
            {
                found = true;
                break;
            }
        }

        return found;
    }

    public Key[] Keys()
    {
        return keys.ToArray();
    }

    public void Clear()
    {
        keys.Clear();
    }

    public void Remove(Key key)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].keyMask == key.keyMask)
            {
                keys.RemoveAt(i);
                break;
            }
        }
        
    }

    public void Draw()
    {
        int currentX = Screen.width - 20;
        int currentY = Screen.height - 20;

        for (var i = 0; i < keys.Count; i++)
        {
            Texture itemIcon = keys[i].iconTexture;

            if (i % 8 == 0)
            {
                // go back to the top where we were when we started and then go left 50 because images will be 50 in pixels
                currentY = Screen.height - 20;
                currentX -= 50;
            }

            currentPosition.x = currentX;
            currentPosition.y = currentY;
            GUI.DrawTexture(currentPosition, itemIcon);
            currentY = -50;

        }

    }
}
