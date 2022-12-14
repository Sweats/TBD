using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private List<Key> keys;

    private Rect currentPosition;

    public void ServerInit()
    {
        keys = new List<Key>();
    }

    public void ServerAdd(Key key)
    {
        keys.Add(key);
    }

    public Key[] ServerKeys()
    {
        return keys.ToArray();
    }

    public void ClientDraw()
    {
        int currentX = Screen.width - 20;
        int currentY = 20;

        for (var i = 0; i < keys.Count; i++)
        {
            //Texture itemIcon = keys[i].Texture();

            if (i % 8 == 0)
            {
                // go back to the top where we were when we started and then go left 50 because images will be 50 in pixels
                currentX  -= 50;
                currentY = 20;
            }

            currentPosition.x = currentX;
            currentPosition.y = currentY;
            //GUI.DrawTexture(currentPosition, itemIcon);
            currentY += 35;

        }
    }
}
