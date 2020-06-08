using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<Key> keys;


    private Rect startingPosition;

    void Start()
    {
        // TO DO: Get the size of the Keylist and 
        keys = new List<Key>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Add(Key key)
    {
        keys.Add(key);
    }

    public void Clear()
    {
        keys.Clear();
    }


    void OnGUI()
    {
        for (int i = 0; i < keys.Count; i++)
        {
            Image icon = keys[i].Icon;
            // TO DO. Render the icons for the picked up keys in the top right hand corner and move position
            // depending on how many keys we have picked up.
        }
        
    }
}
