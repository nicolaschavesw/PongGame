using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static SaveSystem instance;
    [SerializeField]private string savePath;
    private GameData data;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/progressfile.json";
            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveLevelProgress(int levelIndex)
    {
        data.levels[levelIndex].played = true;
        File.WriteAllText(savePath, JsonUtility.ToJson(data));
    }

    public void LoadProgress()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            data = new GameData { levels = new LevelProgress[10] }; // Suponiendo 10 niveles
            for (int i = 0; i < data.levels.Length; i++)
            {
                data.levels[i] = new LevelProgress { levelIndex = i, played = false };
            }
        }
    }

    public bool HasPlayedLevel(int levelIndex)
    {
        return data.levels[levelIndex].played;
    }

    public static SaveSystem Instance
    {
        get { return instance; }
    }
}