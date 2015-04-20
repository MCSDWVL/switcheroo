using UnityEngine;
using System.Collections;

public class RayFader : MonoBehaviour 
{
	public float FadeTime;
	public Color BeginColor = Color.red;
	public LineRenderer Line;
	
	private float beganTime;
	private float elapsedTimeScaleIndependent;

	public void Begin(Vector2 p0, Vector2 p1)
	{
		beganTime = Time.time;
		Line.SetPosition(0, p0);
		Line.SetPosition(1, p1);
		Line.SetColors(BeginColor, BeginColor);
	}

	// Update is called once per frame
	void Update () 
	{
		elapsedTimeScaleIndependent += Time.deltaTime / Time.timeScale;
		var lerp = elapsedTimeScaleIndependent / FadeTime;
		var color = Color.Lerp(BeginColor, Color.clear, lerp);
		Line.SetColors(color, color);
		if (lerp > 1f)
		{
			GameObject.Destroy(this.gameObject);
		}
	}
}
