using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {

	public static Octree octree;
	public int numberOfObjects = 10;
	public GameObject obj;

	public void Awake(){
		if (MainScript.octree != null)
			throw new UnityException ("Octree already exists.");

		MainScript.octree = new Octree (new Bounds (new Vector3(0,50,0), new Vector3 (100, 100, 100)), 6);
	}

	public void Start(){
		for (int i = 0; i < numberOfObjects; ++i)
			Instantiate (obj);
	}

}
