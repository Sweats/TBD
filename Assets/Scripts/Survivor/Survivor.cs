using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : MonoBehaviour
{
    [SerializeField]
    private string name = "player";
    [SerializeField]
    private Insanity insanity;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Flashlight flashlight;
    [SerializeField]
    private Sprint sprint;

    [SerializeField]
    private float speed;

    void Start()
    {

    }


}
