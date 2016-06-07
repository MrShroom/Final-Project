using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OctreeNode {

	private bool isLeaf;
	private int depth;
	private int maxDepth;
	private List<GameObject> objects;
	private OctreeNode[] children;
	private Bounds bounds;
	//public ReadOnlyCollectionBase<GameObject> contents

	public OctreeNode(Bounds b, int depth, int maxDepth) {
		this.depth = depth;
		this.maxDepth = maxDepth;
		isLeaf = true;
		children = new OctreeNode[8];
		objects = new List<GameObject> ();
		bounds = b;
	}

	public void Add(GameObject gameObject){
		//Bounds objectBounds = gameObject.GetComponent<Bounds> ();
		Bounds objectBounds = gameObject.GetComponent<Collider> ().bounds;
		//the octree should encompass all objects in the game world
		//so disallow objects that exist outside of it
		if(!bounds.Intersects(objectBounds))
			throw new UnityException("Object exists outside of octree");
		//if we're at an empty leaf node or the max depth of the tree, add the object
		if (isLeaf && objects.Count == 0 || depth == maxDepth)
			objects.Add (gameObject);
		//else create the children and push down the objects to the next level
		else {
			//if the object is in the center of this node or if it has the same center as all other items in this node
			//add it here
			if (objectBounds.center.Equals (bounds.center) ||
				objects.Count > 0 && objects.TrueForAll (i => objectBounds.center.Equals (i.GetComponent<Collider> ().bounds.center)))
				objects.Add (gameObject);
			//else build out the new leaf nodes
			//push all the objects in this node down to their new nodes
			//and find the child to place the new object into
			else {
				if (isLeaf) {
					BuildChildren ();
					PushObjectsDown ();
				}
				AddToChildren (gameObject);
			}
		}
	}

	private void BuildChildren(){
		isLeaf = false;
		//bounds.extents is half the length from corner to corner of the node's bounds
		//the center of each of the children is then half the extents of this node
		//away from the center, and the size of each child is simply the extents of this node
		Vector3 e = bounds.extents / 2.0f;

		children [0] = new OctreeNode (new Bounds (bounds.center + new Vector3 (e.x,  e.y,  e.z), bounds.extents), depth + 1, maxDepth);
		children [1] = new OctreeNode (new Bounds (bounds.center + new Vector3 (e.x,  e.y, -e.z), bounds.extents), depth + 1, maxDepth);
		children [2] = new OctreeNode (new Bounds (bounds.center + new Vector3 (e.x, -e.y,  e.z), bounds.extents), depth + 1, maxDepth);
		children [3] = new OctreeNode (new Bounds (bounds.center + new Vector3 (e.x, -e.y, -e.z), bounds.extents), depth + 1, maxDepth);
		children [4] = new OctreeNode (new Bounds (bounds.center + new Vector3 (-e.x, e.y,  e.z), bounds.extents), depth + 1, maxDepth);
		children [5] = new OctreeNode (new Bounds (bounds.center + new Vector3 (-e.x, e.y, -e.z), bounds.extents), depth + 1, maxDepth);
		children [6] = new OctreeNode (new Bounds (bounds.center + new Vector3 (-e.x,-e.y,  e.z), bounds.extents), depth + 1, maxDepth);
		children [7] = new OctreeNode (new Bounds (bounds.center + new Vector3 (-e.x,-e.y, -e.z), bounds.extents), depth + 1, maxDepth);
	}

	private void PushObjectsDown(){
		List<GameObject> toRemove = new List<GameObject>(1);
		foreach (GameObject o in objects) {
			Bounds objectBounds = o.GetComponent<Collider>().bounds;
			if(!objectBounds.center.Equals (bounds.center)) {
				AddToChildren(o);
				toRemove.Add (o);
				//objects.Remove(o);
			}
		}

		foreach (GameObject o in toRemove) {
			objects.Remove (o);
		}
	}

	private void AddToChildren(GameObject gameObject){
		foreach (OctreeNode c in children)
			if (c.bounds.Intersects (gameObject.GetComponent<Collider> ().bounds))
				c.Add (gameObject);
	}

	public void Remove(GameObject gameObject){
		//if we find the object in this node, remove it

		GameObject g = null;

		foreach (GameObject o in objects)
			if (o == gameObject)
				g = o;

		if (g != null)
			objects.Remove (g);

		//else try to find it in the children
		//then once we find the node
		//if this node has children, check for empty children and adjust the tree as needed
		//if all are empty, mark as leaf and create new children
		//if all but one are empty, copy the objects into this node, mark as leaf, create new children
		if (!isLeaf) {
			int emptyChildren = 0;
			bool hasFloater = true;
			List<GameObject> floaters = null;

			foreach (OctreeNode c in children) {
				c.Remove (gameObject);

				if (c.IsEmpty())
					++emptyChildren;
				else if (c.isLeaf){
					if (floaters == null)
						floaters = c.objects;
					else if(!floaters.Equals (c.objects))
						hasFloater = false;
				} else {
					hasFloater = false;
				}

			}
			if (emptyChildren == 8) {
				isLeaf = true;
				children = new OctreeNode[8];
			} else if (hasFloater && objects.Count == 0) {
				foreach(GameObject obj in floaters)
					objects.Add (obj);

				isLeaf = true;
				children = new OctreeNode[8];
			}

		}

	}

	private bool IsEmpty(){
		return isLeaf && objects.Count == 0;
	}

	private static Material _LineMaterial;

	public void OnPostRender() {
		Material outlineMaterial = new Material(Shader.Find("Diffuse"));
		outlineMaterial.SetPass (0);



		GL.Begin (GL.LINES);
		GL.Color (new Color (1, 1, 1, 1));

		DrawNode (this.bounds);

		GL.End ();

		if (!isLeaf)
			foreach (OctreeNode c in children) 
				c.OnPostRender ();
	}

	private void DrawNode(Bounds b) {
		Vector3 c = b.center;
		Vector3 e = b.extents;

		//find vertices of the cube to draw
		Vector3[] v = new Vector3[8];
		v [0] = c + new Vector3 (-e.x, -e.y, -e.z);
		v [1] = c + new Vector3 (-e.x, -e.y,  e.z);
		v [2] = c + new Vector3 ( e.x, -e.y,  e.z);
		v [3] = c + new Vector3 ( e.x, -e.y, -e.z);
		v [4] = c + new Vector3 (-e.x,  e.y, -e.z);
		v [5] = c + new Vector3 (-e.x,  e.y,  e.z);
		v [6] = c + new Vector3 ( e.x,  e.y,  e.z);
		v [7] = c + new Vector3 ( e.x,  e.y, -e.z);

		//each row represents a line to draw for the cube
		//12 lines total
		int[] drawOrder = {0,1,
		                   1,2,
		                   2,3,
		                   3,0,
		                   4,5,
		                   5,6,
		                   6,7,
		                   7,4,
		                   0,4,
		                   1,5,
		                   2,6,
						   3,7};

		foreach(int vertex in drawOrder)
			GL.Vertex (v[vertex]);
	}

}
