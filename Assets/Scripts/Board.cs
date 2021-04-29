using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
   // readonly int xMin = 14;
   // readonly int xMax = 16;

    // This method makes sure that the ranges x=[-6, -1]  and x=[31, 36] is populated with pieces
    public void FixSides(Transform transform)
    {
        Vector3 pos = new Vector3();

        foreach(Transform child in transform)
        {
            pos = Playfield.RoundVec2(child.position);

            pos.x += Playfield.w;

            Instantiate(child.gameObject, pos, Quaternion.identity);

            pos.x -= Playfield.w;

            pos.x = -(Playfield.w - pos.x);

            Instantiate(child.gameObject, pos, Quaternion.identity);
        }
    }
}
