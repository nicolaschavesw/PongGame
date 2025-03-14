using UnityEngine;
using System.Collections;
using TMPro.Examples;

public class BallMechanics : MonoBehaviour
{
    [Header("Destrucción de la Pelota")]
    public GameObject ballPrefab; // Prefab de la pelota
    public GameObject fracturedBallPrefab; // Prefab de la pelota rota
    public float explosionForce = 5f; // Fuerza de la explosión
    public float explosionRadius = 2f; // Radio de la explosión
    public float destroyDelay = 3f; // Tiempo antes de eliminar los fragmentos
    [Header("Frenado y Rebote al Ganar")]
    public float bounceReduction = 0.2f; // Cuánto reducir el rebote en cada colisión con "Win"
    public float linearDampingFriction = 2f; // Fricción para frenar la bola
    public float stopAngularDamping = 2f; // Fricción rotacional
    public float minVelocity = 0.1f; // Velocidad mínima antes de detenerse completamente
    private WinLoseCollision winLoseCollision;
    private Rigidbody rb;
    private PhysicsMaterial ballMaterial;
    private bool win = false;
    private bool lose = false;
    private bool hasWinStarted = false;
    private bool hasLoseStarted = false;
    private CameraController cameraController;

    private void Start()
    {
        rb = ballPrefab.GetComponent<Rigidbody>();
        Collider col = ballPrefab.GetComponent<Collider>();
        winLoseCollision = ballPrefab.GetComponent<WinLoseCollision>();
        GameObject Camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraController = Camera.GetComponent<CameraController>();

        if (col != null && col.material != null)
        {
            ballMaterial = col.material; // Obtener el material físico de la pelota
        }
    }
    private void Update()
    {
        if (winLoseCollision.BallWin() && !hasWinStarted)
        {
            hasWinStarted = true;
            StartCoroutine(WinWait());
        }
        if (winLoseCollision.BallLose() && !hasLoseStarted)
        {
            hasLoseStarted = true;
            StartCoroutine(LoseWait());
        }
    }

    void DestroyBall()
    {
        fracturedBallPrefab.transform.position = ballPrefab.transform.position;
        fracturedBallPrefab.SetActive(true);

        foreach (Rigidbody rb in fracturedBallPrefab.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
        ballPrefab.SetActive(false);
    }

    void ReduceBounceAndSlowDown()
    {
        if (ballMaterial != null)
        {
            ballMaterial.bounciness = Mathf.Max(ballMaterial.bounciness - bounceReduction, 0f); // Reducir rebote sin ser negativo
        }
        rb.useGravity = false;
        rb.linearDamping = linearDampingFriction; // Aumentar fricción lineal
        rb.angularDamping = stopAngularDamping; // Aumentar fricción rotacional

        StartCoroutine(CheckForStop()); // Iniciar la rutina que detendrá la pelota
    }

    IEnumerator CheckForStop()
    {
        while (rb.linearVelocity.magnitude > minVelocity)
        {
            yield return new WaitForSeconds(0.1f); // Revisar cada 0.1 segundos
        }
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; // Bloquear la física para que no vuelva a moverse
    }

    IEnumerator WinWait()
    {
        ReduceBounceAndSlowDown();
        yield return new WaitForSeconds(3.0f);
        win = true;
    }
    IEnumerator LoseWait()
    {
        DestroyBall();
        cameraController.TriggerHardImpact(); // Efecto de perder
        //cameraController.TriggerSmallShake(); // Efecto de perder
        //cameraController.TriggerWaveShake(); // Efecto de perder
        yield return new WaitForSeconds(3.0f);
        lose = true;
    }

    public bool BallWin()
    {
        return win;
    }

    public bool BallLose()
    {
        return lose;
    }
}
