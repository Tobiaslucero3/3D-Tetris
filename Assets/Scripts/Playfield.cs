using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static int w = 30;
    public static int h = 15;
    public static Transform[,] grid = new Transform[w, h]; // This matrix will keep track
    // of all of the squares in the stage (from (0,0) to (9, 19)) and will store if there is a block on it 

    // This function rounds a vec2 so [1.000001, 2.00002] = [1,2]
    public static Vector2 RoundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    // This functions check whether the coordinate sent in is within the borders or not
    public static bool InsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.y >= 0 && (int)pos.x < w);
    }

    // Checks if the point is outside the left or right borders
    public static bool OutsideBorder(Vector2 pos)
    {
        return (((int)pos.x < 0) || ((int)pos.x >= w));
    }

    public static void DeleteRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    // Decreases a row on a level
    // Decreases row y to row y - 1
    public static void DecreaseRow(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            // Check if there is a block in the x and y position
            if (grid[x, y] != null)
            {
                // Move one towards the bottom
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                // Update Block position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    // Decreases all the rows above one that is deleted
    public static void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < h; ++i)
        {
            DecreaseRow(i);
        }
    }

    // Checks if a row is full
    public static bool IsRowFull(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            if (grid[x, y] == null)
                return false;
        }

        return true;
    }

    // This function will delete all the full rows on the board
    public static void DeleteFullRows()
    {
        for (int y = 0; y < h; ++y)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                --y;
            }
        }

    }

}
