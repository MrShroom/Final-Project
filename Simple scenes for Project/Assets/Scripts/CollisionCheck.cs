using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CollisionCheck : MonoBehaviour {

	public bool UseOctree = false;
	[HideInInspector]
	static public int ChecksPerFrame;

	public Text counter;
	
	public void UpdateCounter(int count){
		counter.text = count.ToString ();
	}


	void BruteForceCollisionCheck(){
		foreach (Transform t in transform) {
			GameObject o = t.gameObject;
			if(o.GetComponent<OurCollider>()){
				foreach(Transform r in transform){
					GameObject g = r.gameObject;
					if(g == o) continue;
					else if(o.GetComponent<OurCollider>().doesCollide (g)){
						//print ("Collision");
					}
					++ChecksPerFrame;
				}
			}
		}
	}

	void BruteForceCollisionCheck(ref List<GameObject> potentialCollisions) {
		foreach (GameObject o in potentialCollisions) {
			if(o.GetComponent<OurCollider>()){
				foreach(GameObject g in potentialCollisions){
					if(g == o) continue;
					else if(o.GetComponent<OurCollider>().doesCollide (g)){
						//print ("Collision");
					}
					++ChecksPerFrame;
				}
			}
		}
	
	}

	void OctreeCollisionCheck(){
		List<GameObject> potentialCollisions = new List<GameObject>(1);
		foreach (Transform t in transform) {
			potentialCollisions.Clear ();
			MainScript.octree.RetrievePotentialCollisions(t.gameObject, ref potentialCollisions);
			BruteForceCollisionCheck (ref potentialCollisions);
		}
	}

	public void Start(){
		ChecksPerFrame = 0;
	}

	// Update is called once per frame
	void FixedUpdate () {
		ChecksPerFrame = 0;
		if (!UseOctree)
			BruteForceCollisionCheck ();
		else 
			OctreeCollisionCheck();
		UpdateCounter (ChecksPerFrame);
	}
}
