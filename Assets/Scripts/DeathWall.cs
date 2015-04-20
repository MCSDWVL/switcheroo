using UnityEngine;
using System.Collections;

public class DeathWall : MonoBehaviour 
{
	public ParticleSystem DeathFx;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Killable killable;
		if (killable = other.gameObject.GetComponent<Killable>())
		{
			killable.OnKilled();
			DeathFx.Stop();
			DeathFx.Clear();
			DeathFx.Play();
		}
	}
}
