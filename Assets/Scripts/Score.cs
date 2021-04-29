using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public static int score = 0;
    public static string scoreTextString = "Score: ";
    public Text scoreText;
    //public int score;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = scoreTextString;
    }

    // Update is called once per frame
    void Update()
    {
        //Update the scoreTextString every frame
        scoreTextString = "Score: " + score.ToString();

        //Need to update the actual game text last
        scoreText.text = scoreTextString;
    }
}
