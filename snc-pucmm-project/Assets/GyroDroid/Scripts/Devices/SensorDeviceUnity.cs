
using UnityEngine;
using System.Collections;

public abstract class SensorDeviceUnity : Sensor
{
	public string Error = "";
    
    protected override void AwakeDevice()
    {
    }

    protected override void DisableDevice()
    {
    }

    protected override bool ActivateDeviceSensor(Type sensorID, Sensor.Delay sensorSpeed)
    {
		Get (sensorID).gotFirstValue = true;
        return false;
    }

    protected override bool DeactivateDeviceSensor(Type sensorID)
    {
        return false;
    }
	
//	private IEnumerator StartLocationService(Type sensorID)
//	{
//	    // Start service beefore querying location
//        Input.location.Start(0.5f, 0.25f);
//
//        var maxWait = 20;
//        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
//        {
//            yield return new WaitForSeconds(1);
//            maxWait--;
//        }
//
//        // Service didn´t initialize in 20 seconds
//        if (maxWait < 1)
//        {
//			Error = "Timed out starting the location services";
//            Debug.Log(Error);
//        }
//        else
//        {
//            if (Input.location.status == LocationServiceStatus.Failed)
//            {
//                // User denied access to device location
//				Error = "User denied access to device location";
//                Debug.Log(Error);
//            }
//            else
//            {
//                Error = "GPS OK";
//                Debug.Log(Error);
//				SetSensorOn(sensorID);
//            }
//		}
//	}
	
//	private void StopLocationService(Type sensorID)
//	{
//		SetSensorOff(sensorID);
//		Input.location.Stop();
//	}

    protected override Vector3 GetDeviceSensor(Type sensorID)
    {
        switch (sensorID)
        {
			default:
				return Vector3.zero;
        }
    }

    protected override float GetDeviceAltitude(float pressure, float pressureAtSeaLevel = PressureValue.StandardAthmosphere)
    {
        return Input.location.status == LocationServiceStatus.Running ? Input.location.lastData.altitude : 0f;
    }

    /*
	protected override Quaternion _getSurfaceRotationCompensation()
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
	*/
	
	protected override ScreenOrientation ScreenOrientationDevice {
		get {
			return Screen.width >= Screen.height ? ScreenOrientation.LandscapeLeft : ScreenOrientation.Portrait;
		}
	}
}