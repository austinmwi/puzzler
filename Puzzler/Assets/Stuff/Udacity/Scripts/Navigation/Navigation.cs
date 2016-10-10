using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour 
{
	//this class requires a reference to the view that is raycasting
	public View view;

	//speed at which we move between waypoints
	public float speed	= 0.05f;

	//a list of all waypoints
	private Waypoint[]	_waypoint;

	//the current waypoint
	private Waypoint	_current;

	void Start () 
	{
		//first, find all the waypoints that have been placed in the scene
		_waypoint						= FindAll();

		//now, search them for the nearest one
		_current						= Nearest();

		//set it to occupied
		_current.Occupy();
		
		//update all the waypoints to reflect their active / inactive status
		UpdateAll();

		//warp the viewpoint to the currently occupied position 
		gameObject.transform.position	= _current.position;
	}


	void LateUpdate () 
	{
		//create a ray and hit 
		Ray ray			= new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit	= new RaycastHit();

		//this physics raycast has an "out" parameter - that means you must hand it a reference to an already existing hit
		//(because that's how it was written - likely to do with making the raycast fast)
		Physics.Raycast(ray, out hit);

		//check to see if the raycast has hit any colliders
		if(hit.rigidbody != null)
		{
			//if so, check all the waypoints to see if one of them has been hit
 			for(int i = 0; i < _waypoint.Length; i++)
			{
				if(hit.rigidbody == _waypoint[i].rigid_body)
				{
					//if a waypoint has been hit, it's an active waypoint, and the person is pressing the trigger, activate it
					if(GvrViewer.Instance.Triggered && _waypoint[i].active)
					{
						//exit the current waypoint
						_current.Exit();
		
						//tell the new waypoint it's been triggered
						_waypoint[i].Trigger();		

						//set the current waypoint to be the new waypoint
						_current	= _waypoint[i];
						
						//update all the waypoints to reflect their new active/inactive status
						UpdateAll();
					}
					else
					{
						//otherwise, if it has not been triggered just hilight it
						_waypoint[i].Hilight();
					}
				}
			}
		}	

		//if the current waypoint isn't occupied (ie, it has been changed) and we aren't already on it, move towards it
		if(_current.occupied == false && gameObject.transform.position != _current.position)
		{
			MoveTo(_current);
		}
	}


	//finds all the waypoint prefabs in the scene (tagged as "Taypoint") and puts them in an array
 	public Waypoint[] FindAll() 
	{
		GameObject[] waypoint_object = GameObject.FindGameObjectsWithTag("Waypoint");
		
		Waypoint[] waypoint 		= new Waypoint[waypoint_object.Length];

		for(int i = 0; i < waypoint_object.Length; i++)
		{
			waypoint[i] = waypoint_object[i].GetComponent<Waypoint>();
		}
		
		return waypoint;
	}


	//moves the player to the current waypoint - if the player is within .05 it snaps them directly on it
	public void MoveTo(Waypoint waypoint)
	{
		float distance = Vector3.Distance(gameObject.transform.position, waypoint.position);

		if(distance > 0.05f)
		{
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, waypoint.position, speed);
		}
		else
		{
			gameObject.transform.position = waypoint.position;
			
			_current.Occupy();

			UpdateAll();
		}
	}


	//this searches all the waypoints to find the one closest to the view
	public Waypoint Nearest () 
	{
		int nearest_waypoint_index	= 0;
		float distance_to_nearest	= float.PositiveInfinity;

		for(int i = 0; i < _waypoint.Length; i++)
		{
			float distance_to_waypoint = Vector3.Distance(gameObject.transform.position, _waypoint[i].position);
			
			if(distance_to_waypoint < distance_to_nearest)
			{
				nearest_waypoint_index	= i;
				distance_to_nearest 	= distance_to_waypoint;
			}
		}

		return _waypoint[nearest_waypoint_index];
	}


	//this tells all the waypoint prefabs to update their status
	public void UpdateAll()
	{
		for(int i = 0; i < _waypoint.Length; i++)
		{
			_waypoint[i].UpdateActivation();
		}		
	}
}
