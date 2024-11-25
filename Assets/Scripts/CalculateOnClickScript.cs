using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class CalculateOnClickScript : MonoBehaviour
{

    GameObject controller;
    GameObject outline;
    GameObject canvas;
    public int tileValue;
    public int tileLocationX;
    public int tileLocationY;
    public int[,] allTiles;
    public GameObject[,] tileValues;
    public static int dfsSum = 0;
    public static int dfsCount = 0;
    public static float hoverScale = 1.3f;
    static int[] dRow = { 0, 1, 0, -1, -1, 1, 1, -1 };
    static int[] dCol = { -1, 0, 1, 0, 1, -1, 1, -1 };
    public int health;

    static bool IsValid(bool[,] vis, int row, int col, GameObject[,] grid)
    {
        if (row < 0 || col < 0 ||
            row >= 30 || col >= 30)
            return false;

        if (vis[row, col] || grid[row,col].GetComponent<CalculateOnClickScript>().tileValue == 0)
            return false;

        return true;
    }

    static void DFS(int row, int col, int[,] grid, bool[,] vis, GameObject[,] gameGrid)
    {
        Stack st = new();
        st.Push(new Tuple<int, int>(row, col));

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

            for (int i = 0; i < 8; i++)
            {
                int adjx = row + dRow[i];
                int adjy = col + dCol[i];
                st.Push(new Tuple<int, int>(adjx, adjy));
            }
        }
    }

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        allTiles = controller.GetComponent<GameScript>().tiles;
        tileValues = controller.GetComponent<GameScript>().gameTiles;
        outline = controller.GetComponent<GameScript>().outline;
        canvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    private void OnMouseDown()
    {
        GameScript gs = controller.GetComponent<GameScript>();
        GameObject parent = null;
        bool[,] vis = new bool[30, 30];
        float islandSum = 0;
        dfsSum = 0;
        dfsCount = 0;

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                vis[i, j] = false;
            }
        }
        DFS(tileLocationY, tileLocationX, allTiles, vis, tileValues);

        if (dfsCount != 0) islandSum = (float)dfsSum / dfsCount;

        if (tileValue != 0) parent = transform.parent.gameObject;

        if(parent != null && !parent.CompareTag("AlreadySelected"))
        {
            if (gs.GetComponent<GameScript>().largestIslandSum == islandSum)
            {
                gs.GameOver("win");
                //Debug.Log("Correct island! + " + islandSum);
            }
            else
            {
                foreach (Transform child in parent.transform)
                {
                    GameObject newOutline = Instantiate(outline, new Vector3(child.transform.position.x, child.transform.position.y, -1), Quaternion.identity);
                    newOutline.GetComponent<SpriteRenderer>().color = new Color(204f / 255f, 0 / 255f, 0 / 255f, 0.9f);
                    newOutline.tag = "Incorrect";
                    parent.tag = "AlreadySelected";
                }

                GameObject[] outlines = GameObject.FindGameObjectsWithTag("Outline");

                foreach (GameObject go in outlines)
                {
                    Destroy(go);
                }

                //Debug.Log("Incorrect island! + " + islandSum);

                gs.health--;
                canvas.GetComponent<CanvasController>().ChangeHealth(gs.health);
                if (gs.health <= 0)
                {
                    gs.GameOver("lose");
                }
            }
        }
    }

    void OnMouseEnter()
    {
        if (tileValue != 0)
        {
            GameObject parent = transform.parent.gameObject;
            if (!parent.CompareTag("AlreadySelected"))
            {
                foreach (Transform child in parent.transform)
                {
                    GameObject newOutline = Instantiate(outline, new Vector3(child.transform.position.x, child.transform.position.y, -1), Quaternion.identity);
                    newOutline.GetComponent<SpriteRenderer>().color = new Color(50f / 255f, 48f / 255f, 56f / 255f, 0.45f);
                }
            }
        }
    }

    void OnMouseExit()
    {
        GameObject[] outlines = GameObject.FindGameObjectsWithTag("Outline");

        foreach(GameObject go in outlines)
        {
            Destroy(go);
        }
    }
}
