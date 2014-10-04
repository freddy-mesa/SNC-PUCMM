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

public class TurntableSensorCamera : MonoBehaviour {
	
	public Transform target;
	public float distance;
	public bool useRelativeCameraRotation = true;
	
	// initial camera and sensor value
	private Quaternion initialCameraRotation = Quaternion.identity;
	private bool gotFirstValue = false;
	
	// Use this for initialization
	void Start ()
	{
		// for distance calculation --> its much easier to make adjusments in the editor, just put
		// your camera where you want it to be
		if(target == null) {Debug.LogWarning("Warning! Target for TurntableSensorCamera is null."); return;}
		
		// if distance is set to zero, use current camera position --> easier setup
		if(distance == 0)
			distance = (transform.position - target.position).magnitude;
		
		// if you start the app, you will be viewing in the same direction your unity camera looks right now
		if(useRelativeCameraRotation)
			initialCameraRotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);
		else
			initialCameraRotation = Quaternion.identity;
		// direct call
		// Sensor.Activate(Sensor.Type.RotationVector);
		
		// SensorHelper call with fallback
//		SensorHelper.ActivateRotation();
		SensorHelper.TryForceRotationFallback(RotationFallbackType.OrientationAndAcceleration);
		
		StartCoroutine(Calibration());
	}
	
	IEnumerator Calibration()
	{
		gotFirstValue = false;
		
		while(! SensorHelper.gotFirstValue) {
			SensorHelper.FetchValue();
			yield return null;
		}
		
		SensorHelper.FetchValue();
		
		// wait some frames
		yield return new WaitForSeconds(0.1f);
		
		// Initialize rotation values
		Quaternion initialSensorRotation = SensorHelper.rotation;
		initialCameraRotation *= Quaternion.Euler(0,-initialSensorRotation.eulerAngles.y,0);
		
		// allow updates
		gotFirstValue = true;
	}
	
	// Update is called once per frame
	void LateUpdate()
	{
		// first value gotten from sensor is the offset value for further processing
		if(useRelativeCameraRotation)
		if(!gotFirstValue) return;
	
		// do nothing if there is no target
		if(target == null) return;
		
		transform.rotation = initialCameraRotation * SensorHelper.rotation; // Sensor.rotationQuaternion;
		transform.position = target.position - transform.forward * distance;		
	}
}
