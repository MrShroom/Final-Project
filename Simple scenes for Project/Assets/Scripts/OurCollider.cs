using UnityEngine;
using System.Collections;

public class OurCollider : MonoBehaviour {
	
	bool SphereCollision(GameObject o){
		if (o.tag == "Sphere") {
			Vector3 distanceBetweenCenters = transform.position - o.transform.position;
			float sumOfRadii = GetComponent<SphereCollider>().radius * transform.lossyScale.x + o.GetComponent<SphereCollider>().radius * transform.lossyScale.x;
			if(distanceBetweenCenters.magnitude <= sumOfRadii)
				return true;
		}
		if (o.tag == "Plane") {
			Plane p = new Plane(new Vector3(0,1,0), new Vector3(0,0,0));
			float distance = p.GetDistanceToPoint(transform.position);
			if(distance <= GetComponent<SphereCollider>().radius)
				return true;
		}
		if (o.tag == "Box") {

			Bounds b = o.GetComponent<BoxCollider>().bounds;


			float xDist = Mathf.Abs(transform.position.x - o.transform.position.x);
			float yDist = Mathf.Abs(transform.position.y - o.transform.position.y);
			float zDist = Mathf.Abs(transform.position.z - o.transform.position.z);

			if(xDist >= (b.size.x + GetComponent<SphereCollider>().radius)) return false;
			if(yDist >= (b.size.y + GetComponent<SphereCollider>().radius)) return false;
			if(zDist >= (b.size.z + GetComponent<SphereCollider>().radius)) return false;

			if(xDist < b.size.x) return true;
			if(yDist < b.size.y) return true;
			if(zDist < b.size.z) return true;

			float squaredDistance = Mathf.Pow ((xDist - b.size.x), 2) + 
									Mathf.Pow ((yDist - b.size.y), 2) + 
									Mathf.Pow ((zDist - b.size.z), 2); 

			return squaredDistance < Mathf.Pow (GetComponent<SphereCollider>().radius, 2);
		}
		
		return false;
	}
	
	bool BoxCollision(GameObject o){
		if (o.tag == "Sphere") {
			return o.GetComponent<OurCollider>().SphereCollision(this.gameObject);
		}
		if (o.tag == "Plane") {
			return o.GetComponent<OurCollider>().PlaneCollision(this.gameObject);
		}
		if (o.tag == "Box") {

			Vector3 thisMin = GetComponent<BoxCollider>().bounds.min;
			Vector3 oMin = o.GetComponent<BoxCollider>().bounds.min;

			Vector3 thisMax = GetComponent<BoxCollider>().bounds.max;
			Vector3 oMax = o.GetComponent<BoxCollider>().bounds.max;

			return (thisMax.x >= oMin.x && thisMin.x <= oMax.x)
					&& (thisMax.y >= oMin.y && thisMin.y <= oMax.y)
					&& (thisMax.z >= oMin.z && thisMin.z <= oMax.z);
		}
		return false;	
	}
	
	bool PlaneCollision(GameObject o){
		Plane p = new Plane(new Vector3(0,1,0), new Vector3(0,0,0));
		if (o.tag == "Sphere") {
			float distance = p.GetDistanceToPoint(o.transform.position);
			if(distance <= o.GetComponent<SphereCollider>().radius)
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
