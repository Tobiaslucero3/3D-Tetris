using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    Group parent;

    private void Start()
    {
        UpdateShadow();
    }

    void UpdateShadow()
    {

        transform.position = Playfield.RoundVec2(new Vector2(transform.position.x, 0));
        
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.RoundVec2(child.position);
            /*
            if (v.x < 0)
            {
                Debug.Log(child.position.x + " " + v.x);
                child.position = new Vector2(Playfield.w - 1, v.y);
            }
            else if (v.x >= Playfield.w)
            {
                child.position = new Vector2(0, v.y);
            }*/
            if (v.y < 0)
            {
                transform.position += new Vector3(0, 1);
            }
        }

        int childCount = transform.childCount;
        int j = 0;
        bool done = false;
        int pos = 0;
        int posx = 0;

        while ((!done) && (transform.position.y < Playfield.h))
        {
            foreach (Transform child in transform)
            {
                pos = (int)Mathf.Round(child.position.y);
                posx = (int)Mathf.Round(child.position.x);
                if (Playfield.grid[posx, pos] != null)
                {
                    transform.position += new Vector3(0, 1);
                    break;
                }
                else
                {
                    if (j == childCount - 1)
                    {
                        done = true;
                    }
                }
                ++j;
            }
            j = 0;
        }

        int move = 0;
        foreach (Transform child in transform)
        {
            posx = (int)Mathf.Round(child.position.x);
            pos = (int)Mathf.Round(child.position.y);
            for (int i = pos + 1; i < parent.GetYPos() - 1; ++i)
            {
                //Debug.Log("ran");
                if (Playfield.grid[posx, i] != null)
                {
                    if ((i + 1) - pos > move)
                    {
                        move = (i + 1) - pos;
                    }
                }
            }
        }
        transform.position += new Vector3(0, move);
    }

    public void AssignGroup(Group group)
    {
        parent = group;
    }

    public void UpdateShadowRotate(int degree)
    {
        transform.Rotate(0, 0, degree);
        UpdateShadow();
    }

    public void UpdateShadowMove(int x)
    {
        transform.position += new Vector3(x, 0);
        UpdateShadow();
    }

    public void DestroyShadow()
    {
        Destroy(gameObject);
        Destroy(this);
    }

}

