using System.IO;
using UnityEngine;
public static class SaveLoadManager
{
    //save game
    public static void SaveGame(GameData gameData)
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "skibidi.json");
        gameData.set1D();
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved!");
    }

    //load game
    public static GameData LoadGame()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "skibidi.json");
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            gameData.set2D();
            Debug.Log("Game Loaded!");
            return gameData;
        }
        else
        {
            Debug.Log("No save file found.");
            return new GameData();
        }
    }

    //check if save data exists
    public static bool SaveDataExists()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "skibidi.json");
        return File.Exists(saveFilePath);
    }

    //delete save data
    public static void DeleteSaveData()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "skibidi.json");
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save data deleted.");
        }
        else
        {
            Debug.Log("No save file to delete.");
        }
    }
}