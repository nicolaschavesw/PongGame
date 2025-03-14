using UnityEngine;

public class CopySizeBackground : MonoBehaviour
{
    [SerializeField] private RectTransform mainRectTransform;
    private RectTransform rectTransformBackground;
    void Start()
    {
        rectTransformBackground = GetComponent<RectTransform>();
        rectTransformBackground.sizeDelta = mainRectTransform.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
