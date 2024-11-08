using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateOnClickScript : MonoBehaviour
{

    GameObject controller;
    public int tileValue;
    public int tileLocationX;
    public int tileLocationY;
    public int[,] allTiles;
    public GameObject[,] tileValues;

    static int[] dRow = { 0, 1, 0, -1 };
    static int[] dCol = { -1, 0, 1, 0 };

    static bool isValid(bool[,] vis, int row, int col, GameObject[,] grid)
    {

        // If cell is out of bounds
        if (row < 0 || col < 0 ||
            row >= 30 || col >= 30)
            return false;

        // If the cell is already visited
        if (vis[row, col] || grid[row,col].GetComponent<CalculateOnClickScript>().tileValue == 0)
            return false;

        // Otherwise, it can be visited
        return true;
    }

    static void DFS(int row, int col, int[,] grid, bool[,] vis, GameObject[,] gameGrid)
    {

        // Initialize a stack of pairs and
        // push the starting cell into it
        Stack st = new Stack();
        st.Push(new Tuple<int, int>(row, col));

        // Iterate until the
        // stack is not empty
        while (st.Count > 0)
        {

            // Pop the top pair
            Tuple<int, int> curr = (Tuple<int, int>)st.Peek();
            st.Pop();

            row = curr.Item1;
            col = curr.Item2;

            // Check if the current popped
            // cell is a valid cell or not
            if (!isValid(vis, row, col, gameGrid))
                continue;

            // Mark the current
            // cell as visited
            vis[row, col] = true;

            // Print the element at
            // the current top cell
            Debug.Log(grid[row, col] + " ");

            // Push all the adjacent cells
            for (int i = 0; i < 4; i++)
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
    }

    private void OnMouseDown()
    {
        GameScript gs = controller.GetComponent<GameScript>();
        bool[,] vis = new bool[30, 30];

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                vis[i, j] = false;
            }
        }

        DFS(tileLocationX, tileLocationY, allTiles, vis, tileValues);
    }
}
