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

public class SpaceShipControl : MonoBehaviour {

	void Start() {
		SensorHelper.ActivateRotation();
		initialSensorValue = SensorHelper.rotation;
		gotFirstValue = false;
		
		StartCoroutine(Calibration());
	}
	
	IEnumerator Calibration() {
		while(! SensorHelper.gotFirstValue) {
			SensorHelper.FetchValue();
			yield return null;
		}
		
		// wait some frames
		yield return new WaitForSeconds(0.1f);
		
		// set initial rotation
		initialSensorValue = SensorHelper.rotation;
		
		// allow updates
		gotFirstValue = true;
	}
	
	bool gotFirstValue = false;
	Quaternion initialSensorValue;
	
	Quaternion differenceRotation;
	Vector3 differenceEuler;
	
	public float strength = 1;
	public float movementStrength = 10;
		
	void Update() {
		if(!gotFirstValue) return;
		
		// calculate difference between current rotation and initial rotation
		differenceRotation = FromToRotation(initialSensorValue, SensorHelper.rotation);
		
		// differenceEuler is the difference in degrees between the current SensorHelper.rotation and the initial value
		differenceEuler = differenceRotation.eulerAngles;
		
		if(differenceEuler.x > 180) differenceEuler.x -= 360;
		if(differenceEuler.y > 180) differenceEuler.y -= 360;
		if(differenceEuler.z > 180) differenceEuler.z -= 360;
		
		// for an airplane: disable yaw,
		// only use roll and pitch
		differenceEuler.y = 0;
		
		// rotate us
		transform.Rotate(differenceEuler * Time.deltaTime * strength);
		// move forward all the time (no speed control)
		transform.Translate(Vector3.forward * movementStrength * Time.deltaTime, Space.Self);
	}
	
	/// <summary>
	/// Calculates the rotation C needed to rotate from A to B.
	/// </summary>
	public static Quaternion FromToRotation(Quaternion a, Quaternion b) {
		return Quaternion.Inverse(a) * b;
	}

	public bool showGUI = false;
	public void OnGUI() {
		if(!showGUI) return;
		GUI.Label(new Rect(10,10,200,25), "Relative rotation to start in degrees:");
		GUI.Label(new Rect(10,40,200,25), ""+differenceEuler);
	}
}
