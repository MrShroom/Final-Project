using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Octree {

	private OctreeNode root;
	
	public Octree(Bounds b, int maxDepth){
		root = new OctreeNode (b, 0, maxDepth);
	}

	public void Add(GameObject gameObject){
		root.Add (gameObject);
	}

	public void Remove(GameObject gameObject){
		root.Remove (gameObject);
	}

	public void UpdateObject(GameObject gameObject){
		root.Remove (gameObject);
		root.Add (gameObject);
	}

	public void OnPostRender(){
		root.OnPostRender ();
	}

	public List<GameObject> RetrievePotentialCollisions(GameObject gameObject, ref List<GameObject> potentialCollisions) {
		return root.RetrievePotentialCollisions (gameObject, ref potentialCollisions);
	}
}
