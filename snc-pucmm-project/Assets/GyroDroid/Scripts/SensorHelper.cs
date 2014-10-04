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

public static class SensorHelper
{
	
	// Uses the best available rotation sensor
	
	// The first one available on this list will be used:
	// 1) RotationVector
	// 3) Orientation (deprecated. but works on many devices)
	// 2) Accelerometer + MagneticField
	// 3) Accelerometer only - no compass function
	
	// usage:
	// (e.g. on a camera)
	// 
	
	//   void Start()
	//	 {
	//     SensorHelper.ActivateRotation();
	//   }
	
	//   void Update()
	//	 {
	//     transform.rotation = SensorHelper.rotation;
	//   }
	
	
	// ATTENTION! This class is under construction. It may not work on some devices.
	
	#region API
	
	public static void FetchValue()
	{
		if (_rotationHelper != null)
		{
			_rotationHelper();
		}
	}
	
	// this holds the newest rotation value by the best found sensor
	public static Quaternion rotation
	{
		get
        {
		    return _rotationHelper != null ? _rotationHelper() : Quaternion.identity;
		}
	}

    public static bool gotFirstValue { get; private set; }

    // Tries to activate a certain rotation fallback.
	// If ActivateRotation is called, the best (in most cases) fallback is used.
	// There might be cases/devices where other approaches yield better results.
	// 
	// Returns true if fallback is available and activated.
	public static bool TryForceRotationFallback(RotationFallbackType fallbackType)
	{
#pragma warning disable 162
		switch(fallbackType)
		{
		    case RotationFallbackType.RotationQuaternion:
			    if (Sensor.Get(Sensor.Type.RotationVector).available)
			    {
				    Sensor.Activate(Sensor.Type.RotationVector);
				    _rotationHelper = new GetRotationHelper(RotationQuaternion);
				    current = RotationFallbackType.RotationQuaternion;
					
					Debug.Log("First sensor choice is available.");
				    return true;
			    }
		        break;
		    case RotationFallbackType.OrientationAndAcceleration:
			    if (Sensor.Get(Sensor.Type.Orientation).available)
			    {
				    Sensor.Activate(Sensor.Type.Orientation);
				    _rotationHelper = new GetRotationHelper(OrientationAndAcceleration);
					current = RotationFallbackType.OrientationAndAcceleration;
				    
					Debug.Log("Second sensor choice is available.");
			        return true;
			    }
		        break;
		    case RotationFallbackType.MagneticField:
			    if (Sensor.Get(Sensor.Type.MagneticField).available)
			    {
				    Sensor.Activate(Sensor.Type.Accelerometer);
				    Sensor.Activate(Sensor.Type.MagneticField);
				    _rotationHelper = new GetRotationHelper(MagneticField);
					current = RotationFallbackType.MagneticField;
				
				    Debug.Log("Third sensor choice is available.");
			        return true;
			    }
		        break;
		}
		return false;
#pragma warning restore 162
	}
	
	static RotationFallbackType current;

	// Call this to activate the best available rotation sensor
	public static void ActivateRotation()
	{
		// looks which sensors are available
		// and uses the best one for rotation calculation
		
		gotFirstValue = false;
		
		if (Sensor.Get(Sensor.Type.RotationVector).available)
		{
			// on iOS, first choice will always be used (others are made for Android)
			Sensor.Activate(Sensor.Type.RotationVector);
			_rotationHelper = new GetRotationHelper(RotationQuaternion);
			current = RotationFallbackType.RotationQuaternion;
			
			Debug.Log("RotationVector is available.");
		}
		else if (Sensor.Get(Sensor.Type.Orientation).available)
		{
			Sensor.Activate(Sensor.Type.Orientation);
			_rotationHelper = new GetRotationHelper(OrientationAndAcceleration);
			current = RotationFallbackType.OrientationAndAcceleration;
			
			Debug.Log("Orientation/Acceleration is available.");
		}
		else if (Sensor.Get(Sensor.Type.MagneticField).available)
		{
			Sensor.Activate(Sensor.Type.Accelerometer);
			Sensor.Activate(Sensor.Type.MagneticField);
			_rotationHelper = new GetRotationHelper(MagneticField);
			current = RotationFallbackType.MagneticField;
			
			Debug.Log("Accelerometer/MagneticField is available.");
		}
		else
		{
			_rotationHelper = new GetRotationHelper(InputAcceleration);
			Debug.Log("InputAcceleration is available - no compass support.");
		}
	}
	
