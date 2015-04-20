using UnityEngine;
using System.Collections;

public class SwitchTarget : Killable 
{
	public static int LivingEnemies { get; set; }

	public ParticleSystem WarpFX;

	// Use this for initialization
	void OnEnable() 
	{
		LivingEnemies++;
	}
	
	public void OnDisable()
	{
		LivingEnemies--;
	}

	public override void OnKilled()
	{
		base.OnKilled();
		gameObject.SetActive(false);
		
	}

	
	
	// Update is called once per frame
	void Update () {
	
	}
}
