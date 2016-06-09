using UnityEngine;
using System.Collections;

public class ObjectScript : MonoBehaviour {

	private bool destroyed = false;
	private Vector3 velocity;
	public float speed;

	public void Start() {
		MainScript.octree.Add (this.gameObject);
		StartCoroutine (updateObject ());

		velocity = new Vector3 (Random.Range (0, 5), Random.Range (0, 5), Random.Range (0, 5)) * speed;

	}

	public void Update() {
		transform.position += velocity * Time.deltaTime;

		if (transform.position.x > MainScript.octree.bounds.max.x)
			velocity.x *= -1;
		if (transform.position.y > MainScript.octree.bounds.max.y)
			velocity.y *= -1;
		if (transform.position.z > MainScript.octree.bounds.max.z)
			velocity.z *= -1;

		if (transform.position.x < MainScript.octree.bounds.min.x)
			velocity.x *= -1;
		if (transform.position.y < MainScript.octree.bounds.min.y)
			velocity.y *= -1;
		if (transform.position.z < MainScript.octree.bounds.min.z)
			velocity.z *= -1;

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
