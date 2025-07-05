using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public ConfirmBox confirmBox;
    public void Start()
    {
        Debug.Log("Hello World!");
    }

    //play game
    public void PlayGame(bool newGame)
    {
        if (newGame)
        {
            //check if save data exist alert user to confirm new game
            if (SaveLoadManager.SaveDataExists())
            {
                // Show confirmation box
                Debug.Log("Save data exists");
                confirmBox.Show();
                return;
            }
        }
        SceneManager.LoadSceneAsync(1);
    }
    public void OverWriteSaveData(bool cf)
    {
        //hide confirmation box
        confirmBox.Hide();
        //check cf
        if (cf)
        {
            //if confirm new game
            Debug.Log("New Game!");
            SaveLoadManager.DeleteSaveData();
            //load game
            PlayGame(false);
        }
    }
    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
