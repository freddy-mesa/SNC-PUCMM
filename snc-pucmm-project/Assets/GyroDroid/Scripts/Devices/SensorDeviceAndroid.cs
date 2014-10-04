#if UNITY_ANDROID

using UnityEngine;

class SensorDeviceAndroid : SensorDeviceUnity
{
    // the static reference to access the gyro values
    private AndroidJavaObject _ao;

    protected override void AwakeDevice()
    {
        if (SystemInfo.supportsGyroscope)
        {
			Input.gyro.enabled = true;
		}
		
//// old and deprecated way
//		// set up connection to java class
//
//        var clsActivity = AndroidJNI.FindClass("com/unity3d/player/UnityPlayer");
//        var fidActivity = AndroidJNI.GetStaticFieldID(clsActivity, "currentActivity", "Landroid/app/Activity;");
//        var objActivity = AndroidJNI.GetStaticObjectField(clsActivity, fidActivity);
//
//        var clsJavaClass = AndroidJNI.FindClass("com/pfc/sensors/SensorClass");
//        var midJavaClass = AndroidJNI.GetMethodID(clsJavaClass, "<init>", "(Landroid/app/Activity;)V");
//
//        var args = new jvalue[1];
//        args[0].l = objActivity;
//		
//        var objJavaClass = AndroidJNI.NewObject(clsJavaClass, midJavaClass, args);
		
		
		//// new way
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject obj = jc.GetStatic<AndroidJavaObject>("currentActivity");
		_ao = new AndroidJavaObject("com.pfc.sensors.SensorClass", obj);

        // get all sensor informations (including whether they area available)
        for (var i = 1; i <= Sensor.Count; i++)
        {
            // fill the sensor information array
            Sensors[i] = new Information(_ao.Call<bool>("isSensorAvailable", i), _ao.Call<float>("getMaximumRange", i), _ao.Call<int>("getMinDelay", i),
                                               _ao.Call<string>("getName", i), _ao.Call<float>("getPower", i), _ao.Call<float>("getResolution", i),
                                               _ao.Call<string>("getVendor", i), _ao.Call<int>("getVersion", i), Description[i]);
        }

        base.AwakeDevice();
		
		// this does not work, no need to try it - just experimental
        //		// setup LocationManager
        //		LocationManager.SetInternalObject(_ao);
        //		
        //		// setup mock providers
        //		LocationManager.AddTestProvider(LocationProvider.GPS);
        //		LocationManager.SetTestProviderLocation(LocationProvider.GPS, new Location(LocationProvider.GPS, 10, 10));
        //		
        //		// request updates
        //		LocationManager.RequestLocationUpdates(LocationProvider.GPS,0,0);
        //		// LocationManager.RequestLocationUpdates(LocationProvider.Network,0,0);
        //		
        //		LocationManager.SetTestProviderLocation(LocationProvider.GPS, new Location(LocationProvider.GPS, 50, 15));
        //		
        //		
        //		// Debug.Log("### " + _ao.Call<string>("GetLastKnownLocation", "gps"));
    }
	
	protected Quaternion CompensateSurfaceRotation;
	protected override void DeviceUpdate()
	{
		CompensateSurfaceRotation = _getSurfaceRotationCompensation();
	}
	
    protected override bool ActivateDeviceSensor(Type sensorID, Sensor.Delay sensorSpeed)
    {
        if (!base.ActivateDeviceSensor(sensorID, sensorSpeed))
		{
			if (_ao.Call<bool>("ActivateSensor", (int) sensorID, (int) sensorSpeed))
			{
				Sensors[(int)sensorID].active = true;
				return true;
			}
		}
		return false;
    }

    protected override bool DeactivateDeviceSensor(Type sensorID)
    {
        if (!base.DeactivateDeviceSensor(sensorID))
		{
			if (_ao.Call<bool>("DeactivateSensor", (int) sensorID))
			{
				Sensors[(int)sensorID].active = false;
				return true;
			}
		}
		return false;
    }

    protected override Vector3 GetDeviceSensor(Type sensorID)
    {
		AndroidJNI.AttachCurrentThread();
		
        Get(sensorID).gotFirstValue = _ao.Call<bool>("gotFirstValue", (int)sensorID);
		
		var ret = Vector3.zero;
		ret.x = _ao.Call<float>("getValueX", (int)sensorID);
        ret.y = _ao.Call<float>("getValueY", (int)sensorID);
        ret.z = _ao.Call<float>("getValueZ", (int)sensorID);
		
		if (sensorID == Type.Orientation && (surfaceRotation == Sensor.SurfaceRotation.Rotation90 || surfaceRotation == Sensor.SurfaceRotation.Rotation270))
		{
			_swapXY(ref ret);
		}
		
		if(sensorID == Type.LinearAcceleration) {
			ret = Quaternion.Inverse(CompensateSurfaceRotation) * ret;
		}
	    return ret;
	}
	
