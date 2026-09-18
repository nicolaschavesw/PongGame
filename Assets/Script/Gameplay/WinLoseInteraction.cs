using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinLoseInteraction : MonoBehaviour
{
    private BallMechanics ballMechanics;
    private bool isPaused = false;
    public float pauseDuration = 0.5f;
    public GameObject WinUI;
    public GameObject LoseUI;

    void Awake()
    {
        if (WinUI != null)
            WinUI.SetActive(false);
        if (LoseUI != null)
            LoseUI.SetActive(false);
    }

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
            SaveSystem.Instance.SaveLevelProgress(SceneManager.GetActiveScene().buildIndex);
            WinUI.SetActive(true);
            Debug.Log("Ganaste");
        }
        if (ballMechanics.BallLose())
        {
            LoseUI.SetActive(true);
            Debug.Log("Perdiste");
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

    public void GoToNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (SaveSystem.Instance.HasPlayedLevel(SceneManager.GetActiveScene().buildIndex))
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No hay más niveles. Volviendo al menú principal.");
            GoToMainMenu();
        }
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
