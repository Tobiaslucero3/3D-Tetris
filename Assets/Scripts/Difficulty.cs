using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    public static int difficulty = 1;
    public static string difficultyTextString = "DIFFICULTY: ";

    public Text difficultyText;

    public void incDifficulty ()
    {
        if (difficulty == 10)
        {
            return;
        }
        difficulty++;
    }

    public void decDifficulty ()
    {
        if(difficulty == 1)
        {
            return;
        }
        difficulty--;
    }

    void Start()
    {
        difficultyText.text = difficultyTextString;
    }
    void Update()
    {
        difficultyTextString = "DIFFICULTY: " + difficulty.ToString();
        difficultyText.text = difficultyTextString;
    }
}
