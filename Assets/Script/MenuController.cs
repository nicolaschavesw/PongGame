using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button[] levelButtons; // Asigna los botones en el Inspector

    void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool played = SaveSystem.Instance.HasPlayedLevel(i);
            levelButtons[i].interactable = played || i == 0; // El primer nivel siempre está disponible
        }
    }

    public void LoadLevel(int levelIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
    }
}