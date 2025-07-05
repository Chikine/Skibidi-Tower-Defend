using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    private Main main;
    public GameObject pausePanel;
    public void Start()
    {
        main = FindFirstObjectByType<Main>();
        Continue();
    }


    public void Exit()
    {
        Debug.Log("exit");
        SceneManager.LoadSceneAsync(0);
        Resources.UnloadUnusedAssets();
    }

    public void RestartGame()
    {
        SaveLoadManager.DeleteSaveData();
        SceneManager.LoadSceneAsync(1);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void Continue()
    {
        GameData gameData = main.gameData;
        Time.timeScale = gameData.speed;
        pausePanel.SetActive(false);
    }
}
