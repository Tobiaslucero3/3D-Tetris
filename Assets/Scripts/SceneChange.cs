using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
	public GameObject playButton;

	public void ChangeScene()
	{
		SceneManager.LoadScene ("SampleScene");
        Time.timeScale = Difficulty.difficulty;
	}
	public void Exit()
	{
		Application.Quit ();
	}
}