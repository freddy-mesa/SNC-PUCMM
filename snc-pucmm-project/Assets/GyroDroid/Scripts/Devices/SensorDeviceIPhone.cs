//#define SENSOR_DEBUG
#if UNITY_IPHONE || SENSOR_DEBUG
using UnityEngine;

class SensorDeviceIPhone : SensorDeviceUnity
{
    protected override void AwakeDevice()
    {
        // get all sensor informations (including whether they are available)
        for (var i = 1; i < 12; i++)
        {
            // fill the sensor information array
			Type sensorType = (Type) i;
            Sensors[i] = new Sensor.Information(
				IsSensorAvailable(sensorType), 
				GetMaximumRange(sensorType), 
				GetMinDelay(sensorType),
				GetName(sensorType), 
				GetPower(sensorType),                                               
				GetResolution(sensorType), 
				GetVendor(sensorType), 
				GetVersion(sensorType), 
				Description[i]);
			Sensors [i].gotFirstValue = true;
        }
        base.AwakeDevice();
    }

    #region Internal Helpers

    private static bool IsSensorAvailable(Type idx)
    {
        switch (idx)
        {
            case Type.Accelerometer:
                return SystemInfo.supportsAccelerometer;

            case Type.MagneticField:
                return true;

			case Type.Orientation:
			case Type.Gyroscope:
			case Type.Gravity:
			case Type.RotationVector:
			case Type.LinearAcceleration:
		   		//return SystemInfo.supportsGyroscope;
				return true;

            case Type.Light:
            case Type.Pressure:
            case Type.Temperature:
            case Type.Proximity:
            default:
                return false;
        }
    }

    private static float GetMaximumRange(Type idx)
    {
        switch (idx)
        {
            case Type.Accelerometer:
            case Type.MagneticField:
            case Type.Orientation:
            case Type.Gyroscope:
            case Type.Light:
            case Type.Pressure:
            case Type.Temperature:
            case Type.Proximity:
            case Type.Gravity:
            case Type.LinearAcceleration:
            case Type.RotationVector:
            default:
                return -1;
        }
    }
    
    private static int GetMinDelay(Type idx)
    {
        switch (idx)
        {
            case Type.Accelerometer:
            case Type.MagneticField:
            case Type.Orientation:
            case Type.Gyroscope:
            case Type.Light:
            case Type.Pressure:
            case Type.Temperature:
            case Type.Proximity:
            case Type.Gravity:
            case Type.LinearAcceleration:
            case Type.RotationVector:
            default:
                return -1;
        }
    }
    
    private static string GetName(Type idx)
    {
        switch (idx)
        {
#pragma warning disable 162
            case Type.Accelerometer:
				return "Accelerometer"; break;
            case Type.MagneticField:
				return "MagneticField"; break;
            case Type.Orientation:
				return "Orientation"; break;
            case Type.Gyroscope:
				return "Gyroscope"; break;
            case Type.Light:
				return "Light"; break;
            case Type.Pressure:
				return "Pressure"; break;
            case Type.Temperature:
				return "Temperature"; break;
            case Type.Proximity:
				return "Proximity"; break;
            case Type.Gravity:
				return "Gravity"; break;
            case Type.LinearAcceleration:
				return "LinearAcceleration"; break;
            case Type.RotationVector:
				return "RotationVector"; break;
            default:
                return "Unknown";
#pragma warning restore 162
        }
    }

    private static float GetPower(Type idx)
    {
        switch (idx)
        {
            case Type.Accelerometer:
            case Type.MagneticField:
            case Type.Orientation:
            case Type.Gyroscope:
            case Type.Light:
            case Type.Pressure:
            case Type.Temperature:
            case Type.Proximity:
            case Type.Gravity:
            case Type.LinearAcceleration:
            case Type.RotationVector:
            default:
                return -1;
        }
    }

    private static float GetResolution(Type idx)
    {
        switch (idx)
        {
            case Type.Accelerometer:
            case Type.MagneticField:
            case Type.Orientation:
            case Type.Gyroscope:
            case Type.Light:
            case Type.Pressure:
            case Type.Temperature:
            case Type.Proximity:
            case Type.Gravity:
            case Type.LinearAcceleration:
            case Type.RotationVector:
            default:
                return -1;
        }
    }
    
