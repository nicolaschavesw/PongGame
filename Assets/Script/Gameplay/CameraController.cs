using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private GameObject gameBall;
    private Rigidbody ballRb;
    [Header("Seguimiento de Camara")]
    [SerializeField] private float baseCameraSpeed = 5f; // Velocidad base de la cámara
    [SerializeField] private float speedMultiplier = 0.5f; // Multiplicador de la velocidad
    public Vector3 Movement;
    private bool isShaking = false; // Estado de vibración
    void Awake()
    {
        gameBall = GameObject.FindGameObjectWithTag("Ball");
        if (gameBall != null)
        {
            ballRb = gameBall.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.Log("El objeto encontrado por la camara no tiene RigidBody");
        }
        Movement = transform.position;
    }

    void LateUpdate()
    {
        if (gameBall == null) return;
        float dynamicSpeed = baseCameraSpeed;

        if (ballRb != null)
        {
            float ballSpeed = ballRb.linearVelocity.magnitude; // Magnitud de la velocidad de la bola
            dynamicSpeed += ballSpeed * speedMultiplier; // Ajustar la velocidad de la cámara
        }
        Vector3 targetPosition = gameBall.transform.position + Movement;
        Vector3 softPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * dynamicSpeed);
        if (!isShaking)
        {
            transform.position = softPosition;
        }
    }
    public void TriggerSmallShake(float duration = 0.3f, float intensity = 0.15f)
    {
        StartCoroutine(ShakeCamera(duration, intensity));
    }
    public void TriggerHardImpact(float intensity = 0.5f)
    {
        StartCoroutine(HardImpact(intensity));
    }
    private IEnumerator ShakeCamera(float duration, float intensity)
    {
        isShaking = true;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float x = (Mathf.PerlinNoise(Time.time * 20f, 0f) * 2f - 1f) * intensity;
            float y = (Mathf.PerlinNoise(0f, Time.time * 20f) * 2f - 1f) * intensity;
            transform.position += new Vector3(x, y, 0);
            yield return null;
        }
        isShaking = false;
    }
    private IEnumerator HardImpact(float intensity)
    {
        isShaking = true;
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = originalPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * intensity;
        transform.position = targetPosition;
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < 3; i++)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, 0.5f);
            yield return new WaitForSeconds(0.05f);
        }
        transform.position = originalPosition;
        isShaking = false;
    }
}
