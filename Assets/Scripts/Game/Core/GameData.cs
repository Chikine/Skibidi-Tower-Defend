using System;
using UnityEngine;
[System.Serializable]
public class GameData
{
    [NonSerialized] public int[,] arr = { };
    [NonSerialized] public int[,] path = { };
    public int[] flatArr = { };
    public int width = 24;
    public int height = 11;
    public int round = 0;
    public float maxHP = 20;
    public float HP = 20;
    [NonSerialized] public float speed = 1.0f;
    public int money = 200;//
    public int point = 0;
    public float enemyBuff = 1f;// 1f or more
    public float debuffEfficiency = 1f;//0f to 1f
    public GameData()
    {
        speed = 1.0f;
        flatArr = new int[]
        {
            1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
            1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
            1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
            1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
        };
        arr = new int[height, width];
        path = new int[height, width];
        set2D();
    }
    public void set2D()
    {
        for (int y = 0; y < height; y++) for (int x = 0; x < width; x++) arr[y, x] = flatArr[y * width + x];
    }

    public void set1D()
    {
        for (int y = 0; y < height; y++) for (int x = 0; x < width; x++) flatArr[y * width + x] = arr[y, x];
    }
}
