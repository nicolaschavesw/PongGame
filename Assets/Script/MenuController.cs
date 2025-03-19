using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button[] levelButtons; 

    void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool played = SaveSystem.Instance.HasPlayedLevel(i);
            levelButtons[i].interactable = played || i == 0;
        }
    }

    public void LoadLevel(int levelIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
    }
}