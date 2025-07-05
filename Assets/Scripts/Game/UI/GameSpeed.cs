using UnityEngine;
using UnityEngine.UI;

public class GameSpeed : MonoBehaviour
{
    public Sprite[] sprites;
    private Image image;
    private Main main;
    private int maxIndex;
    private int index = 0;
    public void Start()
    {
        main = FindFirstObjectByType<Main>();
        image = GetComponent<Image>();
        GameData gameData = main.gameData;
        maxIndex = sprites.Length;
        index = (int)gameData.speed - 1;
        setGameSpeed(gameData.speed);
    }

    public void setGameSpeed(float speed)
    {
        Time.timeScale = speed;
        GameData gameData = main.gameData;
        gameData.speed = speed;
    }

    public void nextSpeed()
    {
        index = (index + 1) % maxIndex;
        image.sprite = sprites[index];
        float speed = (float)(index + 1);
        setGameSpeed(speed);
    }
}
