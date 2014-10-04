using UnityEngine;
using System.Collections;

public class MagneticFieldCamera : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		// GetOrientation internally needs access to MagneticField and Accelerometer
		// MagneticField alone is not sufficient - it returns a vector pointing towards the magnetic south pole
		// (right trough the crust of the earth - the magnetic south pole is near the geographic north pole), and you probably do not want this,
		// although you can calculate other things with it if you need it.
		// MagneticField really does what it is saying - return the magnetic field values, which do not correspond to angles.
		Sensor.Activate(Sensor.Type.MagneticField);
		Sensor.Activate(Sensor.Type.Accelerometer);
	}
	
	// filtering for the rotation value
	FloatFilter magneticFilter = new AngleFilter(10);
	
	// Update is called once per frame
	void Update () {
		// camera is above scene, rotate camera around
		transform.rotation = Quaternion.Euler(90,magneticFilter.Update(Sensor.GetOrientation().x),0);
	}
}