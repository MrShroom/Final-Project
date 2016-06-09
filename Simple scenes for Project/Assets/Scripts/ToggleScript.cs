using UnityEngine;
using System.Collections;

public class ToggleScript : MonoBehaviour {

	public GameObject collisionChecker;

	void Update () {
		if (Input.GetKeyDown (KeyCode.Space))
			collisionChecker.GetComponent<CollisionCheck> ().OctreeToggle ();
	}
}
