using System;
using UnityEngine;

public class PlayerMovementController : Killable
{
	public float JumpForce = 1f;
	public Transform GroundCheckTransform;
	public float GroundCheckRadius = .1f;

	public float TurnReactivityMultiplier = 1f;
	public float MaxHorizontalVelocity = 50f;
	public float HorizontalAcceleration = 1f;
	public float AirAcceleration = 1f;
	public float MaxDirectinChangetime = .1f;

	public ParticleSystem WarpFX;

	public AudioSource PlayerAudio;
	public AudioClip WarpSound;
	public AudioClip JumpSound;
	public AudioClip ScreechSound;

	private bool grounded;
	private bool jump;
	private Rigidbody2D rigidBody;
	private bool changingDirection;
	private float directionChangeTime = 0f;
	private float directionChangeStartVelocity = 0f;

	private Animator playerAnimator;

	private bool jumpedOnPurpose = false;
	private float fallForgiveness = 0;

	private float horInput = 0f;
	
	private void Start()
	{
		rigidBody = gameObject.GetComponent<Rigidbody2D>();
		PlayerAudio = gameObject.GetComponent<AudioSource>();
		playerAnimator = GetComponent<Animator>();
	}

	private void Update()
	{
		fallForgiveness -= Time.deltaTime;
		if (!jump)
		{
			// Read the jump input in Update so button presses aren't missed.
			jump = Input.GetButtonDown("Jump");
			jumpedOnPurpose = true;
		}
		PostWarpCooldown = false;
		horInput = Input.GetAxis("Horizontal");

		// flip
		if (rigidBody.velocity.x * gameObject.transform.localScale.x < 0)
		{
			var scale = gameObject.transform.localScale;
			scale.x = -scale.x;
			gameObject.transform.localScale = scale;
		}

		if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Escape))
			OnKilled();
	}

	public bool PostWarpCooldown = false;
	private bool wasGrounded = false;
	private void FixedUpdate()
	{
		if (PostWarpCooldown)
			return;

		grounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheckTransform.position, GroundCheckRadius, Physics.kDefaultRaycastLayers);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				grounded = true;
				jumpedOnPurpose = jump;
				break;
			}
		}

		if (wasGrounded != grounded && !jumpedOnPurpose)
			fallForgiveness = .1f;

		// Read the inputs.
		//bool crouch = Input.GetKey(KeyCode.LeftControl);
		//float h = CrossPlatformInputManager.GetAxis("Horizontal");
		// Pass all parameters to the character control script.
		//m_Character.Move(h, crouch, m_Jump);
		//m_Jump = false;
		if ((grounded || (!jumpedOnPurpose && fallForgiveness > 0)) && jump)
		{
			PlayerAudio.PlayOneShot(JumpSound);	
			rigidBody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
			jump = false;
		}

		var currentHorizontalVelocity = rigidBody.velocity.x;
		var targetVelocity = currentHorizontalVelocity;
		var inputIsOppositeVelocity = currentHorizontalVelocity * horInput < 0;
		var noMovementInput = horInput * horInput < Mathf.Epsilon;

		if (noMovementInput && currentHorizontalVelocity > Mathf.Epsilon)
		{
			targetVelocity += (targetVelocity > 0 ? -1 : 1) * HorizontalAcceleration;
		}
		if (grounded && changingDirection && (directionChangeTime > MaxDirectinChangetime || currentHorizontalVelocity * currentHorizontalVelocity < Mathf.Epsilon))
		{
			PlayerAudio.PlayOneShot(ScreechSound);	
			targetVelocity = -directionChangeStartVelocity;
			changingDirection = false;
		}
		else if (grounded && inputIsOppositeVelocity && (MaxHorizontalVelocity - Math.Abs(currentHorizontalVelocity) < Mathf.Epsilon))
		{	
			if (!changingDirection)
				directionChangeStartVelocity = currentHorizontalVelocity;
			
			changingDirection = true;
			directionChangeTime += Time.deltaTime;
		}
		else if (grounded)
		{
			// Just accelerating up normally
			targetVelocity = currentHorizontalVelocity + horInput * HorizontalAcceleration * Time.deltaTime;
		}
		else
		{
			targetVelocity = currentHorizontalVelocity + horInput * AirAcceleration * Time.deltaTime;
		}

		if (targetVelocity*targetVelocity < currentHorizontalVelocity*currentHorizontalVelocity || targetVelocity*targetVelocity < MaxHorizontalVelocity*MaxHorizontalVelocity)
		{
			var newCompoundVelocity = new Vector2(targetVelocity, rigidBody.velocity.y);
			rigidBody.velocity = newCompoundVelocity;
		}

		playerAnimator.SetBool("Grounded", grounded);
		playerAnimator.SetFloat("Speed", Mathf.Abs(rigidBody.velocity.x));

		
	}

	public override void OnKilled()
	{
		base.OnKilled();
		Application.LoadLevel(Application.loadedLevel);
	}

	public void OnWarped()
	{
		PlayerAudio.PlayOneShot(WarpSound);
	}
}

