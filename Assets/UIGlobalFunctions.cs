using UnityEngine;

public class UIGlobalFunctions : MonoBehaviour
{
    // Prints a message at the top left hand corner.
    // Can only use this inside a OnGUI function.

    [SerializeField]
    private static Camera survivorCamera;
    public static void PrintText(string text)
    {
        GUIStyle style = new GUIStyle();
        Rect position = new Rect(0, 0, 50, 50);
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 12;
        GUI.Label(position, text, style);

    }

}
