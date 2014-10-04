#if (!UNITY_ANDROID && !UNITY_IPHONE) || UNITY_EDITOR

using UnityEngine;

class SensorEditorUnity : Sensor
{
	// for debugging the values in the editor
	public bool accelerometerAvailable = true;
	public bool magneticFieldAvailable = true;
	public bool orientationAvailable = true;
	public bool gyroscopeAvailable = true; 			
	public bool lightAvailable = true; 				
	public bool pressureAvailable = true; 			
	public bool temperatureAvailable = true; 		
	public bool proximityAvailable = true; 		
	public bool gravityAvailable = true; 			
	public bool linearAccelerationAvailable = true;
	public bool rotationVectorAvailable = true;
	public bool ambientTemperatureAvailable = true;
	public bool relativeHumidityAvailable = true;	
	
	// Actual Values
    public Vector3 accelerometerDebugValue = Vector3.zero;
    public Vector3 magneticFieldDebugValue = Vector3.zero;
    public Vector3 orientationDebugValue = Vector3.zero;
    public Vector3 gyroscopeDebugValue = Vector3.zero;
    public float lightDebugValue = 0;
    public float pressureDebugValue = 0;
    public float temperatureDebugValue = 0;
    public float proximityDebugValue = 0;
    public Vector3 gravityDebugValue = Vector3.zero;
    public Vector3 linearAccelerationDebugValue = Vector3.zero;
    public Vector3 rotationVectorDebugValue = new Vector3(0, -75.79f, 0);
    public Vector3 getOrientationDebugValue = Vector3.zero;
	public float ambientTemperatureDebugValue = 0;
	public float relativeHumidityDebugValue = 0;
	
    private const float AltitudeCoef = 1.0f / 5.255f;
	
	protected override void AwakeDevice()
    {
		for (var i = 1; i <= Sensor.Count; i++) 
		{
			// fill the sensor information array with debug values
			Sensors[i] = new Information(GetSensorDebugAvailable(i), 1, 0, "DEBUG", 0, 100, "DEBUG", 0, Description[i]);
		}
	}
	
    protected override void DisableDevice()
    {
        // Nothing to do
    }
	
    protected override bool ActivateDeviceSensor(Type sensorID, Sensor.Delay sensorSpeed)
    {
		SetSensorOn(sensorID);
		return true;
	}
	
	protected override bool DeactivateDeviceSensor(Type sensorID)
    {
		SetSensorOff(sensorID);
		return true;
	}
	
	protected override Vector3 GetDeviceSensor(Type sensorID)
    {
		Get(sensorID).gotFirstValue = true;
			
	    // if not everything is fine, use debug values (can be set in the inspector)
	    switch (sensorID)
	    {
	        case Type.Accelerometer:
	            return accelerometerDebugValue;
	        case Type.Gravity:
	            return gravityDebugValue;
	        case Type.Gyroscope:
	            return gyroscopeDebugValue;
	        case Type.Light:
	            return new Vector3(lightDebugValue, 0, 0);
	        case Type.LinearAcceleration:
	            return linearAccelerationDebugValue;
	        case Type.MagneticField:
	            return magneticFieldDebugValue;
	        case Type.Orientation:
	            return orientationDebugValue;
	        case Type.Pressure:
	            return new Vector3(pressureDebugValue, 0, 0);
	        case Type.Proximity:
	            return new Vector3(proximityDebugValue, 0, 0);
	        case Type.RotationVector:
	            return rotationVectorDebugValue;
	        case Type.Temperature:
	            return new Vector3(temperatureDebugValue, 0, 0);
			case Type.AmbientTemperature:
				return new Vector3(ambientTemperatureDebugValue, 0, 0);
			case Type.RelativeHumidity:
				return new Vector3(relativeHumidityDebugValue, 0, 0);
            default:
	            return Vector3.zero;
	    }
	}
	
	protected override Vector3 _getDeviceOrientation()
	{
		return getOrientationDebugValue;
	}
	
	protected override float GetDeviceAltitude(float pressure, float pressureAtSeaLevel = PressureValue.StandardAthmosphere)
    {
        if (pressure == 0)
        {
            return 0;
        }
        return 44330.0f * (1.0f - Mathf.Pow(pressure / pressureAtSeaLevel, AltitudeCoef));
		//	return -7 * Mathf.Log((pressure / 1000) / (pressureAtSeaLevel / 1000)) * 1000;
	}
	
	protected override Sensor.SurfaceRotation GetSurfaceRotation()
	{
		return Sensor.SurfaceRotation.Rotation0;
	}
	
	protected override Quaternion QuaternionFromDeviceRotationVector(Vector3 v)
	{
		return Quaternion.Euler(rotationVectorDebugValue);
	}
	
	protected override void CompensateDeviceOrientation(ref Vector3 k)
    {
	}
	
	protected Quaternion _getSurfaceRotationCompensation()
    {
    	return Quaternion.Euler(0, 0, 0);
	}
	
	// Available-Checkbox for every sensor for testing whether sensor fallback etc. works
	private bool GetSensorDebugAvailable(int id)
	{
		switch ((Type) id)
		{
			case Type.Accelerometer:
				return accelerometerAvailable;
			case Type.Gravity:
				return gravityAvailable;
			case Type.Gyroscope:
				return gyroscopeAvailable;
			case Type.Light:
				return lightAvailable;
			case Type.LinearAcceleration:
				return linearAccelerationAvailable;
			case Type.MagneticField:
				return magneticFieldAvailable;
			case Type.Orientation:
				return orientationAvailable;
			case Type.Pressure:
				return pressureAvailable;
			case Type.Proximity:
				return proximityAvailable;
			case Type.RotationVector:
				return rotationVectorAvailable;
			case Type.Temperature:
				return temperatureAvailable;
			case Type.AmbientTemperature:
				return ambientTemperatureAvailable;
			case Type.RelativeHumidity:
				return relativeHumidityAvailable;
			default:
				return false;
		}
	}
	
	protected override ScreenOrientation ScreenOrientationDevice {
		get {
			return ScreenOrientation.LandscapeLeft;
		}
	}
}

#endif