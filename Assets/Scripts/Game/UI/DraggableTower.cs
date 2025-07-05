using System.Collections.Generic;
using UnityEngine;

public class DraggableTower : MonoBehaviour, IDragBehavior
{
    public int RepresentPrefabIndex;
    public int cost;
    private Alternate alternate;
    private Vector2 originalPosition;
    //private bool isDragging = false;
    private Main main;
    private PathFinder pathFinder;
    public void Start()
    {
        main = FindFirstObjectByType<Main>();
        pathFinder = FindFirstObjectByType<PathFinder>();
        alternate = FindFirstObjectByType<Alternate>();
        originalPosition = transform.position;
        //Debug.Log("origin pos: " +  originalPosition.x + ", " + originalPosition.y);
    }
    public bool startRequire()
    {
        return main.gameData.money >= cost;
    }
    public void onStartDrag()
    {
        setScale(0.1f);
        alternate.show();
        //isDragging = true; 
    }

    public void onDragging()
    {
        //if (isDragging)
        //{
        //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    transform.position = mousePosition;
        //}
        return;
    }

    public void onDrop()
    {
        //if (!isDragging) return;
        GameData gamedata = main.getGameData();
        Vector2 currentPos = transform.position;
        int[] position = new int[2];
        position[0] = (int)(-currentPos.y);
        position[1] = (int)(currentPos.x);
        if (!(position[0] >= 1 && position[1] >= 1 && position[0] < gamedata.height - 1 && position[1] < gamedata.width - 1))
        {
            Debug.Log("not on the field");
        }
        else
        {
            int currentPrefabIndex = gamedata.arr[position[0], position[1]];
            gamedata.arr[position[0], position[1]] = RepresentPrefabIndex;
            var newPath = pathFinder.FindPath(new int[] { 5, 0 }, new int[] { 5, 23 }, gamedata.arr);
            bool checkTrap = EnemyTrapped(newPath);
            if (currentPrefabIndex > 0 || newPath == null || checkTrap )
            {
                gamedata.arr[position[0], position[1]] = currentPrefabIndex;
                Debug.Log("invalid cell!");
            }
            else
            {
                Vector2 newPos = new(position[1] + 0.5f, -position[0] - 0.5f);
                gamedata.path = newPath;
                gamedata.money -= cost;
                main.updateGamedata(gamedata);
                main.placeNewTower(newPos, RepresentPrefabIndex);
                //main.DebugPathFinder();
                Debug.Log("money -" + cost + ", money left: " + gamedata.money);
            }
        }
        alternate.hide();
        //isDragging = false;
        transform.position = originalPosition;
        Debug.Log("move " + this.name + " back to " + originalPosition.x + ", " + originalPosition.y);
        setScale(0.2f);
    }
    private bool EnemyTrapped(int[,] newPath)
    {
        if(newPath == null)
        {
            return true;
        }
        foreach (Enemy enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            int x = enemy.currentPosition[0];
            int y = enemy.currentPosition[1];
            int row = newPath.GetLength(0);
            int col = newPath.GetLength(1);
            //Debug.Log("enemy " + enemy.name + " is at position " + x + ", " + y + ", at there cell = " + newPath[x, y]);
            if (!(x >= 0 && y >= 0 && x < row && y < col)) return true;
            if (newPath[x, y] == int.MaxValue) return true;
        }
        return false;
    }

    private void setScale(float scale)
    {
        transform.localScale = new(scale, scale, scale);
    }
}
