using UnityEngine;
using System.Collections;

public class ObjectScript : MonoBehaviour {
	private static Octree octree;
	private bool destroyed = false;

	public void Start() {
		MainScript.octree.Add (this.gameObject);
		StartCoroutine (updateObject ());
	}

	private IEnumerator updateObject() {
		while (!destroyed) {
			MainScript.octree.UpdateObject(this.gameObject);
			yield return new WaitForSeconds(0.05f);
		}
	}

	public void OnDestroy(){
		destroyed = true;
		MainScript.octree.Remove (this.gameObject);
	}
}
