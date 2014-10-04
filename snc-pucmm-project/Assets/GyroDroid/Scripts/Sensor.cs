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

// TODO Minimales Package erzeugen und mit Verteilen (ohne die Demoszenen, sonst muss man sich das immer selbst raussuchen)
// TODO Demo-Szene für Magnetfeld
// TODO Demo-Szene für konstanten Horizont

// USAGE:

// first, enable the sensor you want to use (in any Start() function)
//   void Start()
//   {
//      Sensor.Activate(Sensor.SensorType.Light);
//   }

// second, grab the sensor value by accessing the shortcut:
//   void Update()
//   {
//      float lightValue = Sensor.light;
//   }

// See the individual demo scripts for more information.


public abstract partial class Sensor : MonoBehaviour
{
	
	public static int Count = 13;
	
	#region Shorthands
	
	//public static float AngleFixX = 90;
	//public static float AngleFixY = 0;
	//public static float AngleFixZ = 240;
	
	// Properties written in lowerCase, because Unity does that everywhere
	// raw sensors - be sure to first call Activate(Sensor.SensorType.<yourSensor>);!
	public static Vector3 accelerometer	{ get {	return Value(Type.Accelerometer);	} } // [meters/second^2]
	public static Vector3 magneticField { get {	return Value(Type.MagneticField);	} }	// [uT] (micro-Tesla)
	public static Vector3 orientation {	get	{ return Value(Type.Orientation);	} }	// [degrees]
	public static Vector3 gyroscope { get { return Value(Type.Gyroscope);	} } // [radians/second]
	public static new float light {	get	{ return Value(Type.Light).x;	} }	// [lux]
	public static float pressure { get { return Value(Type.Pressure).x; } } 
	public static float temperature { get {	return Value(Type.Temperature).x;	} } // [degrees Celsius]
	public static float proximity {	get	{ return Value(Type.Proximity).x;	} } // [cm]
	public static Vector3 gravity {	get	{ return Value(Type.Gravity);	} }	// [meters/second^2]
	public static Vector3 rotationVector { get { return Value(Type.RotationVector); } } // [last three values of a quaternion]
    public static Vector3 linearAcceleration {	get	{ return Value(Type.LinearAcceleration); } } // [meters/second^2]
	public static float ambientTemperature { get { return Value(Type.AmbientTemperature).x; } } // [degrees Celsius]
	public static float relativeHumidity { get { return Value(Type.RelativeHumidity).x; } } // [percent]
	
	// helper functions
	public static Quaternion rotationQuaternion	{ get { return Singleton.QuaternionFromRotationVector(rotationVector);	} } 
	
	// get current altitude (usually used with pressure sensor - simply use GetAltitude() to get height from last pressure value)
	public static float GetAltitude()
	{
		return Singleton.GetAltitude(pressure);
	}

	
	// getOrientation is an additional method provided by Android. It is recommended instead of SensorType.Orientation, because
	// it is fused together from SensorType.magneticField and SensorType.Accelerometer, so these two have to be enabled!

	public static Vector3 GetOrientation()
	{
		return _deviceOrientation;
	}

    public static Sensor.SurfaceRotation surfaceRotation { get; private set; }

	public static ScreenOrientation NativeDeviceOrientation {
		get { 
			return Singleton.ScreenOrientationDevice;
		}
	}	
	

    // Device sensors obey the following relation:
	// accelerometer = gravity + linearAcceleration
	
	#endregion
	
	#region ConstantValues

    protected static readonly string[] Description = new [] { "",
														      "Accelerometer",
														      "Magnetic Field",
														      "Orientation",
														      "Gyroscope",
														      "Light",
														      "Pressure",
														      "Temperature",
														      "Proximity",
														      "Gravity",
														      "Linear Acceleration",
													          "Rotation Vector",
														 	  "Relative Humidity",
		 													  "Ambient Temperature"
												            };
	#endregion
	
	// check whether everything works fine...
	private static bool CheckID(Type sensorID)
	{
		return Singleton != null && ((int) sensorID > -1) && ((int) sensorID < Sensors.Length) && Sensors[(int) sensorID] != null;		
	}
	
	// can also be retrieved by Get(Sensor.SensorType.<yourType>).values (this is just for internal use)
	private static Vector3 Value(Type sensorID)
	{
	    return !CheckID(sensorID) ? Vector3.zero : Sensors[(int)sensorID].values;
	}

    // _________________________API
	
	#region API
	
	public static Information Get(Type sensorID)
	{
	    return !CheckID(sensorID) ? null : Sensors[(int)sensorID];
	}

    // Activate a sensor
	// e.g.
	// Sensor.Activate(Sensor.SensorType.Light);
	// or
	// Sensor.Activate(Sensor.SensorType.Light, Sensor.SensorDelay.UI);
	public static bool Activate(Type sensorID, Sensor.Delay sensorSpeed = Sensor.Delay.Game)
	{
		if (!CheckID(sensorID))
		{
			return false;
		}
		return Singleton.ActivateDeviceSensor(sensorID, sensorSpeed);
	}
	
	// Deactivate a sensor, always deactivate sensors not longer needed!
	public static bool Deactivate(Type sensorID)
	{
        if (!CheckID(sensorID))
        {
            return false;
        }
	    return Singleton.DeactivateDeviceSensor(sensorID);
	}
	