    private static string GetVendor(Type idx)
    {
        switch (idx)
        {
            case Type.Accelerometer:
            case Type.MagneticField:
            case Type.Orientation:
            case Type.Gyroscope:
            case Type.Light:
            case Type.Pressure:
            case Type.Temperature:
            case Type.Proximity:
            case Type.Gravity:
            case Type.LinearAcceleration:
            case Type.RotationVector:
            default:
                return "Unknown";
        }
    }

    private static int GetVersion(Type idx)
    {
        switch (idx)
        {
            case Type.Accelerometer:
            case Type.MagneticField:
            case Type.Orientation:
            case Type.Gyroscope:
            case Type.Light:
            case Type.Pressure:
            case Type.Temperature:
            case Type.Proximity:
            case Type.Gravity:
            case Type.LinearAcceleration:
            case Type.RotationVector:
            default:
                return -1;
        }
    }

    #endregion

    protected override bool ActivateDeviceSensor(Type sensorID, Sensor.Delay sensorSpeed)
    {
        switch (sensorID)
        {
            case Type.Accelerometer:
                SetSensorOn(sensorID);
                return true;

            case Type.Orientation:
            case Type.MagneticField:
                Input.compass.enabled = true;
				SetSensorOn(sensorID);
                return true;

            case Type.Gyroscope:
            case Type.Gravity:
			case Type.RotationVector:
			case Type.LinearAcceleration:
			    Input.gyro.enabled = true;
				SetSensorOn(sensorID);
                return true;

            case Type.Light:
            case Type.Pressure:
            case Type.Temperature:
            case Type.Proximity:
            default:
                return base.ActivateDeviceSensor(sensorID, sensorSpeed);
        }
    }

    protected override bool DeactivateDeviceSensor(Type sensorID)
    {
        switch (sensorID)
        {
            case Type.Accelerometer:
                SetSensorOff(sensorID);
                return true;
			
            case Type.Orientation:
            case Type.MagneticField:
                SetSensorOff(sensorID);
			
                Input.compass.enabled =
					Sensors[(int)Type.Orientation].active ||
					Sensors[(int)Type.MagneticField].active;
			
                return true;

            case Type.Gyroscope:
            case Type.Gravity:
			case Type.RotationVector:
			case Type.LinearAcceleration:
			    SetSensorOff(sensorID);
                
				Input.gyro.enabled =
					Sensors[(int)Type.Gyroscope].active ||
					Sensors[(int)Type.Gravity].active ||
					Sensors[(int)Type.RotationVector].active;
                
				return true;
			
			case Type.Light:
            case Type.Pressure:
            case Type.Temperature:
            case Type.Proximity:
            default:
                return base.DeactivateDeviceSensor(sensorID);
        }
    }

	Vector3 lastAcceleration;
	new Vector3 linearAcceleration;
    protected override Vector3 GetDeviceSensor(Type sensorID)
    {
        switch (sensorID)
        {
            case Type.Accelerometer:
                return Input.acceleration.normalized;

            case Type.Orientation:
                return new Vector3(Input.compass.magneticHeading, 0, 0);
            case Type.MagneticField:
                return Input.compass.rawVector;

            case Type.Gyroscope:
            case Type.RotationVector:
			    return Input.gyro.attitude.eulerAngles;
			
            case Type.Gravity:
                return Input.gyro.gravity;
			case Type.LinearAcceleration:
				linearAcceleration = Input.acceleration - lastAcceleration;
				lastAcceleration = Input.acceleration;
				return linearAcceleration * 10; // hack because of lower values for iOS devices (why?)
            case Type.Light:
            case Type.Pressure:
            case Type.Temperature:
            case Type.Proximity:
            default:
                return base.GetDeviceSensor(sensorID);
        }
    }

    protected override Vector3 _getDeviceOrientation()
    {
        return Input.compass.rawVector;
    }

