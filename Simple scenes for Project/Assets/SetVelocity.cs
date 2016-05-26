using UnityEngine;
using System.Collections;

public class SetVelocity : MonoBehaviour {
	public GameObject newSphere;
	public GameObject floor;

	// Use this for initialization
	void Start () {
		Vector3 randomPos = new Vector3 ();
		randomPos.x = Random.Range(-45,45) * 1f;
		randomPos.z = Random.Range(-45,45) * 1f;
		randomPos.y = 2.5f;
		transform.position = randomPos;

		Vector3 centerOfMass = new Vector3 ();
		object[] obj = GameObject.FindSceneObjectsOfType(typeof (GameObject));
		foreach (object o in obj)
		{
			GameObject g = (GameObject) o;
			if(g.CompareTag("Sphere"))
				centerOfMass += g.transform.position;
		}
		centerOfMass /= obj.Length;
		if (centerOfMass.x > 25 || centerOfMass.x < -25)
			centerOfMass = new Vector3 ();
		if (centerOfMass.z > 25 || centerOfMass.z < -25)
			centerOfMass = new Vector3 ();
		Vector3 vel = (centerOfMass - transform.position).normalized * 50f;
		vel.y = 0f;
		GetComponent<Rigidbody>().velocity = vel;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (transform.position.y <= -1f ) {
			Start();
		}

	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.CompareTag("Sphere") && GameObject.FindSceneObjectsOfType(typeof (GameObject)).Length < 50)
			Instantiate(newSphere);
		
	}
}
