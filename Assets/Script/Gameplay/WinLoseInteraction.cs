using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseInteraction : MonoBehaviour
{
    private BallMechanics ballMechanics;
    void Start()
    {
        GameObject ballObject = GameObject.FindGameObjectWithTag("Main Ball"); // Encuentra el primer objeto con el tag "Ball"

        if (ballObject != null)
        {
            ballMechanics = ballObject.GetComponent<BallMechanics>(); // Obtiene el script BallInteraction

            if (ballMechanics != null)
            {
                //Debug.Log("BallInteraction encontrado en " + ballObject.name);
            }
            else
            {
                Debug.LogWarning("El objeto con tag 'Ball' no tiene el script BallInteraction.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró ningún objeto con el tag 'Ball'.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ballMechanics.BallWin())
        {
            RestartLevel();
        }
        if (ballMechanics.BallLose())
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);// Recarga la escena actual
        ResetTime(); 
    }

    public void ResetTime()
    {
        Time.timeScale = 1f;  // Restablece la velocidad normal
        Time.fixedDeltaTime = 0.02f;
    }
}
