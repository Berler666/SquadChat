using UnityEngine;
using System.Collections;

public class ChnageScene : MonoBehaviour {

	public void NextLevelButton(int index)
	{
		Application.LoadLevel(index);
	}
	
	public void NextLevelButton(string levelName)
	{
		Application.LoadLevel(levelName);
	}

	public void CloseApp()
	{
		Application.Quit();
	}



}
