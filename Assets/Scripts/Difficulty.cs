using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty : MonoBehaviour
{
    public static int difficulty = 1;
    public static string difficultyTextString = "Difficulty: ";

    public Text difficultyText;

    public void incDifficulty ()
    {
        difficulty++;
    }

    public void decDifficulty ()
    {
        difficulty--;
    }

    void Start()
    {
        difficultyText.text = difficultyTextString;
    }
    void Update()
    {
        difficultyTextString = "Difficulty: " + difficulty.ToString();
        difficultyText.text = difficultyTextString;
    }
}
