using UnityEngine;
using System.Collections;

public class InfoCanvas : MonoBehaviour
{
	//list of collision objects (one is already attached to the prefab)
	public GameObject[] 			collision;
	
	private static CanvasRenderer[]	_canvas_renderer;

	private static float 			_opacity		= 1.0f;
	private static bool 			_fade			= false;
	private static float 			_fade_rate		= 0.125f;

	//should I add a fade delay? hmm...

	void Start()
	{
		_canvas_renderer = gameObject.GetComponentsInChildren<CanvasRenderer>();
	}

	void LateUpdate()
	{	
		//check to see if we are looking at one of this objects colliders
		_fade	= false;
		for(int i = 0; i < collision.Length; i++)
		{
			if(collision[i] != null)
			{
				if(View.hit.rigidbody == collision[i].GetComponent<Rigidbody>())
				{
					_fade = true;
				}
			}
		}	

		Fade();
	}

	private void Fade()
	{
		//fade to opaque or invisible based on the _fade state
		_opacity = !_fade ? Mathf.Lerp(_opacity, 0.0f, _fade_rate) : Mathf.Lerp(_opacity, 1.0f, _fade_rate);
		_opacity = Mathf.Clamp01(_opacity);
		
		for(int i = 0; i < _canvas_renderer.Length; i++)
		{
			_canvas_renderer[i].SetAlpha(_opacity);
		}
	}
}
