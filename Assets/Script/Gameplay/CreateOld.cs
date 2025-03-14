using System;
using UnityEngine;

public class CreateOld : MonoBehaviour
{
    [Header("Configuración de Plataforma")]
    public GameObject platformPrefab;
    public string platformName;

    [Header("Cámara y Entrada")]
    private Camera gameCamera;

    [Header("Control de Creación")]
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 lastValidPoint;
    private GameObject currentObject;
    private bool isDragging = false;
    public LayerMask ballLayer;

    private bool isMobile;

    void Awake()
    {
        gameCamera = Camera.main ?? FindFirstObjectByType<Camera>();
        if (gameCamera == null)
        {
            Debug.LogWarning("No se encontró ninguna cámara en la escena.");
        }
    }

    void Start()
    {
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        Debug.Log(isMobile ? "📱 Ejecutando en un celular" : "💻 Ejecutando en PC");
    }

    void Update()
    {
        if (IsPointerDown())
        {
            startPoint = GetPointerWorldPosition();
            CreateObject();
            isDragging = true;
        }

        if (isDragging && currentObject != null)
        {
            lastValidPoint = GetPointerWorldPosition();
            UpdateObjectSize();

            if (CheckCollisionWithBall())
            {
                Destroy(currentObject);
                currentObject = null;
                isDragging = false;
                Debug.Log("Plataforma cancelada: tocó la pelota.");
            }
        }

        if (IsPointerUp())
        {
            isDragging = false;
            lastValidPoint = GetPointerWorldPosition();
            UpdateObjectSize();
        }
    }
    private bool IsPointerDown()
    {
        return Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }
    private bool IsPointerUp()
    {
        return Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
    }
    private Vector3 GetPointerWorldPosition()
    {
        Vector2 pointerPosition = Vector2.zero;

        if (isMobile && Input.touchCount > 0)
        {
            pointerPosition = Input.GetTouch(0).position;
        }
        else
        {
            pointerPosition = Input.mousePosition;
        }

        Vector3 worldPosition = gameCamera.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y, Mathf.Abs(gameCamera.transform.position.z)));
        worldPosition.z = 0;
        return worldPosition;
    }

    private void CreateObject()
    {
        currentObject = Instantiate(platformPrefab, startPoint, Quaternion.identity);
        currentObject.name = platformName;
    }

    private void UpdateObjectSize()
    {
        if (currentObject != null)
        {
            Vector3 currentPoint = lastValidPoint;
            Vector3 midPoint = (startPoint + currentPoint) / 2f;
            float distance = Vector3.Distance(startPoint, currentPoint);

            currentObject.transform.position = midPoint;
            currentObject.transform.right = (currentPoint - startPoint).normalized;
            currentObject.transform.localScale = new Vector3(distance, currentObject.transform.localScale.y, currentObject.transform.localScale.z);
        }
    }

    private bool CheckCollisionWithBall()
    {
        if (currentObject == null) return false;

        Vector3 size = currentObject.transform.localScale / 2f;
        Collider[] colliders = Physics.OverlapBox(currentObject.transform.position, size, currentObject.transform.rotation, ballLayer);

        return colliders.Length > 0;
    }
}