	#endregion
	
	// _________________ EndAPI
	
	// singleton value - no need to use from outside
	private static Sensor _singleton;
	protected static Sensor Singleton
	{
		get
		{
			if (_singleton == null)
			{
#if UNITY_IPHONE && !UNITY_EDITOR
                _singleton = (Sensor)FindObjectOfType(typeof(SensorDeviceIPhone));
#elif UNITY_ANDROID && !UNITY_EDITOR
                _singleton = (Sensor)FindObjectOfType(typeof(SensorDeviceAndroid));
#else
				_singleton = (Sensor)FindObjectOfType(typeof(SensorEditorUnity));
#endif
				if (_singleton == null)
				{
					var go = new GameObject("Singleton::SensorHolder");
#if UNITY_IPHONE && !UNITY_EDITOR
                    _singleton = go.AddComponent<SensorDeviceIPhone>();
#elif UNITY_ANDROID && !UNITY_EDITOR
                    _singleton = go.AddComponent<SensorDeviceAndroid>();
#else
                    _singleton = go.AddComponent<SensorEditorUnity>();
#endif
                }
			}
			return _singleton;
		}
		set
		{
			_singleton = value; 
		}
	}
	
	// internal structure for managing all the sensors and information
	protected static readonly Information[] Sensors = new Information[Sensor.Count + 1];
		
	// getOrientation is an additional method of the android sensor class which should be used instead of SensorType.Orientation, because it fuses multiple sensors together.
	private static Vector3 _deviceOrientation;


    #region UnityMethods
	
	// fetch the active sensors every frame
	void Update()
	{
		DeviceUpdate();
		
	    // update all possible sensors
		for (var i = 1; i <= Sensor.Count; i++)
		{
			if (Sensors[i] != null && Sensors[i].active)
			{
				Sensors[i].SetValue(GetDeviceSensor((Type)i));
			}
		}
		
		// fetch additional values
		surfaceRotation = GetSurfaceRotation();
		// set the compensateSurfaceRotation
	    //CompensateSurfaceRotation = _getSurfaceRotationCompensation();
		
		// only call this if the appropriate sensors are activated
//		if(Get(Type.MagneticField).active && Get(Type.Accelerometer).active)
			_deviceOrientation = _getDeviceOrientation();
	}

    void OnApplicationPause(bool pause)
	{
		// deactivate sensors
		for (var i = 1; i <= Sensor.Count; i++)
		{
			var t = (Type) i;
			
			if (!pause && Get(t) != null && Get(t).suspended)
			{
				Activate((Type)i);
			}

			if (pause)
			{
				if (Get(t) != null )
				{
					Get(t).suspended = Get(t).active;
				}
			}
			else
			{
				if (Get(t) != null)
				{
					Get(t).suspended = false;
				}
			}
		}
	}
	
	void OnDisable()
	{
		for (var i = 1; i <= Sensor.Count; i++)
		{
			var t = (Type) i;
			Deactivate(t);
		}

	    DisableDevice();

		_singleton = null;
	}

    void Awake()
	{
		Singleton = this;

	    AwakeDevice();
		
		// get Device orientation
		surfaceRotation = GetSurfaceRotation();
	    // GPSPosition = GetPosition();
	}

    #endregion
	
	#region Wrappers
	
	protected void SetSensorOn(Type sensorID)
	{
		Sensors[(int)sensorID].active = true;
	}
	
	protected void SetSensorOff(Type sensorID)
	{
		Sensors[(int) sensorID].active = false;
	}	
    
	public float GetAltitude(float pressure, float pressureAtSeaLevel = PressureValue.StandardAthmosphere)
	{
		return GetDeviceAltitude(pressure, pressureAtSeaLevel);
	}
	
	public Quaternion QuaternionFromRotationVector(Vector3 v)
	{
	    var r = QuaternionFromDeviceRotationVector(v);
		
		return float.IsNaN(r.x) ? Quaternion.identity : r;
	}
	
	#endregion
	
	#region internals device dependant
	
	protected virtual void DeviceUpdate() {}
	
	protected abstract void DisableDevice();
	protected abstract void AwakeDevice();
	
	protected abstract bool ActivateDeviceSensor(Type sensorID, Sensor.Delay sensorSpeed);
	protected abstract bool DeactivateDeviceSensor(Type sensorID);

	protected abstract Quaternion QuaternionFromDeviceRotationVector(Vector3 v);
	
	protected abstract Vector3 GetDeviceSensor(Type sensorID);
	
	// to sort
	
	//protected abstract Quaternion _getSurfaceRotationCompensation();
    protected abstract Sensor.SurfaceRotation GetSurfaceRotation();
	
	protected abstract ScreenOrientation ScreenOrientationDevice {
		get;
	}
	
	protected abstract float GetDeviceAltitude(float pressure, float pressureAtSeaLevel = PressureValue.StandardAthmosphere);
	protected abstract Vector3 _getDeviceOrientation();
	
    // protected abstract Vector3 GetDeviceOrientation();
    protected abstract void CompensateDeviceOrientation(ref Vector3 k);
	
    #endregion
}