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
        bool done = false;
        int childCount = transform.childCount;
        int j = 0;
        int pos = 0;
        int posx = 0;
        foreach (Transform child in transform)
        {
            pos = (int)Mathf.Round(child.position.y);
            if(pos==0)
            {
                break;
            }
            if(j == childCount - 1)
            {
                transform.position -= new Vector3(0, 1);
                break;
            }
            ++j;
        }

        j = 0;

        while ((!done) && (transform.position.y < Playfield.h))
        {
            foreach (Transform child in transform)
            {
                pos = (int)Mathf.Round(child.position.y);
                posx = (int)Mathf.Round(child.position.x);
                if(pos < 0)
                {
                    transform.position += new Vector3(0, 1);
                    break;
                }
                if (posx < 0)
                {
                    posx = Playfield.w + posx; // putting it on the right side
                }
                else if (posx >= Playfield.w)
                {
                    posx = posx - Playfield.w; // putting it on the left side
                }
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
            if (posx < 0)
            {
                posx = Playfield.w + posx; // putting it on the right side
            }
            else if (posx >= Playfield.w)
            {
                posx = posx - Playfield.w; // putting it on the left side
            }
            for (int i = pos + 1; i < parent.GetYPos() - 1; ++i)
            {
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
        FixTransform();
        UpdateShadow();
    }

    public void UpdateShadowMove(int x)
    {
        transform.position += new Vector3(x, 0);
        FixTransform();
        UpdateShadow();
    }

    public void FixTransform()
    {
        transform.position = Playfield.RoundVec2(new Vector2(transform.position.x, 0));
        if (transform.position.x < 0)
        {
            transform.position += new Vector3(Playfield.w, 0);
        }
        else if (transform.position.x >= Playfield.w)
        {
            transform.position -= new Vector3(Playfield.w, 0);
        }
    }

    public void DestroyShadow()
    {
        Destroy(gameObject);
        Destroy(this);
    }

}

