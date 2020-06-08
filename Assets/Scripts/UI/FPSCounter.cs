using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public float FPS;

    [SerializeField]
    private Camera survivorCamera;


    private Rect rect;

    private GUIStyle style;

    public bool show;

    void Start()
    {
        style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = survivorCamera.pixelHeight * 2 / 100;
        style.normal.textColor = Color.yellow;
        rect = new Rect(0, 0, survivorCamera.pixelWidth * -1, survivorCamera.pixelHeight * -1);
    }

    void OnGUI()
    {
        if (show)
        {
            FPS = (int)1.0f / Time.unscaledDeltaTime;
            string text = string.Format("{0}", FPS);
            GUI.Label(rect, text, style);
        }
    }
}
