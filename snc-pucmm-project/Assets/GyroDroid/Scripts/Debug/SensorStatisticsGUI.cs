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

public class SensorStatisticsGUI : MonoBehaviour
{
    public GUISkin guiSkin;
	
	void Start () 
	{
		StartCoroutine(StartEverythingSlowly());
	}
	
	IEnumerator StartEverythingSlowly()
	{
		// Some devices seem to crash when activating a lot of sensors immediately.
		// To prevent this for this scene, we start one sensor per frame.
		
		Sensor.Activate(Sensor.Type.Accelerometer);
        yield return new WaitForSeconds(0.1f);
		Sensor.Activate(Sensor.Type.Temperature);
        yield return new WaitForSeconds(0.1f);
		Sensor.Activate(Sensor.Type.MagneticField);
        yield return new WaitForSeconds(0.1f);
		Sensor.Activate(Sensor.Type.RotationVector);
        yield return new WaitForSeconds(0.1f);
		Sensor.Activate(Sensor.Type.LinearAcceleration);
        yield return new WaitForSeconds(0.1f);
		Sensor.Activate(Sensor.Type.Gravity);
        yield return new WaitForSeconds(0.1f);
		Sensor.Activate(Sensor.Type.Gyroscope);
        yield return new WaitForSeconds(0.1f);
		Sensor.Activate(Sensor.Type.Light);
        yield return new WaitForSeconds(0.1f);
		Sensor.Activate(Sensor.Type.Orientation);
        yield return new WaitForSeconds(0.1f);
		Sensor.Activate(Sensor.Type.Pressure);
        yield return new WaitForSeconds(0.1f);
		Sensor.Activate(Sensor.Type.Temperature);
        yield return new WaitForSeconds(0.1f);
		Sensor.Activate(Sensor.Type.AmbientTemperature);
		yield return new WaitForSeconds(0.1f);
		Sensor.Activate(Sensor.Type.RelativeHumidity);
		yield return new WaitForSeconds(0.1f);
		
		// wait a moment
		yield return new WaitForSeconds(0.2f);
		
		// activate GUI
		_showGui = true;
	}

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = SensorHelper.Rotation;
        transform.localRotation = Sensor.rotationQuaternion;
	}
		
	Vector2 _scrollPosition;
    readonly Color _guiColor = new Color(0.8f,0.8f,0.8f); 
	bool _showGui = false;
	int[] W = new int[]{120,50,50,200,60,85,90,130};
	
	int C = 0;
	void OnGUI()
	{
		if (!_showGui)
		{
			return;
		}
		
		GUI.skin = guiSkin;
		// show all sensors and values in a big, fat table
		// Remember : You can only see on your device what sensors are supported, not in the editor.
		GUI.color = _guiColor;
		
		GUILayout.BeginArea(new Rect(5,5,Screen.width-10, Screen.height-10));
		_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
		// GUILayout.BeginArea(new Rect(0, 150, 1000,800));
		
		
		GUILayout.BeginHorizontal();
		{
			C = 0;
			GUILayout.Label("Sensor", GUILayout.Width(W[C++]));
			// GUILayout.Label("#", GUILayout.Width(20));
			GUILayout.Label("Exists", GUILayout.Width(W[C++]));
			GUILayout.Label("Active", GUILayout.Width(W[C++]));
			GUILayout.Label("Name", GUILayout.Width(W[C++]));
			GUILayout.Label("Power", GUILayout.Width(W[C++]));
			GUILayout.Label("Resolution", GUILayout.Width(W[C++]));
			GUILayout.Label("MaxRange", GUILayout.Width(W[C++]));
			// GUILayout.Label("MinDelay", GUILayout.Width(60));
			GUILayout.Label("Values", GUILayout.Width(W[C++]));
			// if (GUILayout.Button(s.active?"Deactivate":"Activate", GUILayout.Width(90)))
			// {
			//		if (s.active)
			//		{
			//			Sensor.Deactivate(i);
			//		}
			//		else
			//		{
			//			Sensor.Activate(i);
			//		}
			//	}
		}
		GUILayout.EndHorizontal();
		
		GUILayout.Label("");
		
		for (var i = 1; i <= Sensor.Count + 1; i++)
		{
			C = 0;
			var s = Sensor.Get((Sensor.Type) i);
		    if (s == null)
		    {
		        continue;
		    }
		    GUILayout.BeginHorizontal();
		    {
		        GUILayout.Label("" + s.description, GUILayout.Width(W[C++]));
		        GUILayout.Label("" + (s.available?"Yes":"No"), GUILayout.Width(W[C++]));
		        GUILayout.Label(s.active ? "X" : "O", GUILayout.Width(W[C++]));
		        GUILayout.Label("" + s.name, GUILayout.Width(W[C++]));
		        GUILayout.Label("" + s.power, GUILayout.Width(W[C++]));
		        GUILayout.Label("" + s.resolution.ToString("F2"), GUILayout.Width(W[C++]));
		        GUILayout.Label("" + s.maximumRange, GUILayout.Width(W[C++]));
		        // GUILayout.Label("" + s.minDelay, GUILayout.Width(60));
		        GUILayout.Label("" + s.values, GUILayout.Width(W[C++]));
					
		        if (s.available)
		        {
		            if (GUILayout.Button(s.active?"Deactivate":"Activate", GUILayout.Width(110)))
		            {
		                if (s.active)
		                {
		                    Sensor.Deactivate((Sensor.Type) i);
		                }
		                else
		                {
		                    Sensor.Activate((Sensor.Type) i);
		                }
		            }
		        }
		        else
		        {
		            GUILayout.Label("Not available", GUILayout.Width(110));
		        }
		    }
		    GUILayout.EndHorizontal();
		}
		GUILayout.Label("");	
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Best rotation value", GUILayout.Width(W[0]));
			GUILayout.Label("(provided by SensorHelper.rotation)", GUILayout.Width(510));
			GUILayout.Label("" + SensorHelper.rotation, GUILayout.Width(120));
		}
		GUILayout.EndHorizontal();
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("getOrientation", GUILayout.Width(W[0]));
			GUILayout.Label("Needs MagneticField and Accelerometer to be enabled. Gets fused from the two.", GUILayout.Width(510));
			GUILayout.Label("" + Sensor.GetOrientation(), GUILayout.Width(120));
		}
		GUILayout.EndHorizontal();
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Rotation Quaternion", GUILayout.Width(W[0]));
			GUILayout.Label("Calculated from rotation vector. Best accuracy.", GUILayout.Width(510));
			GUILayout.Label("" + Sensor.rotationQuaternion, GUILayout.Width(120));
			try
			{
            	GUILayout.Label("" + Sensor.rotationQuaternion.eulerAngles, GUILayout.Width(120));
			}
			catch
			{
				
			}
		}
		GUILayout.EndHorizontal();
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("getAltitude", GUILayout.Width(W[0]));
			GUILayout.Label("Calculated from pressure.", GUILayout.Width(510));
			GUILayout.Label("" + Sensor.GetAltitude(), GUILayout.Width(120));
		}
		GUILayout.EndHorizontal();
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("SurfaceRotation", GUILayout.Width(120));
			GUILayout.Label("Device surface rotation.", GUILayout.Width(510));
			GUILayout.Label("" + Sensor.surfaceRotation, GUILayout.Width(120));
		}
		GUILayout.EndHorizontal();
		
		if (Input.gyro.enabled)
		{
	        GUILayout.BeginHorizontal();
		    {
	            GUILayout.Label("Gyro", GUILayout.Width(W[0]));
	            GUILayout.Label("Attitude", GUILayout.Width(120));
		        GUILayout.Label("" + Input.gyro.attitude, GUILayout.Width(120));
	            GUILayout.Label("" + Input.gyro.attitude.eulerAngles, GUILayout.Width(120));
		    }
	        GUILayout.EndHorizontal();
		}
		
		if (Input.compass.enabled)
		{
	        GUILayout.BeginHorizontal();
		    {
	            GUILayout.Label("Compass", GUILayout.Width(W[0]));
	            GUILayout.Label("Raw Vector", GUILayout.Width(120));
	            GUILayout.Label("" + Input.compass.rawVector, GUILayout.Width(120));
	        }
	        GUILayout.EndHorizontal();
		}
		if (Application.isEditor)
		{
			GUILayout.Label("WARNING: Sensors can only be accessed on an Android device, not in the editor.");
		}

	    GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
}