	public static void DeactivateRotation()
	{
		switch(current)
		{
		case RotationFallbackType.RotationQuaternion:
			Sensor.Deactivate(Sensor.Type.RotationVector);
			break;
		case RotationFallbackType.OrientationAndAcceleration:
			Sensor.Deactivate(Sensor.Type.Orientation);
			break;
		case RotationFallbackType.MagneticField:
			Sensor.Deactivate(Sensor.Type.Accelerometer);
			Sensor.Deactivate(Sensor.Type.MagneticField);
			break;	
		}
	}
	
	#endregion
	
	#region HelperFunctions
	
	private delegate Quaternion GetRotationHelper();
	private static GetRotationHelper _rotationHelper;
	
	private static Quaternion RotationQuaternion()
	{
		if (Sensor.Get(Sensor.Type.RotationVector).gotFirstValue && Sensor.rotationQuaternion != Quaternion.identity)
		{
			gotFirstValue = true;
		}
		return Sensor.rotationQuaternion;
	}

//    private static Vector3 GetPosition()
//    {
//        if (Sensor.Get(Sensor.Type.GPS).gotFirstValue)
//        {
//            gotFirstValue = true;
//        }
//        return Sensor.GetPosition();
//    }

    private static Quaternion GetOrientation()
	{
		if (Sensor.Get(Sensor.Type.MagneticField).gotFirstValue && Sensor.Get(Sensor.Type.Accelerometer).gotFirstValue)
		{
			gotFirstValue = true;
		}
		return Quaternion.Euler(Sensor.GetOrientation());
	}
	
	static Quaternion Q1(float k)
	{
		return new Quaternion(Mathf.Sin(k/2),0,0,Mathf.Cos(k/2));
	}
	
	static Quaternion Q2(float k)
	{
		return new Quaternion(0, Mathf.Sin(k/2),0,Mathf.Cos(k/2));
	}
	
	static Quaternion Q3(float k)
	{
		return new Quaternion(0,0, Mathf.Sin(k/2),Mathf.Cos(k/2));
	}
	
	private static readonly AngleFilter OrientationXFilter = new AngleFilter(8);
	private static Quaternion OrientationAndAcceleration()
	{
		var k = Quaternion.identity;
		
		var accelerometer = AccelerometerOrientation();
		if (Sensor.Get(Sensor.Type.Orientation).gotFirstValue)
		{
			gotFirstValue = true;
		}
		
//		 float yOffset = Sensor.accelerometer.z < 0 ? 180 : 0; 
//		 float y = yFilter.Update(Sensor.GetOrientation().x + yOffset);
		var y = OrientationXFilter.Update(Sensor.orientation.x);
	
		k.eulerAngles = new Vector3(-accelerometer.x, y, accelerometer.z);
		return k;
	}
	
	private static Quaternion Orientation()
	{
		if (Sensor.Get(Sensor.Type.Orientation).gotFirstValue)
		{
			gotFirstValue = true;
		}
		
		var gyro = Sensor.orientation;
		var k1 = Q2(gyro.x/90*Mathf.PI/2);
		var k2 = Q1((gyro.y+90)/90*Mathf.PI/2);
		var k3 = Q3((gyro.z)/90*Mathf.PI/2);
		// current workaround, until better solution is found
		// var k3 = Quaternion.identity;
		
		var rot = k1 * k2 * k3;
		return rot;
	}
	
	private static Quaternion InputAcceleration()
	{
		var accelerometer = AccelerometerOrientation();
		var k = Quaternion.identity;
		k.eulerAngles = new Vector3(-accelerometer.x, accelerometer.y, accelerometer.z);
		return k;
	}

	static readonly AngleFilter YFilter = new AngleFilter(2);
	private static Quaternion MagneticField()
	{
		var accelerometer = AccelerometerOrientation();
		var k = Quaternion.identity;
		
		if (Sensor.Get(Sensor.Type.MagneticField).gotFirstValue && Sensor.Get(Sensor.Type.Accelerometer).gotFirstValue)
		{
			gotFirstValue = true;
		}
		
		var yOffset = Sensor.accelerometer.z < 0 ? 180 : 0; 
		var y = YFilter.Update(Sensor.GetOrientation().x + yOffset);
		
		k.eulerAngles = new Vector3(-accelerometer.x, y, accelerometer.z);
		return k;
	}
		
	static readonly Vector3Filter AccFilter = new Vector3Filter(8);

    static SensorHelper()
    {
        gotFirstValue = false;
    }

    private static Vector3 AccelerometerOrientation()
	{
		var tilt = Input.acceleration;
		
		// seems to have changed in Unity > 4.0
		tilt = Quaternion.Euler(0,0,-90) * tilt;
		
		var xRot = tilt.z * 90;
		const float yRot = 0f;
		var zRot = tilt.y * 90;
		
		AccFilter.Update(new Vector3(xRot, yRot, zRot));
		
		return AccFilter.Value;
	}
	
	#endregion
}
