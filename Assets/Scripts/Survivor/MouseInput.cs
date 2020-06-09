using System;
//using System.Numerics;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    [SerializeField]
    private float minimumX;
    [SerializeField]
    private float maximumX;


    public static int invertX;
    public static int invertY;
    public static float mouseSensitivity;
    private float xRotation;

    public void Handle(Camera camera, Transform flashlight, Transform survivor, bool gamePaused)
    {
        if (gamePaused)
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
        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        flashlight.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        survivor.localRotation = Quaternion.Euler(xRotation, 0, 0);
        survivor.Rotate(Vector3.up * mouseX);
    }
}
