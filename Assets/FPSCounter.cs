using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public float FPS;

    [SerializeField]
    private Camera survivorCamera;

    //[SerializeField]
    // Update is called once per frame


    void Start()
    {
    }

    void OnGUI()
    {
        FPS = (int)1.0f / Time.unscaledDeltaTime;
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, 0, survivorCamera.pixelWidth * -1, survivorCamera.pixelHeight * -1);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = survivorCamera.pixelHeight * 2 / 100;
        style.normal.textColor = Color.yellow;
        string text = string.Format("{0}", FPS);
        GUI.Label(rect, text, style);
    }
}
