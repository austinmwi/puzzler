using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class View : MonoBehaviour 
{
	public bool toggle_fade	= false;

	public static RaycastHit hit;

	void Start()
	{
		FadeEffect.SetBlack();
		toggle_fade = true;

		if (Application.isEditor)
		{
			gameObject.AddComponent<KeyboardMovement>();
		}
	}

	void LateUpdate() 
	{
		GvrViewer.Instance.UpdateState();
	}		

	void OnGUI()
	{
		if(!toggle_fade)
		{
			FadeEffect.FadeOut();
		}

		if(toggle_fade)
		{
			FadeEffect.FadeIn();
		}
	}
}
