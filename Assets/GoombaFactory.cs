using UnityEngine;
using System.Collections;

public class GoombaFactory : MonoBehaviour 
{
	public Rigidbody2D Prefab;

	public Vector2 StartingVelocity;

	private Rigidbody2D instance;

	
	
	// Update is called once per frame
	void Update () 
	{
		var spawned = false;
		if (instance == null)
		{
			instance = GameObject.Instantiate(Prefab);
			spawned = true;
		}
		else if (!instance.gameObject.activeInHierarchy)
		{
			instance.gameObject.SetActive(true);
			spawned = true;
		}

		if (spawned)
		{
			instance.transform.position = transform.position;
			instance.velocity = StartingVelocity;
		}
	}
}
