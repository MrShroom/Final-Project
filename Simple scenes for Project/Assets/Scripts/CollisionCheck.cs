using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CollisionCheck : MonoBehaviour {

	private float runningAverage;
	private int averageUpdateTimer;
	private int ChecksPerFrame;

	public bool UseOctree = true;
	public Text counter, averageCounter;

	public void UpdateCounter(int count){
		counter.text = count.ToString ();
	}

	public void UpdateAverageCounter(){
		averageCounter.text = runningAverage.ToString ();
	}

	public void OctreeToggle(){
		UseOctree = !UseOctree;
	}

	void BruteForceCollisionCheck(){
		foreach (Transform t in transform) {
			GameObject o = t.gameObject;
			if(o.GetComponent<OurCollider>()){
				foreach(Transform r in transform){
					GameObject g = r.gameObject;
					if(g == o) continue;
					else if(o.GetComponent<OurCollider>().doesCollide (g)){
						if(o.GetComponent<Renderer>())
						o.GetComponent<Renderer> ().material.SetColor ("_Color", new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f)));
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
						if(o.GetComponent<Renderer>())
							o.GetComponent<Renderer>().material.SetColor ("_Color", new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f)));
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

	void CalculateAverage(float checksThisFrame){
		runningAverage += checksThisFrame;
		runningAverage /= 2.0f;

	}

	public void Start(){
		ChecksPerFrame = 0;
		averageUpdateTimer = 0;
		UseOctree = true;
	}

	// Update is called once per frame
	void Update () {
		averageUpdateTimer++;
		ChecksPerFrame = 0;
		if (!UseOctree)
			BruteForceCollisionCheck ();
		else 
			OctreeCollisionCheck ();
		UpdateCounter (ChecksPerFrame);
		CalculateAverage (ChecksPerFrame);
		if (averageUpdateTimer >= 120) {
			UpdateAverageCounter ();
			averageUpdateTimer = 0;
			runningAverage = 0f;
		}

		if (Input.GetKeyDown (KeyCode.Space))
			OctreeToggle ();

	}
}
