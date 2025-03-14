using System.IO;
using UnityEngine;

[System.Serializable]
public class LevelProgress
{
    public int levelIndex;
    public bool played;
}

[System.Serializable]
public class GameData
{
    public LevelProgress[] levels;
}