using UnityEngine;
using System.Collections;

public class LevelProgressionManager : MonoBehaviour 
{
	public bool HackLoadFirstLevel;
	private static bool AlreadyExists = false;
	void Start () 
	{
		if (AlreadyExists)
			GameObject.Destroy(this.gameObject);
		else
		{
			DontDestroyOnLoad(this.gameObject);
			if(HackLoadFirstLevel)
				Application.LoadLevel(1);
			AlreadyExists = true;
		}
	}

	public static void LoadNextLevel()
	{
		Application.LoadLevel(Application.loadedLevel + 1);
	}
}
