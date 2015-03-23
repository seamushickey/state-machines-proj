using UnityEngine;
using System.Collections;

public class Perceptions : MonoBehaviour {

	//Using trignometry we can find the distance between objects, in particular Dot product forumlae.
	Vector3 mydirection;
	public Transform target;
	[Range(0,180f)]
	public float fovrange;
	float FOV =45f;// declare cone area
	
	void Start(){
		
	}
	
	// Update is called once per frame
	void Update () {
		//Note, changing position of targets position according to a circle will give e.g target position x=1 y=1 give the debug position 1, if x=-1 and y=0 debug = -1
		mydirection = transform.TransformDirection(transform.right); //By default faces 0
		Vector3 toOther = target.position - transform.position; //Measures distance between object A and B
		float distance = (target.transform.position - transform.position).magnitude;//creates a float which stores position between A & B
		toOther.Normalize (); //Function to help check the angle
		Debug.DrawLine (transform.position, target.position);
		//Debug.DrawLine (transform.position, mydirection * 4f);
		
		float dot = Vector3.Dot(toOther, mydirection); //Unitys built in Dot product function, long version A.B=A*B+AyBy+AzBz=Dot
		float angle = Mathf.Acos (dot) * Mathf.Rad2Deg;//*mathf.rad2deg converts radians to degrees
		
		Debug.Log (distance);
		Debug.DrawRay (transform.position, mydirection * fovrange);
		//everything related to dot is in relation to using a circle which goes 0 to 360 degrees counterclockwise
		if (angle<FOV && target.position.y==this.transform.position.y && distance<=fovrange+0.5)
		{
			target.renderer.material.color=Color.red; //
			Debug.Log ("Enemy is in front of me");
		}
		else
		{
			target.renderer.material.color=Color.blue;
			Debug.Log("Enemy is behind me");
		}
		Debug.Log(dot);
		//Debug.Log(angle); PRINTS angle position
		
	}
}