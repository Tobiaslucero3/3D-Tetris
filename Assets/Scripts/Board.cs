using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    readonly int xMin = 5;
    readonly int xMax = 25;

    // This method makes sure that the ranges x=[-6, -1]  and x=[31, 36] is populated with pieces
    public void FixSides()
    {
        Vector3 pos = new Vector3();
        for(int i = 0; i < 5 ; ++i)
        {
            for (int j = 0; j < Playfield.h; ++j)
            {
                if(Playfield.grid[i,j] != null)
                {
                    pos.x = Playfield.w + i;
                    pos.y = j;
                    Instantiate(Playfield.grid[i, j], pos, Quaternion.identity);
                }
            }
        }

        for (int i = 25; i < Playfield.w; ++i)
        {
            for (int j = 0; j < Playfield.h; ++j)
            {
                if (Playfield.grid[i, j] != null)
                {
                    pos.x = i - Playfield.w;
                    pos.y = j;
                    Instantiate(Playfield.grid[i, j], pos, Quaternion.identity);
                }
            }
        }
    }
}
