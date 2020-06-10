using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameMessage : MonoBehaviour
{
    public string message;
    public int id;
    private bool shouldDelete;
    public float length;

    public GameMessage(string message, int id, float length)
    {
        this.message = message;
        this.id = id;
        this.length = length;

    }

    void Update()
    {
        length -= Time.deltaTime;

        if (length < 0)
        {
            shouldDelete = true;
        }

    }


    public bool Expired()
    {
        return shouldDelete;
    }
}


