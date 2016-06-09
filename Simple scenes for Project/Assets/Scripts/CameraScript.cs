using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public void OnPostRender()
	{
		if(MainScript.octree != null)
			MainScript.octree.OnPostRender();
	}
}
