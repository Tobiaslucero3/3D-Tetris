﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    float lastFall = 0;

    private MainCamera cam;

    private Vector3 cameraPos;

    private bool inBetweenBorders = false; // This tells us if we are currently in between borders

    private int childWithTransform; // This is the child that usually has the piece transform associated with it

    // Start is called at the beggining
    void Start()
    {

        // Default position not valid? Then it's game over
        if (IsValidGridPos() != 1)
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }

        cam = FindObjectOfType<MainCamera>();

        cameraPos = cam.transform.position;

        for (int i = 0; i < transform.childCount; ++i)
        {
            if ((transform.GetChild(i).position.x == transform.position.x) &&
               (transform.GetChild(i).position.y == transform.position.y))
            {
                childWithTransform = i;
                break;
            }
        }

       // Debug.Log(childWithTransform);
    }

    private int IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Playfield.RoundVec2(child.position);
            child.position = v;

            // Not inside border?
            if (!Playfield.InsideBorder(v))
            {
                if (Playfield.OutsideBorder(v))
                {
                    return 2;
                }
                return 0;
            }
            // Block in grid cell (and not part of same group)?
            if (Playfield.grid[(int)v.x, (int)v.y] != null &&
                Playfield.grid[(int)v.x, (int)v.y].parent != transform)
            {
                return 0;
            }
        }
        return 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            cam.transform.position += new Vector3(-1, 0, 0);

            // If the method returns 2 or we are in between borders
            if ((IsValidGridPos() == 2) || (inBetweenBorders))
            {
                inBetweenBorders = true; // We are still in between borders
                UpdateGridInBetweenBorders(); // Update in between borders
            }
            // The position is valid and not between borders
            else if (IsValidGridPos() == 1)
            {
                UpdateGrid(); // Update the grid since it is valid
            }
            else
            {
                transform.position += new Vector3(1, 0, 0); // Revert
                cam.transform.position += new Vector3(1, 0, 0);
            }
        }
        // Move right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Modify position
            transform.position += new Vector3(1, 0, 0);
            cam.transform.position += new Vector3(1, 0, 0);

            // If the method returns 2 or we are in between borders
            if ((IsValidGridPos() == 2) || (inBetweenBorders))
            {
                inBetweenBorders = true;
                UpdateGridInBetweenBorders();
            }
            // Otherwise the position is entirely valid and not between borders
            else if (IsValidGridPos() == 1)
            {
                UpdateGrid(); // Update the grid since it is valid
            }
            else
            {
                transform.position += new Vector3(-1, 0, 0); // Its not valid so revert to original position
                cam.transform.position += new Vector3(-1, 0, 0);
            }
        }
        // Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (inBetweenBorders)
                RotateInBetweenBorders();
            else
                transform.Rotate(0, 0, -90); // Rotate

            // If the method returns 2 or we are in between borders
            if (IsValidGridPos() == 2)
            {
                UpdateGridInBetweenBorders();
            }
            // Otherwise the position is entirely valid and not between borders
            else if (IsValidGridPos() == 1)
            {

                UpdateGrid(); // Update the grid since it is valid
            }
            else
            {
                transform.Rotate(0, 0, 90); // Its not valid so revert to original position
            }

        }
        // Fall
        else if (Input.GetKeyDown(KeyCode.DownArrow) || (Time.time - lastFall >= 2.0))
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            // See if the new position is valid
            if (IsValidGridPos() == 1)
            {
                UpdateGrid(); // Update the grid since it is valid
            }
            else if(IsValidGridPos() == 2)
            {
                UpdateGridInBetweenBorders();
            }
            else
            {
                transform.position += new Vector3(0, 1, 0); // Revert

                // Clear filled horizontal lines
                Playfield.DeleteFullRows();

                // Spawn next group
                FindObjectOfType<Spawner>().SpawnNext();

                // Disable script
                enabled = false;

                cam.transform.position = cameraPos;
                /*
                if ((transform.position.x < 7) || (transform.position.x > 23))
                {
                    // Fix the sides
                    //FindObjectOfType<Board>().FixSides();
                }*/
            }
            lastFall = Time.time; // Update this as last time decreased y position
        }
        // Immediate drop
        else if (Input.GetKeyDown(KeyCode.Space)) // Immediate drop
        {
            // Loop while making sure the position is valid and adding a vector in the downward direction
            while (IsValidGridPos() == 1)
            {
                transform.position += new Vector3(0, -1, 0); // Send it one down
            }

            transform.position += new Vector3(0, 1, 0); // Revert by one step since we went too far

            // Update the grid
            UpdateGrid();

            // Clear filled horizontal lines
            Playfield.DeleteFullRows();

            // Spawn next group
            FindObjectOfType<Spawner>().SpawnNext();

            // Disable script
            enabled = false;

            cam.transform.position = cameraPos;
            /*
            if ((transform.position.x < 7) || (transform.position.x > 23))
            {
                // Fix the sides
                FindObjectOfType<Board>().FixSides();
            }*/
        }
    }

    // This method runs whenever the user tries to rotate the piece while being between the borders
    void RotateInBetweenBorders()
    {
        int transformPosX = (int)Mathf.Round(transform.position.x); // Get the transform position

        Vector2 v;

        bool left = transformPosX < 4; // Whether the group transform is to the left of the board or not
        foreach (Transform child in transform)
        {
            v = Playfield.RoundVec2(child.position);

            // If the Group transform is to the left and the current child is on the right side of the board
            if (left && (v.x > Playfield.w - 4))
            {
                v = new Vector2(-(Playfield.w - (int)v.x), v.y); // Put the child on the left side of the board where it would be
            }
            // Otherwise if the group transform is to the rigth and the current child is on the left side
            else if (!left && (v.x < 4))
            {
                v = new Vector2(Playfield.w + (int)v.x, v.y); // Put child on the right side
            }

            child.position = v; // Set the child position to the vector
        }

        // Actually rotate now. UpdateGridBetweenBorders() will do the rest of the work for us
        transform.Rotate(0, 0, -90);

    }

    // If the group is currently in between borders this is the method that runs
    void UpdateGridInBetweenBorders()
    {
        // Remove old children from the grid
        for (int y = 0; y < Playfield.h; ++y)
        {
            for (int x = 0; x < Playfield.w; ++x)
            {
                if (Playfield.grid[x, y] != null)
                {
                    // Checks if a block belongs to this current group 
                    if (Playfield.grid[x, y].parent == transform)
                    {
                        Playfield.grid[x, y] = null;
                    }
                }
            }
        }
        Vector2 v = Playfield.RoundVec2(transform.position);

        int transformPosX = (int)v.x;

        if ((transformPosX < 0) || (transformPosX >= Playfield.w))
        {
            Debug.Log("Running");
            /*
            // Get the x component of the piece that belonged to the actual transform
            int childPosX = (int)Mathf.Round(transform.GetChild(childWithTransform).transform.position.x);

            int move = 0; // Will use this vector to move all the small block transforms

            v = Playfield.RoundVec2(transform.position);

            move = childPosX - (int)v.x; // The amount we have to compensate is the difference between my child's
            // position and my current position

            Debug.Log(move);

            v.x = childPosX; // Set the new transform position to the old child position
            */
            

            if (transform.position.x >= Playfield.w)
            {
                transform.position -= new Vector3(Playfield.w, 0);
            }
            else
            {
                transform.position -= new Vector3(Playfield.w, 0);
            }
            cam.transform.position = new Vector3(transform.position.x, cam.transform.position.y, cam.transform.position.z);
           // inBetweenBorders = false; // Do this to verify that the piece is still in between the border
        }

        // Add new children to the grid
        foreach (Transform child in transform)
        {
            v = Playfield.RoundVec2(child.position);

            Debug.Log(v.x + " " + v.y);

            // If outside the left border sets the x component to the width
            if (v.x < 0)
            {
                inBetweenBorders = true;
                v.x = Playfield.w + v.x; // putting it on the right side
              //  child.position = v;
            }
            // If outside the rigth border sets the x component to zero
            else if (v.x >= Playfield.w)
            {
                inBetweenBorders = true;
                v.x = v.x - Playfield.w; // putting it on the left side
               // child.position = v;
            }
            Playfield.grid[(int)v.x, (int)v.y] = child; // Back into the grid at new position
        }
           // transform.position = v; // Set the transform position to our vector     
            /*
            // Now we actually go to every child and perform the transform to move it to the right place
            foreach (Transform child in transform)
            {
                child.position -= new Vector3(move, 0, 0);
            }*/
    }

    void UpdateGrid()
    {
        // Remove old children from the grid
        for (int y = 0; y < Playfield.h; ++y)
        {
            for (int x = 0; x < Playfield.w; ++x)
            {
                if (Playfield.grid[x, y] != null)
                {
                    // Checks if a block belongs to this current group 
                    if (Playfield.grid[x, y].parent == transform)
                    {
                        Playfield.grid[x, y] = null;
                    }
                }
            }
        }

        // Add new children to the grid
        foreach (Transform child in transform)
        {
            //Debug.Log(child.position.x + " " + child.position.y);
            Vector2 v = Playfield.RoundVec2(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }
    }
}
