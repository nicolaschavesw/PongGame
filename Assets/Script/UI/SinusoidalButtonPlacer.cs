using UnityEngine;
using UnityEngine.UI;

public class SinusoidalButtonPlacer : MonoBehaviour
{
    public RectTransform contentTransform; // El panel donde irán los botones
    public GameObject buttonPrefab; // Prefab del botón
    public int buttonCount = 10; // Número de botones
    public float amplitude = 480f; // Amplitud de la onda
    public float frequency = 1.0f; // Frecuencia de la onda
    public float spacing = 500f; // Distancia entre botones

    void Start()
    {
        Debug.Log(contentTransform.sizeDelta.x);
        for (int i = 0; i < buttonCount; i++)
        {
            float x = i * spacing - ((contentTransform.sizeDelta.x / 2.0f) - 300.0f);
            float y = Mathf.Sin(x * frequency) * amplitude;

            GameObject newButton = Instantiate(buttonPrefab, contentTransform);
            RectTransform buttonRect = newButton.GetComponent<RectTransform>();

            buttonRect.anchoredPosition = new Vector2(x, y);
        }
    }
}