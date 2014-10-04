// #######################################
// ---------------------------------------
// ---------------------------------------
// PFC - prefrontal cortex
// ---------------------------------------
// Full Android Sensor Access for Unity3D
// ---------------------------------------
// Contact:
// 		contact.prefrontalcortex@gmail.com
// ---------------------------------------
// #######################################


using UnityEngine;
using System.Collections;

public class MoveByLinearAcc : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		Sensor.Activate(Sensor.Type.LinearAcceleration);
	}
	
	// Update is called once per frame 
	void FixedUpdate () {
		Vector3 linearAcc = FilterMax(Sensor.linearAcceleration / 20 * 10);
		rigidbody.position = new Vector3( -linearAcc.x, rigidbody.position.y, -linearAcc.y);
	}
	
	// Decay filter - goes instantly up to higher values, but slowly down back to zero
	// (LinearAcceleration sensor returns one peak and goes immediately back down to zero - this filter preserves the peak)
	
	Vector3 holder = Vector3.zero;
	Vector3 max = Vector3.zero;
	Vector3 velocity = Vector3.zero;
	
	Vector3 FilterMax(Vector3 input)
	{
		if(input.magnitude > max.magnitude) max = input; 
		   
		holder = Vector3.SmoothDamp(holder, max, ref velocity, 0.1f);
		if(Vector3.Distance(holder, max) < 0.4f) max = Vector3.zero;
		return holder;
	}
}