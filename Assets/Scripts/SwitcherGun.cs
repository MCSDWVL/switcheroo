using UnityEngine;
using System.Collections;

public class SwitcherGun : MonoBehaviour {
	public Transform ReticleObject;
	public PlayerMovementController Player;
	public RayFader RayPrefab;

	public AudioClip PewNoise;

	int layerMask;

	void Start()
	{
		layerMask = Physics.kDefaultRaycastLayers & ~(1 << LayerMask.NameToLayer("EnemyEdgeBlocker")) & ~(1 << LayerMask.NameToLayer("IgnoreSwitchGun")) & ~(1 << LayerMask.NameToLayer("EnemyOnlyPassage"));
		// hide the cursor cuz we got a perty reticle
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update () 
	{
		HandleFiringUpdate();
	}

	void LateUpdate()
	{
		HandleCursorPositionUpdate();
	}

	void HandleCursorPositionUpdate()
	{
		var mouseScreenPos = Input.mousePosition;
		mouseScreenPos.z = 10f;
		var mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
		mouseWorldPos.z = -5f;
		ReticleObject.position = mouseWorldPos;
	}

	private bool fireNextUpdate = false;
	void HandleFiringUpdate()
	{
		var firing = Input.GetButtonDown("Fire1");
		if (firing)
		{
			fireNextUpdate = true;
		}
	}

	void FixedUpdate()
	{
		if (fireNextUpdate)
		{		
			fireNextUpdate = false;
			HandleWeaponFired();
		}
	}

	void HandleWeaponFired()
	{
		var direction = ReticleObject.position - Player.transform.position;
		var playerPos = Player.transform.position;
		var hit = Physics2D.Raycast(playerPos, direction, 100, layerMask);
		if (hit)
		{
			SwitchTarget hitTarget = null;
			if (hitTarget = hit.collider.gameObject.GetComponent<SwitchTarget>())
			{
				// exchange positions
				Player.transform.position = hitTarget.transform.position;
				hitTarget.transform.position = playerPos;

				// exchange velocities?  TODO: this might be modified by some more advanced mechanic
				var playerVel = Player.GetComponent<Rigidbody2D>().velocity;
				Player.GetComponent<Rigidbody2D>().velocity = hitTarget.GetComponent<Rigidbody2D>().velocity;
				Player.PostWarpCooldown = true;
				hitTarget.GetComponent<Rigidbody2D>().velocity = playerVel;

				// PEW PEW FX !!
				Player.WarpFX.Stop();
				Player.WarpFX.Clear();
				Player.WarpFX.Play();
				Player.OnWarped();
				hitTarget.WarpFX.Stop();
				hitTarget.WarpFX.Clear();
				hitTarget.WarpFX.Play();

				// tell the camera we got warpy
				Camera.main.GetComponent<CustomCamFollow>().AskForLerp();
			}
			var newRayIndicator = GameObject.Instantiate(RayPrefab);
			newRayIndicator.Begin(playerPos, hit.point);
			Player.PlayerAudio.PlayOneShot(PewNoise);
		}
	}
}
