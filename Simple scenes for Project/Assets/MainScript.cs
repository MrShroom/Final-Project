using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {

	public static Octree octree;

	public void Awake(){
		if (MainScript.octree != null)
			throw new UnityException ("Octree already exists.");

		MainScript.octree = new Octree (new Bounds (new Vector3(0,50,0), new Vector3 (100, 100, 100)), 6);
	}



}
