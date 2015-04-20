using UnityEngine;
using System.Collections;

public class EnemyCounter : MonoBehaviour 
{
	public int RequiredCount = 0;
	public Transform MoveOnCountReached;
	public Vector2 MoveOffsetOnCountReached;
	public TextMesh TargetText;
	public float OpeningTime = 1f;
	
	private int lastCount = -1;
	private Vector2 startPosition;
	private float openTime = -1f;

	
	
	// Update is called once per frame
	void Update () 
	{
		var currentCount = SwitchTarget.LivingEnemies;
		if (currentCount != lastCount)
		{
			lastCount = currentCount;
			Debug.Log("Current count " + lastCount);
			int needed = Mathf.Max(lastCount - RequiredCount, 0);
			TargetText.text = needed + "x";

			if (lastCount <= RequiredCount)
			{
				if(MoveOnCountReached)
					startPosition = MoveOnCountReached.transform.localPosition;
				openTime = Time.time;
			}
		}

		if (lastCount == RequiredCount && MoveOnCountReached != null)
		{
			MoveOnCountReached.transform.localPosition = Vector2.Lerp(startPosition, startPosition + MoveOffsetOnCountReached, (Time.time - openTime) / OpeningTime);
		}
	}
}