    protected override Sensor.SurfaceRotation GetSurfaceRotation()
    {

        switch (Input.deviceOrientation)
        {
            case DeviceOrientation.Portrait:
                return Sensor.SurfaceRotation.Rotation0;
            case DeviceOrientation.PortraitUpsideDown:
                return Sensor.SurfaceRotation.Rotation180;
            case DeviceOrientation.LandscapeLeft:
                return Sensor.SurfaceRotation.Rotation90;
            case DeviceOrientation.LandscapeRight:
                return Sensor.SurfaceRotation.Rotation270;
            default:
                return Sensor.SurfaceRotation.Rotation0;
        }
    }

	public Quaternion GetPreMult() {
		return preMult;
	}

    protected override Quaternion QuaternionFromDeviceRotationVector(Vector3 v)
    {
		CalculateRotations();
		
		var r = preMult * Quaternion.Euler(v) * postMult;
        return r;
    }
	
	Quaternion preMult = Quaternion.identity;
	Quaternion postMult = Quaternion.identity;
	void CalculateRotations()
	{
		// Portrait devices
		if(
			   iPhone.generation == iPhoneGeneration.iPhone 
			|| iPhone.generation == iPhoneGeneration.iPhone3G
			|| iPhone.generation == iPhoneGeneration.iPhone4
			|| iPhone.generation == iPhoneGeneration.iPhone4S
#if UNITY_4_0
			|| iPhone.generation == iPhoneGeneration.iPhone5
			|| iPhone.generation == iPhoneGeneration.iPhoneUnknown
#endif
			|| iPhone.generation == iPhoneGeneration.iPodTouch1Gen
			|| iPhone.generation == iPhoneGeneration.iPodTouch2Gen
			|| iPhone.generation == iPhoneGeneration.iPodTouch3Gen
			|| iPhone.generation == iPhoneGeneration.iPodTouch4Gen
#if UNITY_4_0
			|| iPhone.generation == iPhoneGeneration.iPodTouch5Gen
			|| iPhone.generation == iPhoneGeneration.iPodTouchUnknown
#endif
			|| iPhone.generation == iPhoneGeneration.Unknown
		)
		{
			preMult = Quaternion.Euler(90,-90,0);
			// to be tested

			//if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
			//	postMult = Quaternion.Euler(0,0,90);
			//} else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
			//	postMult = Quaternion.Euler(0,0,270);
	       	//} else if (Screen.orientation == ScreenOrientation.Portrait) {
	           	postMult = Quaternion.Euler(0,0,180);
			//} else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
	        //   	postMult = Quaternion.Euler(0,0,0);
			//}
		}
		// landscape devices 
		else if(
			   iPhone.generation == iPhoneGeneration.iPad1Gen
			|| iPhone.generation == iPhoneGeneration.iPad2Gen
			|| iPhone.generation == iPhoneGeneration.iPad3Gen
#if UNITY_4_0
			|| iPhone.generation == iPhoneGeneration.iPad4Gen
			|| iPhone.generation == iPhoneGeneration.iPadMini1Gen
			|| iPhone.generation == iPhoneGeneration.iPadUnknown
#endif
		)
		{
			preMult = Quaternion.Euler(90,0,90);
			// all the same (?)
			if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
				postMult = Quaternion.Euler(0,0,180);
			} else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
				postMult = Quaternion.Euler(0,0,180);
	       	} else if (Screen.orientation == ScreenOrientation.Portrait) {
	           	postMult = Quaternion.Euler(0,0,180);
			} else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
	           	postMult = Quaternion.Euler(0,0,180);
			}
		}
	}

    protected override void CompensateDeviceOrientation(ref Vector3 k)
    {
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
				(iPhone.generation == iPhoneGeneration.iPad1Gen) ||
				(iPhone.generation == iPhoneGeneration.iPad2Gen) ||
				(iPhone.generation == iPhoneGeneration.iPad3Gen)
			)
				return ScreenOrientation.LandscapeLeft;
			else
				return ScreenOrientation.Portrait;
		}
	}
}
#endif