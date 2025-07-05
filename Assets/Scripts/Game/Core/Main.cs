using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class Main : MonoBehaviour
{
    private PathFinder pathFinder;
    private UpgradeMenu upgradeMenu;
    private Rounds rounds;
    public GameData gameData;
    //initial action
    public void Awake()
    {
        pathFinder = FindFirstObjectByType<PathFinder>();
        upgradeMenu = FindFirstObjectByType<UpgradeMenu>();
        rounds = FindFirstObjectByType<Rounds>();
        gameData = loadGameDataFromFile();
        if (gameData == null)
        {
            Debug.LogError("saveloadmanager not working!");
            gameData = new();
        }
    }
    public GameObject[] prefabs;
    public GameObject[] EnemyPrefabs;
    //start: render the map
    public void Start()
    {
        updatePath();
        //render base on gameData.arr
        for (int i = 0; i < gameData.height; i++)
        {
            for (int j = 0; j < gameData.width; j++)
            {
                int index = gameData.arr[i, j];
                Vector2 position = new(j + 0.5f, -i -0.5f);
                //grass
                if (index != 1) Instantiate(prefabs[0], position, Quaternion.identity);
                //wall
                //else if (index == 1) Instantiate(prefabs[1], position, Quaternion.identity);
                //other 
                if (index > 1 && index < prefabs.Length)
                {
                    Instantiate(prefabs[index], position, Quaternion.identity);
                }
                else
                {
                    //do nothing, no prefab found
                    //Debug.Log("No prefab found: prefab index = " + index);
                }
            }
        }
        //DebugPathFinder();
        //Debug.Log(gameData.arr);
        //Debug.Log(gameData.path);
        // test spawn enemy 
        //Instantiate(EnemyPrefabs[0], new Vector2(0.5f, -5.5f), Quaternion.identity);
        //Spawn(new GameObject[] { EnemyPrefabs[3]}, 0.5f);
        //Spawn(EnemyPrefabs[0], 10, 1f);
        rounds.readRound(gameData.round);
    }

    public void Update()
    {
        
    }

    public GameData getGameData()
    {
        //for temporary game data
        return gameData;
    }

    public void updateGamedata(GameData gamedata)
    {
        //if temporary game data is accepted
        gameData = gamedata;
    }

    public void updatePath()
    {
        var Path = pathFinder.FindPath(new int[] { 5, 0 }, new int[] { 5, 23 }, gameData.arr);
        if (Path == null)
        {
            Debug.Log("way too skibidi");
        }
        else
        {
            gameData.path = Path;
        }
    }

    public void placeNewTower(Vector2 pos, int PrefabIndex)
    {
        Instantiate(prefabs[PrefabIndex], pos, Quaternion.identity);
    }

    public void DebugPathFinder()
    {
        for (int i = 0; i < gameData.height; i++)
        {
            string JSON = "";
            for (int j = 0; j < gameData.width; j++)
            {
                string temp = "";
                temp += ((gameData.path[i, j] == int.MaxValue) ? -1 : gameData.path[i, j]) + "   ";
                JSON += temp.Substring(0, 3);
            }
            Debug.Log(JSON);
        }
    }

    public void Spawn(GameObject[] enemies, float seconds)
    {

        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            StartCoroutine(SpawnAfterDelay(enemy, seconds * i));
        }
    }

    public void Spawn(GameObject enemy, int quantities, float seconds)
    {
        for(int i = 0;i < quantities;i++)
        {
            StartCoroutine(SpawnAfterDelay(enemy, seconds * i));
        }
    }

    public void SpawnEnemy(int index)
    {
        Instantiate(EnemyPrefabs[index], new Vector2(0.5f, 5.5f), Quaternion.identity);
    }

    private IEnumerator SpawnAfterDelay(GameObject enemy, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        Instantiate(enemy, new Vector2(0,0), Quaternion.identity);
    }

    public void LoseHP()
    {
        gameData.HP--;
    }

    public void saveGameDataToFile()
    {
        SaveLoadManager.SaveGame(gameData);
    }

    public GameData loadGameDataFromFile()
    {
        return SaveLoadManager.LoadGame();
    }
}
