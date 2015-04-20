using UnityEngine;
using System.Collections;

public class RemoveRenderOnStart : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
		foreach (var rend in gameObject.GetComponents<Renderer>())
			GameObject.Destroy(rend);
	}
}
