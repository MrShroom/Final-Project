using UnityEngine;
using System.Collections;

public class InstantiateObjects : MonoBehaviour {

	public int numObjects;
	public GameObject obj;

	void Start () {
	
		for (int i = 0; i < numObjects; ++i) {
			GameObject o = Instantiate (obj) as GameObject;
			o.transform.parent = this.transform;
			o.transform.position = new Vector3(Random.Range (-75,75), Random.Range(0,150), Random.Range (-75,75));
		}

	}
}
