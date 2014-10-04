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

public class SwitchRotationMode : MonoBehaviour {
	
	void Start() {
		useGUILayout = false;
	}
	
	void OnGUI () {
		GUI.Label(new Rect(10, 115, 180, 20), "Different modes:");
		
		if(GUI.Button(new Rect(10,140,180,20), "Rotation Vector"))
			SensorHelper.TryForceRotationFallback(RotationFallbackType.RotationQuaternion);
		
		if(GUI.Button(new Rect(10,165,180,20), "Orientation/Acceleration"))
			SensorHelper.TryForceRotationFallback(RotationFallbackType.OrientationAndAcceleration);
		
		if(GUI.Button(new Rect(10,190,180,20), "Magnetic Field"))
			SensorHelper.TryForceRotationFallback(RotationFallbackType.MagneticField);
	}
}
