using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpringJoint))]
[RequireComponent(typeof(MeshRenderer))]
public class PhysicsInteraction : MonoBehaviour
{
	public float			force					= 32.0f;
	public bool				attached				= true;	

	public Color			hilight_color			= Color.white;
	
	private	Color			_origional_color		= Color.black;
	private int				_interactions			= 0;
	private float			_hilight				= 0.0f;
	private float			_hilight_fade_speed		= 0.05f;

	private Rigidbody		_rigid_body;
	private Material		_material;

	void Start()
	{
		_rigid_body 											= gameObject.GetComponent<Rigidbody>();
		_material												= gameObject.GetComponent<MeshRenderer>().material;
		
		_origional_color										= _material.color;
		
		gameObject.GetComponent<SpringJoint>().breakForce		= float.PositiveInfinity;
		gameObject.GetComponent<SpringJoint>().breakTorque		= float.PositiveInfinity;
		
		if(!attached)
		{
			gameObject.GetComponent<SpringJoint>().breakForce 	= 0;
		}
	}


	void LateUpdate()
	{
		Ray ray			= new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit	= new RaycastHit();
		Physics.Raycast(ray, out hit);
		
		if(hit.rigidbody == _rigid_body)
		{
			_hilight = 0.5f;

			if(GvrViewer.Instance.Triggered)
			{
				_rigid_body.AddForceAtPosition(View.hit.normal * -force, View.hit.point);
				
				_hilight = 1.0f;

				_interactions++;

				if(_interactions > 3 && attached)
				{
					_rigid_body.freezeRotation = false;
					
					Detach();
				}
			}
		}	

		_material.color	= Color.Lerp(_origional_color, hilight_color, _hilight);
		_hilight 		-= _hilight_fade_speed;
	}


	private void Detach()
	{
		attached 											= false;
		gameObject.GetComponent<SpringJoint>().breakForce	= 0;
	}
}
