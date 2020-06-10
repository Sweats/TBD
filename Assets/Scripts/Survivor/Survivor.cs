using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    [SerializeField]
    private Texture crosshair;

    private Transform position;

    [SerializeField]
    private Camera survivorCamera;

    private CharacterController controller;

    [SerializeField]
    private int defaultSpeed;

    [SerializeField]
    private int sprintSpeed;

    [SerializeField]
    private int walkSpeed;

    [SerializeField]
    private int crouchSpeed;

    [SerializeField]
    private MouseInput mouseInput;
    [SerializeField]
    private GameMessages survivorChat;


    private bool crouched;
    private bool walking;

    private Rect rect;


    void Start()
    {
        position = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();

        rect = new Rect(Screen.width / 2, Screen.height / 2, 2, 2);

    }

    // We will try to handle as much input as possible here. If not that is okay I think.
    void Update()
    {
        mouseInput.Handle(survivorCamera, flashlight.transform, position, pausedGameInput.gamePaused);
        movementInput.Handle(controller, pausedGameInput.gamePaused);

        if (pausedGameInput.gamePaused)
        {
            return;
        }

        if (sprint.sprinting)
        {
            movementInput.speed = sprint.sprintSpeed;
        }

        else
        {
            movementInput.speed = defaultSpeed;
        }

        if (Keybinds.GetKey(Action.Grab))
        {

        }

        else if (Keybinds.GetKey(Action.SwitchFlashlight))
        {
            flashlight.Toggle();
        }


        else if (Keybinds.GetKey(Action.Crouch))
        {
            movementInput.speed = crouchSpeed;
            crouched = true;

        }

        else if (Keybinds.GetKey(Action.Crouch, true))
        {
            movementInput.speed = defaultSpeed;
            crouched = false;
        }

        else if (Keybinds.GetKey(Action.Walk))
        {
            movementInput.speed = crouchSpeed;
            walking = true;

        }

        else if (Keybinds.GetKey(Action.Walk, true))
        {
            movementInput.speed = defaultSpeed;
            walking = false;
        }

    }

    void OnGUI()
    {
        if (pausedGameInput.gamePaused)
        {
            return;
        }

        GUI.DrawTexture(rect, crosshair);
        inventory.Draw();

        if (walking)
        {

            // TO DO. Find a crouching walking icon and draw it here.
        }

        if (crouched)
        {

            // TO DO. Find a crouching icon and draw it here.
        }
        
    }


    private void HandleGrab()
    {

    }
}
