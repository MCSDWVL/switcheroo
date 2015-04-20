using UnityEngine;
using System.Collections;

public class GoombaWalk : MonoBehaviour 
{
	public float Speed = 1f;
	public bool StartRight = false;
	public bool IgnoreReversalZones = false;

	private bool movingRight;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () 
	{
		movingRight = StartRight;
		rb = gameObject.GetComponent<Rigidbody2D>();

		GetComponent<Animator>().speed = .05f * Speed;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		// cast a ray to make sure it's at "eye level" (don't switch directions for landing on the floor)
		var hitInMovementDirection = Physics2D.Raycast(transform.position, movingRight?Vector2.right:-Vector2.right, 1.5f);
		if (hitInMovementDirection)
		{
			Debug.Log("Collided with " + other.gameObject + "ray check hit " + hitInMovementDirection.collider.gameObject);
			movingRight = !movingRight;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		var vel = rb.velocity;
		vel.x = (movingRight ? 1 : -1) * Speed;
		rb.velocity = vel;
	}
}
