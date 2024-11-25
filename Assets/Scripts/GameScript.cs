using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;
using Random = UnityEngine.Random;

public class GameScript : MonoBehaviour
{

    public GameObject tile;
    public GameObject outline;
    public GameObject[,] gameTiles = new GameObject[30, 30];

    public int[,] tiles;
    public int health;
    public float largestIslandSum;
    public string result;
    public Color newColor;

    static int[] dRow = { 0, 1, 0, -1, -1, 1, 1, -1};
    static int[] dCol = { -1, 0, 1, 0, 1, -1, 1, -1};

    public static int dfsSum = 0;
    public static int dfsCount = 0;

    static bool IsValid(bool[,] vis, int row, int col, GameObject[,] grid)
    {
        if (row < 0 || col < 0 ||
            row >= 30 || col >= 30)
            return false;

        if (vis[row, col] || grid[row, col].GetComponent<CalculateOnClickScript>().tileValue == 0)
            return false;

        return true;
    }

    static void DFS(int row, int col, int[,] grid, bool[,] vis, GameObject[,] gameGrid)
    {
        Stack st = new Stack();
        st.Push(new Tuple<int, int>(row, col));

        GameObject island = null;

        while (st.Count > 0)
        {
            Tuple<int, int> curr = (Tuple<int, int>)st.Peek();
            st.Pop();

            row = curr.Item1;
            col = curr.Item2;

            if (!IsValid(vis, row, col, gameGrid))
                continue;

            vis[row, col] = true;

            dfsSum += grid[row, col];
            dfsCount++;

            if (island == null) island = new GameObject("Island");

            gameGrid[row, col].transform.parent = island.transform;
            gameGrid[row, col].transform.parent.tag = "Tiles";

            //Debug.Log(dfsSum + " " + dfsCount);

            for (int i = 0; i < 8; i++)
            {
                int adjx = row + dRow[i];
                int adjy = col + dCol[i];
                st.Push(new Tuple<int, int>(adjx, adjy));
            }
        }
    }

    void FindLargestIsland(string result)
    {
        bool[,] vis = new bool[30, 30];
        largestIslandSum = 0;

        tiles = CreateMatrix(result);

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                vis[i, j] = false;
            }
        }

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                dfsSum = 0;
                dfsCount = 0;
                DFS(i, j, tiles, vis, gameTiles);
                if (dfsSum != 0)
                {
                    if ((float)dfsSum / dfsCount > largestIslandSum) largestIslandSum = (float)dfsSum / dfsCount;
                }
            }
        }

        //Debug.Log("Largest island: " + largestIslandSum);
    }

    Color GetColor(int value)
    {
        Color lowElevationColor = new Color(0.54f, 0.80f, 0.47f); // green 
        Color midLowElevationColor = new Color(1f, 0.98f, 0.75f); // light yellow  
        Color midHighElevationColor = new Color(1f, 0.86f, 0.56f); // light orange 
        Color highElevationColor = new Color(0.63f, 0.52f, 0.37f); // dark yellow 
        Color veryHighElevationColor = Color.white;
        float newValue;

        if (value >= 1 && value <= 250)
        {
            newValue = value / 250f;
            newColor = Color.Lerp(lowElevationColor, midLowElevationColor, newValue);
        }

        if (value > 250 && value <= 500)
        {
            newValue = value / 500f;
            newColor = Color.Lerp(midLowElevationColor, midHighElevationColor, newValue);
        }

        if (value > 500 && value <= 750)
        {
            newValue = value / 750f;
            newColor = Color.Lerp(midHighElevationColor, highElevationColor, newValue);
        }

        if (value > 750 && value <= 1000)
        {
            newValue = value / 1000f;
            newColor = Color.Lerp(highElevationColor, veryHighElevationColor, newValue);
        }

        return newColor;

    }

    int[,] CreateMatrix(string text)
    {
        string[] lines = text.Split('\n');
        int rows = lines.Length;
        int cols = lines[0].Split(' ').Length;
        int[,] matrix = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            string[] numbers = lines[i].Split(' ');
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = int.Parse(numbers[j]);
            }
        }

        return matrix;
    }

    void InititateTiles(string result)
    {
        GameObject water = null;

        float posx;
        float posy = 7.25f;
        float offsetX, offsetY;

        tiles = CreateMatrix(result);

        for (int i = 0; i < 30; i++)
        {
            if (i == 0) offsetY = 0;
            else offsetY = 0.5f;
            posy -= offsetY;
            posx = -7.247f;
            for (int j = 0; j < 30; j++)
            {
                if (j == 0) offsetX = 0;
                else offsetX = 0.5f;
                posx += offsetX;
                GameObject newTile = Instantiate(tile, new Vector3(posx, posy, 0), Quaternion.identity);
                gameTiles[i, j] = newTile;
                newTile.GetComponent<CalculateOnClickScript>().tileValue = tiles[i, j];
                newTile.GetComponent<CalculateOnClickScript>().tileLocationX = j;
                newTile.GetComponent<CalculateOnClickScript>().tileLocationY = i;
                if (water == null) water = new GameObject("Water");
                if (newTile.GetComponent<CalculateOnClickScript>().tileValue == 0)
                {
                    newTile.GetComponent<SpriteRenderer>().color = new Color(0 / 255f, 157 / 255f, 196 / 255f);
                    newTile.transform.parent = water.transform;
                    newTile.transform.parent.tag = "Tiles";
                }
                else if (newTile.GetComponent<CalculateOnClickScript>().tileValue >= 1) newTile.GetComponent<SpriteRenderer>().color = GetColor(newTile.GetComponent<CalculateOnClickScript>().tileValue);
            }
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(": Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(": HTTP Error: " + webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                result = webRequest.downloadHandler.text;
                InititateTiles(result);
                FindLargestIsland(result);
                break;
        }
    }

    void Start()
    {
        StartCoroutine(GetRequest("https://jobfair.nordeus.com/jf24-fullstack-challenge/test"));
        health = 3;
    }

    public void GameOver(string result)
    {
        GameObject[] allTiles, allIncorrect, allSelected, allOutlines;
        GameObject canvas;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        allTiles = GameObject.FindGameObjectsWithTag("Tiles");
        allIncorrect = GameObject.FindGameObjectsWithTag("Incorrect");
        allSelected = GameObject.FindGameObjectsWithTag("AlreadySelected");
        allOutlines = GameObject.FindGameObjectsWithTag("Outline");

        foreach (GameObject outline in allOutlines)
        {
            outline.SetActive(false);
        }

        foreach (GameObject selected in allSelected)
        {
            selected.SetActive(false);
        }

        foreach (GameObject incorrect in allIncorrect)
        {
            incorrect.SetActive(false);
        }

        foreach (GameObject tile in allTiles)
        {
            tile.SetActive(false);
        }

        if (result == "win")
        {
            canvas.GetComponent<CanvasController>().GameOver("win");
        }
        else
        {
            canvas.GetComponent<CanvasController>().GameOver("lose");
        }
    }
}
