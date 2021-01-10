using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<Key> keys;

    private Rect currentPosition;

    private Canvas inventoryCanvas;

    private void Start()
    {
        // TO DO: Get the size of the Keylist and 
        keys = new List<Key>();

        currentPosition = new Rect
        {
            height = 30,
            width = 30
        };

    }

    public void Add(Key key, Texture keyIcon)
    {
        keys.Add(key);
    }

    public void Add(Key key)
    {
        keys.Add(key);
    }

    public Key[] Keys()
    {
        return keys.ToArray();
    }

    public void Clear()
    {
        keys.Clear();
    }

    public void Draw()
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
