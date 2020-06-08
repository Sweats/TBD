using System;
//using System.Numerics;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    [SerializeField]
    private Transform playerBody;

    [SerializeField]
    private Texture dot;
    [SerializeField]
    private Camera survivorCamera;

    [SerializeField]
    private Flashlight flashlight;

    [SerializeField]
    private float pickupDistance;

    [SerializeField]
    private Insanity insanity;

    [SerializeField]
    private PauseUI pauseUI;

    [SerializeField]
    private float minimumX;
    [SerializeField]
    private float maximumX;


    public static int invertX;
    public static int invertY;
    public static float mouseSensitivity;
    //public static float sensitivityX;
    //public static float sensitivityY;

    //private float xRotation;

    private Rect rect;

    //public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    //public RotationAxes axes = RotationAxes.MouseXAndY;

    private float xRotation;

    private readonly string[] names = new string[]
    {
        "key",
        "battery",
        "door"
    };

    void Start()
    {
        rect = new Rect(survivorCamera.pixelWidth / 2, survivorCamera.pixelHeight / 2, 2, 2);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (pauseUI.gamePaused)
        {
            return;
        }


        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (invertX == 1)
        {
            mouseX *= -1;
        }

        if (invertY == 1)
        {
            mouseY *= -1;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minimumX, maximumX);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        flashlight.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
    private void OnGUI()
    {
        if (!pauseUI.gamePaused)
        {
            GUI.DrawTexture(rect, dot);
        }

    }
}
