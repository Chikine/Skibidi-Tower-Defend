using UnityEngine;
using System;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
    public int[] Pos { get; set; }

    public int[,] FindPath(int[] start, int[] end, int[,] field)
    {
        // Declare filter for passable places
        var filter = new HashSet<int> { 0 };
        // Info
        int row = field.GetLength(0);
        int col = field.GetLength(1);
        //Debug.Log("row:" + row + ",col:" + col);
        // Result which contains the way
        int[,] result = new int[row, col];
        //fill result with 999
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                result[i, j] = int.MaxValue;
            }
        }
        // Passed
        var passed = new HashSet<string> { $"{end[0]}-{end[1]}" };
        // Queue
        var queue = new Queue<int[]>();
        // Start from end point to find the way to first point
        int[] first = { end[0], end[1], 0 };
        queue.Enqueue(first);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            var x = node[0];
            var y = node[1];
            var step = node[2];
            foreach (var (a, b) in new[] { (x, y + 1), (x, y - 1), (x + 1, y), (x - 1, y) })
            {
                if (IsValid(a, b, row, col, field, filter, passed))
                {
                    passed.Add($"{a}-{b}");
                    result[a,b] = step + 1;
                    int[] next = { a, b, step + 1};
                    queue.Enqueue(next);
                }
            }
        }

        if (result[start[0], start[1]] == int.MaxValue)
        {
            Debug.Log("No path found"); // Handle no path found ...
            return null;
        }
        result[end[0], end[1]] = 0;
        return result;
    }

    private bool IsValid(int x, int y, int row, int col, int[,] field, HashSet<int> filter, HashSet<string> passed)
    {
        return x >= 0 && x < row && y >= 0 && y < col && filter.Contains(field[x,y]) && !passed.Contains($"{x}-{y}");
    }
}