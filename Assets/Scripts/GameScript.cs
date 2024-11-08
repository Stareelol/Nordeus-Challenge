using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Windows;
using Random = UnityEngine.Random;

public class GameScript : MonoBehaviour
{

    public GameObject tile;
    public int[,] tiles;
    public string result;
    public GameObject[,] gameTiles = new GameObject[30, 30];
    public Color newColor;

    void Start()
    {
        StartCoroutine(GetRequest("https://jobfair.nordeus.com/jf24-fullstack-challenge/test"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
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
                    break;
            }
        }
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

    //void PrintGameMatrix(GameObject[,] matrix)
    //{
    //    for (int i = 0; i < matrix.GetLength(0); i++)
    //    {
    //        for (int j = 0; j < matrix.GetLength(1); j++)
    //        {
    //            Debug.Log(matrix[i, j] +", i:" + i + " ,j:" + j);
    //        }
    //    }
    //}

    public void PrintMatrix(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Debug.Log(matrix[i, j] + ", i:" + i + " ,j:" + j);
            }
        }
    }

    Color GetColor(GameObject tile, int value)
    {
        Color lowElevationColor = new Color(0.54f, 0.80f, 0.47f);
        Color midLowElevationColor = new Color(1f, 0.98f, 0.75f);
        Color midHighElevationColor = new Color(1f, 0.86f, 0.56f);
        Color highElevationColor = new Color(0.63f, 0.52f, 0.37f);
        Color veryHighElevationColor = Color.white;
        float newValue;

        if (value <= 250)
        {
            newValue = value / 250f;
            newColor = Color.Lerp(lowElevationColor, midLowElevationColor, newValue);
            Debug.Log(newValue + " , " +  newColor);
        }

        if (value > 250 && value <=500)
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

    void InititateTiles(string result)
    {
        float posx;
        float posy = 7.25f;
        float offsetX,offsetY;

        tiles = CreateMatrix(result);

        for (int i = 0; i < 30; i++)
        {
            if (i == 0) offsetY = 0;
            else offsetY = 0.5f;
            posy -= offsetY;
            posx = -7.497f;
            for (int j = 0; j < 30; j++)
            {
                if (j == 0) offsetX = 0;
                else offsetX = 0.5f;
                posx += offsetX;
                GameObject newTile = Instantiate(tile, new Vector3(posx, posy, 0), Quaternion.identity);
                gameTiles[i,j] = newTile;
                newTile.GetComponent<CalculateOnClickScript>().tileValue = tiles[i, j];
                newTile.GetComponent<CalculateOnClickScript>().tileLocationX = j;
                newTile.GetComponent<CalculateOnClickScript>().tileLocationY = i;
                if (tiles[i, j] == 0) tile.GetComponent<SpriteRenderer>().color = new Color(0 / 255f, 105 / 255f, 148 / 255f);
                else if (tiles[i, j] >= 0) tile.GetComponent<SpriteRenderer>().color = GetColor(tile, tiles[i, j]);
            }
        }
    }
}
