using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SinusoidalButtonPlacer : MonoBehaviour
{
    public RectTransform contentTransform; // El panel donde irán los botones
    public GameObject buttonPrefab; // Prefab del botón
    public int buttonCount = 10; // Número de botones
    public float amplitude = 480f; // Amplitud de la onda
    public float frequency = 1.0f; // Frecuencia de la onda
    public float spacing = 500f; // Distancia entre botones
    private LineRenderer lineRenderer;
    private int curveResolution = 1000; // Número de puntos en la curva

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = curveResolution;
        lineRenderer.startWidth = 5f;
        lineRenderer.endWidth = 5f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        DrawSineWave();
        //PlaceButtons();
    }

    void DrawSineWave()
    {
        float graphWidth = contentTransform.sizeDelta.x; // Ancho del área donde se dibuja la onda

        for (int i = 0; i < curveResolution; i++)
        {
            float x = i * spacing - ((contentTransform.sizeDelta.x / 2.0f) - 300.0f);
            float y = Mathf.Sin(x * frequency) * amplitude;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    void PlaceButtons()
    {
        for (int i = 0; i < buttonCount; i++)
        {
            float x = i * spacing - ((contentTransform.sizeDelta.x / 2.0f) - 300.0f);
            float y = Mathf.Sin(x * frequency) * amplitude;

            GameObject newButton = Instantiate(buttonPrefab, contentTransform);
            RectTransform buttonRect = newButton.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(x, y);

            Button buttonComponent = newButton.GetComponent<Button>();
            int sceneIndex = i + 1;
            //buttonComponent.onClick.AddListener(() => LoadScene(sceneIndex));
        }
    }
}