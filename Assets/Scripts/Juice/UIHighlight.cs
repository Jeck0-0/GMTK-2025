using UnityEngine;
using UnityEngine.UI;

public class UIHighlight : MonoBehaviour
{
    public Color colorA = Color.red;
    public Color colorB = Color.blue;
    public float duration = 1f;

    private Graphic uiGraphic;

    void Awake()
    {
        uiGraphic = GetComponent<Graphic>();
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time / duration, 1f);
        uiGraphic.color = Color.Lerp(colorA, colorB, t);
    }
}
