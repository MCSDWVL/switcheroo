using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour 
{

	void OnTriggerEnter2D(Collider2D other)
	{
		PlayerMovementController player = null;
		if (player = other.gameObject.GetComponent<PlayerMovementController>())
		{
			LevelProgressionManager.LoadNextLevel();
		}
	}
}
