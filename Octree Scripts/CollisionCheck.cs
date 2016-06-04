using UnityEngine;
using System.Collections;

public class CollisionCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Transform t in transform) {
			GameObject o = t.gameObject;
			if(o.GetComponent<OurCollider>()){
				foreach(Transform r in transform){
					GameObject g = r.gameObject;
					if(g ==  o) continue;
					else if(o.GetComponent<OurCollider>().doesCollide (g))
						print ("Collision");
				}
			}
		}
	}
}
