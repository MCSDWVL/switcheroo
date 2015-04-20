using UnityEngine;
using System.Collections;

public class SlowTime : MonoBehaviour 
{
	public float SlowTimeMultiplier = .3f;
	private AudioSource audioSource;
	public AudioClip SlowSound;
	public AudioClip UnslowSound;
	private bool timeIsSlowed = false;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () 
	{
		var buttonHeld = Input.GetButton("Fire2");
		if (buttonHeld && !timeIsSlowed)
		{
			timeIsSlowed = true;
			Time.timeScale = SlowTimeMultiplier;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
			audioSource.PlayOneShot(SlowSound);
		}
		else if(!buttonHeld && timeIsSlowed)
		{
			timeIsSlowed = false;
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
			audioSource.PlayOneShot(UnslowSound);
		}

		
	}
}
