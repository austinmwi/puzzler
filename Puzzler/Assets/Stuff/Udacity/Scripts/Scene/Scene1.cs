using UnityEngine;
using System.Collections;

public class Scene1 : MonoBehaviour 
{
	public Material ocean_material;

	void Start () 
	{
		Ocean.gameObject.SetActive(true);	
		Ocean.gameObject.transform.localPosition = new Vector3(0.0f, -3.0f, 0.0f);
		Ocean.gameObject.GetComponent<MeshRenderer>().material = ocean_material;	
	}
}
