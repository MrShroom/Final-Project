using UnityEngine;
using System.Collections;

public class OurCollider : MonoBehaviour {

	bool SphereCollision(GameObject o){
		if (o.tag == "Sphere") {
			Vector3 distanceBetweenCenters = transform.position - o.transform.position;
			float sumOfRadii = GetComponent<SphereCollider>().radius * transform.lossyScale.x + o.GetComponent<SphereCollider>().radius * o.transform.lossyScale.x;
			if(distanceBetweenCenters.magnitude <= sumOfRadii)
				return true;
		}
		if (o.tag == "Plane") {
			Plane p = new Plane(new Vector3(0,1,0), new Vector3(0,0,0));
			float distance = p.GetDistanceToPoint(transform.position);
			if(distance <= GetComponent<SphereCollider>().radius * transform.lossyScale.x)
				return true;
		}
		if (o.tag == "Box") {
			return false;
		}

		return false;
	}

	bool BoxCollision(GameObject o){
		if (o.tag == "Sphere") {
			return false;
		}
		if (o.tag == "Plane") {
			return false;
		}
		if (o.tag == "Box") {
			Plane p = new Plane(new Vector3(0,1,0), new Vector3(0,0,0));
			Vector3 cornerA = o.transform.position;
			Vector3 cornerB = o.transform.position;
			cornerA += o.GetComponent<BoxCollider>().size / 2;
			cornerB -= o.GetComponent<BoxCollider>().size / 2;
			
			if( Vector3.Dot(p.normal,cornerA) > 0 && Vector3.Dot (p.normal,cornerB) > 0 )
				return false;
			else if( Vector3.Dot(p.normal,cornerA) < 0 && Vector3.Dot (p.normal,cornerB) < 0 )
				return false;
			else
				return true;

		}
		return false;	
	}

	bool PlaneCollision(GameObject o){
		Plane p = new Plane(new Vector3(0,1,0), new Vector3(0,0,0));
		if (o.tag == "Sphere") {
			float distance = p.GetDistanceToPoint(o.transform.position);
			if(distance <= o.GetComponent<SphereCollider>().radius * o.transform.lossyScale.x)
				return true;
		}
		if (o.tag == "Plane") {
			return false;
		}
		if (o.tag == "Box") {
			Vector3 cornerA = o.transform.position;
			Vector3 cornerB = o.transform.position;
			cornerA += o.GetComponent<BoxCollider>().size / 2;
			cornerB -= o.GetComponent<BoxCollider>().size / 2;

			if( Vector3.Dot(p.normal,cornerA) > 0 && Vector3.Dot (p.normal,cornerB) > 0 )
				return false;
			else if( Vector3.Dot(p.normal,cornerA) < 0 && Vector3.Dot (p.normal,cornerB) < 0 )
				return false;
			else
				return true;

		}

		return false;
	}

	public bool doesCollide(GameObject o){
		if (tag == "Sphere")
			return SphereCollision (o);
		if (tag == "Box")
			return BoxCollision (o);
		if (tag == "Plane")
			return PlaneCollision (o);
		return false;
	}

}
