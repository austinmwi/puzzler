using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class Waypoint : MonoBehaviour
{
	public bool			occupied							= false;
	public bool			active								= false;
	public bool			triggered							= false;

	public Color		active_color						= new Color(0.0f, 1.0f, 0.0f, 0.5f);
	public Color		hilight_color						= new Color(0.8f, 0.8f, 1.0f, 0.125f);
	public Color		disabled_color						= new Color(0.25f, 0.25f, .25f, 0.125f);

	public float		animation_scale						= 1.5f;
	public float		animation_speed						= 3.0f;

	private	Vector3		_origional_scale					= Vector3.one;

	private float		_hilight							= 0.0f;
	private float		_hilight_fade_speed					= 0.05f;

	public Rigidbody	rigid_body;
	private Material	_material;

	public Vector3		position							= Vector3.zero;

	public Waypoint[] neighbor;


	void Awake()
	{
		rigid_body	 										= gameObject.GetComponent<Rigidbody>();
		_material											= gameObject.GetComponent<MeshRenderer>().material;
		_origional_scale									= transform.localScale;

		if(position == Vector3.zero)
		{
			position = gameObject.transform.position;
		}

		UpdateActivation();
	}


	void LateUpdate()
	{
		if(active && !occupied)
		{
			Animate();
		}
		else
		{
			Deactivate();
		}
	}


	public void UpdateActivation()
	{
		Deactivate();

		for(int i = 0; i < neighbor.Length; i++)
		{
			if(neighbor[i].occupied == true)
			{
				Activate();
			}
		}
	}


	public void Occupy()
	{
		occupied	= true;
	}

	public void Exit()
	{
		occupied	= false;		
	}


	public void Activate()
	{
		_material.color			= active_color;
		transform.localScale	= _origional_scale;
		
		active					= true;
	}


	public void Deactivate()
	{
		_material.color			= disabled_color;
		transform.localScale	= _origional_scale * 0.5f;
		
		active					= false;
		triggered 				= false;
	}


	public void Trigger()
	{
		triggered	= true;
		occupied	= false;
		_hilight	= 1.0f;
	}


	public void Hilight()
	{
		_hilight = 0.5f;
	}


	private void Animate()
	{
		float pulse_animation	= Mathf.Abs(Mathf.Cos(Time.time * animation_speed));
		
		_material.color			= Color.Lerp(active_color, hilight_color, _hilight);
			
		_hilight 				= Mathf.Max(_hilight - _hilight_fade_speed, 0.0f);
		Vector3 hilight_scale	= Vector3.one * _hilight;

		transform.localScale	= Vector3.Lerp(_origional_scale + hilight_scale, _origional_scale * animation_scale + hilight_scale, pulse_animation);
	}
}
