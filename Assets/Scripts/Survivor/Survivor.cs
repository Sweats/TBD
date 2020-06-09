using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private PausedGameInput pausedGameInput;

    [SerializeField]
    private MovementInput movementInput;

    private Rect rect;

    [SerializeField]
    private Texture crosshair;

    private Transform position;

    [SerializeField]
    private Camera survivorCamera;

    private CharacterController controller;

    [SerializeField]
    private MouseInput mouseInput;

    void Start()
    {
        position = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();

        rect = new Rect(Screen.width / 2, Screen.height / 2, 2, 2);
    }

    void Update()
    {
        mouseInput.Handle(survivorCamera, flashlight.transform, position, pausedGameInput.gamePaused);
        movementInput.Handle(controller, pausedGameInput.gamePaused);
    }


    void OnGUI()
    {
        if (pausedGameInput.gamePaused)
        {
            return;
        }

        GUI.DrawTexture(rect, crosshair);
        
    }


}
