using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class Rounds : MonoBehaviour
{
    private Main main;
    private int activeSpawnCoroutine = 0;
    public void Start()
    {
        main = FindFirstObjectByType<Main>();
    }

    public void getNextRound()
    {
        GameData gameData = main.gameData;
        gameData.round++;
        readRound(gameData.round);
        main.saveGameDataToFile();
    }
    public void readRound(int index)
    {
        Debug.Log("get round " + (index + 1));
        string round = RoundsInString.Length > index ? RoundsInString[index] : "";
        string[] arr = round.Split('/');
        StartCoroutine(handleRound(arr));
    }
    private IEnumerator handleRound(string[] arr)
    {
        Debug.Log("round go true");
        int size = arr.Length;
        float publicInterval = (size >= 2) ? Number(arr[1]) : -1f;
        if (size >= 1)
        {
            string[] strings = arr[0].Split('&');
            int index = 0;
            while(index < strings.Length)
            {
                string s = strings[index].Trim();
                index++;
                string[] info = s.Split(':');
                int infoSize = info.Length;
                int amount = (info.Length >= 2) ? Number(info[1]) : 1;
                float startTime = (info.Length >= 3) ? Float(info[2]) : 0;
                string[] Sprefab = (info.Length >= 1) ? getPrefabList(info[0]) : new string[] { "0" };
                string Sinterval = (info.Length >= 4 && info[3].Length > 0) ? info[3] : "0.5";
                Coroutine spawnCoroutine = StartCoroutine(SpawnEnemy(amount, startTime, Sprefab, Sinterval));
                activeSpawnCoroutine++;
                if (publicInterval >= 0)
                {
                    yield return spawnCoroutine;
                    yield return new WaitForSeconds(publicInterval);
                }
            }
        }
        Debug.Log("round go false");
    }

    private string[] getPrefabList(string s)
    {
        s = s.Trim();
        if(s.Length >= 5)
        {
            string type = s.Substring(0, 5);
            if (type == "(set)")
            {
                return s.Substring(5).Split(">");
            }
            if (type == "(rnd)")
            {
                string[] list = s.Substring(5).Split("?");
                int count = list.Length;
                return new string[] { list[UnityEngine.Random.Range(0, count - 1)] };
            }
        }
        return new string[] {s};
    }

    private IEnumerator SpawnEnemy(int amount, float startTime, string[] Sprefab, string Sinterval)
    {
        yield return new WaitForSeconds(startTime);
        for (int i = 0; i < amount; i++)
        {
            for (int j = 0; j < Sprefab.Length; j++)
            {
                int prefabIndex = Number(Sprefab[j]);
                float interval = Float(Sinterval);
                main.SpawnEnemy(prefabIndex);
                if(i == amount - 1 && j == Sprefab.Length - 1)
                {
                    //real endpoint of coroutine
                    activeSpawnCoroutine--;
                }
                yield return new WaitForSeconds(interval);
            }
        }
    }

    public bool isAllCoroutinesFinish()
    {
        return activeSpawnCoroutine == 0;
    }

    private int Number(string s)
    {
        s = s.Trim();
        if (s.Contains("->"))
        {
            string[] ss = s.Split("->");
            int from = (int.TryParse(ss[0], out int a)) ? a : 0;
            int to = (int.TryParse(ss[1], out int b)) ? b : 0;
            return UnityEngine.Random.Range(from, to);
        }
        else return (int.TryParse(s, out int result)) ? result : 0;
    }

    private float Float(string s)
    {
        s = s.Trim();
        if (s.Contains("->"))
        {
            string[] ss = s.Split("->");
            float from = (float.TryParse(ss[0], out float a)) ? a : 0;
            float to = (float.TryParse(ss[1], out float b)) ? b : 0;
            return UnityEngine.Random.Range(from, to);
        }
        else return (float.TryParse(s, out float result)) ? result : 0;
    }

    private string[] RoundsInString = {
        //str structure: prefab : amount : interval : startTime(optional) ( & ) / public interval(optional) / addition(optional)
        //default value:   0        1         0.5                0                           -1                      no
        "0 : 1 : 0 ", //1
        "0 : 3 : 0.5 : 1",//2
        "0 : 5->10 : 1.5",//3
        "0 & 2 & 0 / 1",//4
        "(set)0 > 2 > 0 > 0 : 2 : 1 & 0 : 5 : 0.2 : 3",//5
        "2 & (set)2 > 0 : 3 & 0 : 5 / 0.5",//6
        "2 : 10 & 0 : 5 / 0.5",//7
        "1 : 3 : 1",//8
        "(set)0 > 2 > 1 : 3 : 1 : 0.5 & 0 : 10 : 0.5 : 5",//9
        "1 : 10 : 0.5 -> 1.5 : 0 -> 2 & 0 : 10 : 0.5 -> 1.5 : 1 -> 3 & 2 : 10 : 0.5 -> 1.5 : 2 -> 4",//10
        "5 : 2 / 0.5", //11
        "4 : 1", //12
        "1 & 2 & 3 & 4 & 5 : 2 / 0.5" //13
    };
}
/* Declare prefabs here
 * 0: normale skibidi
 * 1: ghost
 * 2: freeze skibidi (ice)
 * 3: iron skibidi
 * 4: shieldibidi
 * 5: fly skibidi
 */