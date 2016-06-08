using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CollisionCheck : MonoBehaviour {

	public bool UseOctree = false;

	void BruteForceCollisionCheck(){
		foreach (Transform t in transform) {
			GameObject o = t.gameObject;
			if(o.GetComponent<OurCollider>()){
				foreach(Transform r in transform){
					GameObject g = r.gameObject;
					if(g == o) continue;
					else if(o.GetComponent<OurCollider>().doesCollide (g))
						print ("Collision");
				}
			}
		}
	}

	void BruteForceCollisionCheck(List<GameObject> potentialCollisions) {
		foreach (GameObject g in potentialCollisions) {
			if(g.GetComponent<OurCollider>()){
				foreach(GameObject o in potentialCollisions){
					if(g == o) continue;
					else if(o.GetComponent<OurCollider>().doesCollide (g))
						print ("Collision");
				}
			}
		}
	
	}

	void OctreeCollisionCheck(){
		List<GameObject> potentialCollisions = new List<GameObject> ();
		foreach (Transform t in transform) {
			potentialCollisions.Clear ();
			potentialCollisions = MainScript.octree.RetrievePotentialCollisions(t.gameObject);

			BruteForceCollisionCheck (potentialCollisions);
		}
	}

	// Update is called once per frame
	void Update () {
		if (!UseOctree)
			BruteForceCollisionCheck ();
		else 
			OctreeCollisionCheck();
	}
}
