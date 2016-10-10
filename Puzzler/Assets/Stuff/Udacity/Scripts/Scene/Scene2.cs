using UnityEngine;
using System.Collections;
using System.IO;

public class Scene2 : MonoBehaviour 
{
	public Material ocean_material;
	public Material cloud_material;

	void Start () 
	{
		RenderSettings.fog 			= true;
		RenderSettings.fogDensity 	= .2f;
		RenderSettings.fogColor 	= Color.white * 0.75f;

		Clouds.gameObject.SetActive(true);	
		Clouds.gameObject.transform.localScale 	= Vector3.one * 192.0f;
		Clouds.gameObject.transform.position 	= new Vector3(0.0f, 32.0f, 0.0f);
		Clouds.gameObject.transform.Rotate(new Vector3(0.0f, 90.0f, 180.0f));
		Clouds.gameObject.GetComponent<MeshRenderer>().material = cloud_material;	

		Ocean.gameObject.SetActive(true);	
		Ocean.gameObject.transform.position = new Vector3(0.0f, -1.7f, 0.0f);
		Ocean.gameObject.transform.localScale 	= Vector3.one * 32.0f;
		Ocean.gameObject.GetComponent<MeshRenderer>().material = ocean_material;	
		Ocean.audio_source.volume 				= 0.125f;
		Ocean.audio_source.Play();
	}

	void MakePNG() 
	{
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);

		Color[] color = new Color[4];
		for(int i = 0; i < 4; i++)
		{
			color[i] = new Color(0.0f, 0.0f, 0.0f, 0.25f);
		}
		tex.SetPixels(color);

		// Read screen contents into the texture
		tex.Apply();

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Object.Destroy(tex);

		// For testing purposes, also write to a file in the project folder
		//File.WriteAllBytes(Application.dataPath + "/../UI_Background.png", bytes);

		Debug.Log("Made PNG");
	}

	void Update () 
	{
		Ocean.SetSoundPositionRelativeToViewer();
	}
}
