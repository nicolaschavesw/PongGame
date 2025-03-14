using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;

public class CreateNew : MonoBehaviour
{
    [Header("Configuración de Plataforma")]
    public GameObject platformPrefab; // Prefab del objeto a generar
    public string platformName; // Nombre de la plataforma

    [Header("Cámara y Entrada")]
    private Camera gameCamera; // Cámara principal

    [Header("Control de Creación")]
    [SerializeField] private Vector3 startPoint; // Punto inicial del arrastre
    [SerializeField] private Vector3 lastValidPoint; // Última posición válida
    private GameObject currentObject; // Plataforma en construcción
    private bool isDragging = false; // Control de arrastre
    public LayerMask ballLayer; // Capa de la pelota

    private bool isMobile; // Para saber si estamos en móvil o PC

    void Awake()
    {
        gameCamera = Camera.main ?? FindAnyObjectByType<Camera>();
        if (gameCamera == null)
        {
            Debug.LogWarning("No se encontró ninguna cámara en la escena.");
        }
    }

    void Start()
    {
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        Debug.Log(isMobile ? "📱 Ejecutando en un celular" : "💻 Ejecutando en PC");
        if (isMobile)
        {
            Instantiate(platformPrefab, new Vector3(0, 10, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(platformPrefab, new Vector3(0, -10, 0), Quaternion.identity);
        }
        EnhancedTouchSupport.Enable();
        if (Touchscreen.current == null)
        {
            Debug.LogError("❌ Touchscreen.current es NULL. Revisa la configuración del Input System.");
        }
        InputSystem.EnableDevice(Mouse.current);
        Debug.Log("🖱️ Simulación de Touch activada en el Editor.");
    }

    void Update()
    {
        if (IsPointerDown())
        {
            startPoint = GetPointerWorldPosition();
            CreateObject();
            isDragging = true;
            Debug.Log("Touch in");
        }

        // Mientras arrastra, actualiza el tamaño
        if (isDragging && currentObject != null)
        {
            lastValidPoint = GetPointerWorldPosition();
            UpdateObjectSize();

            if (CheckCollisionWithBall()) // Si toca la pelota, cancela
            {
                Destroy(currentObject);
                currentObject = null;
                isDragging = false;
                Debug.Log("Plataforma cancelada: tocó la pelota.");
            }
        }

        // Cuando se suelta, guarda la última posición válida
        if (IsPointerUp())
        {
            isDragging = false;
            lastValidPoint = GetPointerWorldPosition();
            UpdateObjectSize();
            Debug.Log("Touch out");
        }
    }
    private bool IsPointerDown()
    {
        return (Mouse.current?.leftButton.wasPressedThisFrame ?? false) ||
               (Touchscreen.current?.primaryTouch.press.wasPressedThisFrame ?? false);
    }

    private bool IsPointerUp()
    {
        return (Mouse.current?.leftButton.wasReleasedThisFrame ?? false) ||
               (Touchscreen.current?.primaryTouch.press.wasReleasedThisFrame ?? false);
    }
    // Obtiene la posición del puntero en el mundo
    private Vector3 GetPointerWorldPosition()
    {
        Vector2 pointerPosition = Vector2.zero;

        if (isMobile)
        {
            if (Touchscreen.current?.primaryTouch.press.isPressed == true)
            {
                pointerPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                pointerPosition.y = Screen.height - pointerPosition.y; // Ajuste para coordenadas de pantalla
            }
            else
            {
                return Vector3.zero; // ⚠️ Evitar usar ScreenToWorldPoint con coordenadas inválidas
            }
        }
        else if (Mouse.current != null)
        {
            pointerPosition = Mouse.current.position.ReadValue();
        }

        // 🚨 Validar si pointerPosition es correcta
        if (pointerPosition == Vector2.zero || float.IsInfinity(pointerPosition.x) || float.IsInfinity(pointerPosition.y))
        {
            Debug.LogWarning("⚠️ No se obtuvo una posición válida del puntero");
            return Vector3.zero; // 🔴 Evitar que se use una posición inválida
        }

        Vector3 worldPosition = gameCamera.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y, Mathf.Abs(gameCamera.transform.position.z)));
        worldPosition.z = 0; // 🔥 Mantener en el mismo plano 2D
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
            currentObject.transform.localScale = new Vector3(distance, 0.2f, 2.0f);
        }
    }

    private bool CheckCollisionWithBall()
    {
        if (currentObject == null) return false;

        Vector3 size = currentObject.transform.localScale / 2f; // Mitad del tamaño del objeto
        Collider[] colliders = Physics.OverlapBox(currentObject.transform.position, size, currentObject.transform.rotation, ballLayer);

        return colliders.Length > 0; // Devuelve true si toca la pelota
    }
}