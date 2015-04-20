using UnityEngine;
using System.Collections;

public class CustomCamFollow : MonoBehaviour 
{
	public Transform FollowTarget;
	public float LerpTime = 1f;

	private float lerpTimeLeft { get; set; }
	private Vector2 lerpStartPosition { get; set; }

	public void AskForLerp() { lerpStartPosition = transform.position; lerpTimeLeft = LerpTime; }

	void LateUpdate()
	{
		var newPos = transform.position;
		if (lerpTimeLeft > 0)
		{
			newPos.x = Mathf.Lerp(lerpStartPosition.x, FollowTarget.transform.position.x, (LerpTime - lerpTimeLeft) / LerpTime);
			lerpTimeLeft -= Time.deltaTime / Time.timeScale;
		}
		else
		{
			newPos.x = FollowTarget.transform.position.x;
		}

		transform.position = newPos;
	}
}
