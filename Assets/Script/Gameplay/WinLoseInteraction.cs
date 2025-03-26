using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinLoseInteraction : MonoBehaviour
{
    private BallMechanics ballMechanics;
    private bool isPaused = false;
    public float pauseDuration = 0.5f;
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

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        ResetTime();
    }

    public void ResetTime()
    {
        Time.timeScale = 1f;  
        Time.fixedDeltaTime = 0.02f;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        StopAllCoroutines();
        StartCoroutine(isPaused ? GradualPause() : GradualResume());
    }

    private IEnumerator GradualPause()
    {
        float elapsedTime = 0f;
        float startScale = Time.timeScale;
        while (elapsedTime < pauseDuration)
        {
            Time.timeScale = Mathf.Lerp(startScale, 0f, elapsedTime / pauseDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 0f;
    }

    private IEnumerator GradualResume()
    {
        float elapsedTime = 0f;
        float startScale = Time.timeScale;
        while (elapsedTime < pauseDuration)
        {
            Time.timeScale = Mathf.Lerp(startScale, 1f, elapsedTime / pauseDuration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1f;
    }
}