	protected override Vector3 _getDeviceOrientation()
	{
//		if (!((Sensors.Length > 0) && Get(Type.MagneticField).active && Get(Type.Accelerometer).active))
//		{
//			Debug.Log("To use getOrientation, MagneticField and Accelerometer have to be active, because getOrientation internally fuses the two.\n " +
//				      "Magnetic Field: " + Get(Type.MagneticField).active + ", Accelerometer: " + Get(Type.Accelerometer).active);
//		}

	    var k = Vector3.zero;
		AndroidJNI.AttachCurrentThread();
        k.x = _ao.Call<float>("getOrientationX") * Mathf.Rad2Deg;
        k.y = _ao.Call<float>("getOrientationY") * Mathf.Rad2Deg;
        k.z = _ao.Call<float>("getOrientationZ") * Mathf.Rad2Deg;
		
		if (surfaceRotation == Sensor.SurfaceRotation.Rotation90 || surfaceRotation == Sensor.SurfaceRotation.Rotation270)
		{
			// switch y and z
		    _swapYZ(ref k);
		}
		
		// compensate surface rotation
	    CompensateDeviceOrientation(ref k);
		
		return k;
	}

    protected override float GetDeviceAltitude(float pressure, float pressureAtSeaLevel = PressureValue.StandardAthmosphere)
    {
        return _ao.Call<float>("getAltitude", pressure, pressureAtSeaLevel);
    }

    protected override Sensor.SurfaceRotation GetSurfaceRotation()
    {
        return (Sensor.SurfaceRotation)_ao.Call<int>("getWindowRotation");
    }

    protected override Quaternion QuaternionFromDeviceRotationVector(Vector3 v)
    {
        var r = new Quaternion(0, 0, 1, 0)
        {
            x = -_ao.Call<float>("getQuaternionX", v.x, v.y, v.z),
            y = -_ao.Call<float>("getQuaternionY"),
            z = _ao.Call<float>("getQuaternionZ"),
            w = _ao.Call<float>("getQuaternionW")
        };

        // switch axis 
        r = Quaternion.Euler(90, 0, 0) * r * CompensateSurfaceRotation;

        return r;
    }

    protected override void CompensateDeviceOrientation(ref Vector3 k)
    {
        // add or subtract x
        switch (surfaceRotation)
        {
            case Sensor.SurfaceRotation.Rotation90:
                k.x += 90;
                break;
            case Sensor.SurfaceRotation.Rotation270:
                k.x -= 90;
                break;
            case Sensor.SurfaceRotation.Rotation180:
                k.x += 180;
                break;
        }
    }
	
	protected override ScreenOrientation ScreenOrientationDevice {
		get {
			if(
				(surfaceRotation == Sensor.SurfaceRotation.Rotation0 && Screen.orientation == ScreenOrientation.LandscapeLeft) ||
				(surfaceRotation == Sensor.SurfaceRotation.Rotation270 && Screen.orientation == ScreenOrientation.Portrait) ||
				(surfaceRotation == Sensor.SurfaceRotation.Rotation180 && Screen.orientation == ScreenOrientation.LandscapeRight) ||
				(surfaceRotation == Sensor.SurfaceRotation.Rotation90 && Screen.orientation == ScreenOrientation.PortraitUpsideDown)
			)
				return ScreenOrientation.LandscapeLeft;
			else
				return ScreenOrientation.Portrait;
		}
	}
	
	private static void _swapXY(ref Vector3 k)
	{
		var temp = k.y;
		k.y = -k.z;
		k.z = temp;
	}

    private static void _swapYZ(ref Vector3 k)
    {
        var temp = k.y;
        k.y = k.z;
        k.z = temp;
    }
	
	private Quaternion _getSurfaceRotationCompensation()
    {
        switch (surfaceRotation)
        {
            case Sensor.SurfaceRotation.Rotation90:
                return Quaternion.Euler(0, 0, -90);
            case Sensor.SurfaceRotation.Rotation270:
                return Quaternion.Euler(0, 0, 90);
            case Sensor.SurfaceRotation.Rotation180:
                return Quaternion.Euler(0, 0, 180);
            default:
                return Quaternion.Euler(0, 0, 0);
        }
    }
}
#endif