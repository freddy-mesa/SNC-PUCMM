-----------------------

prefrontal cortex, 2011-2013
contact@prefrontalcortex.de

IMPORTANT:
The plugins folder MUST be child to the root folder (/Assets/). Otherwise Unity will not see the plugins in there.
It may be that the plugins folder got moved in the upload process, so you have to move it back by hand.

-----------------------
GyroDroid 4.0
-----------------------



Change notes:

---

RELEASED:
4.0
* partial iOS-crosscompile support including linear acceleration
* complete restructuring of scripts/classes to accomodate multiplatform usage
* added two new sensors, RelativeHumidity and AmbientTemperature
* lots of tweaks for Unity 4.2 (that release broke stuff!)

3.5
* Compilation for Standalone/iOS does not give Android specific errors anymore	
	
3.1
* Rotation mode selection implemented
  - some devices work perfect with RotationVector, but many work better with Orientation+Acceleration.
  - if you want to use only devices you have tested GyroDroid on, go with RotationVector.
  - if you want to publish to market, consider choosing Orientation+Acceleration for more supported devices.
* Exploration Demo scene (changing the target / starting point of the camera)
* Relative Movement Demo
* Sensor Statistics scene fixed (caused crash on some devices)
* Scroll bars for scene selection

2.5
* Rotation is fixed for all our test devices - drop a mail if some sensor does not work for some device in some orientation.
* Other modes than "Landscape Left" may be used for orientation (even Auto).
* LinearAcceleration (not available before Android 2.3) does what one would intuitively expect.

2.0
* Complete Rewrite, now supports each and every sensor Android has to offer.

1.0
* Initial Release.




IMPORTANT:
The plugins folder MUST be child to the root folder (/Assets/). Otherwise Unity will not see the plugins in there.
It may be that the plugins folder got moved in the upload process, so you have to move it back by hand.



You find an .unitypackage with the most important things in the GyroDroid folder:
	GyroDroid-minimal.zip --> contains a GyroDroid-minimal.unitypackage.

Tested with:
	- Asus Transformer TF300T	- Android 4.0 and Android 4.1
	- Samsung Galaxy Nexus		- Android 4.0 and Android 4.1
	- Samsung Galaxy Tab 7" 	- Android 2.2
	- Samsung Galaxy Tab 10.1v 	- Android 3.0
	- Motorola Xoom 			- Android 3.1, 3.2, 4.0, 4.1
	- Samsung Google Nexus S 	- Android 2.3.3, 2.3.4, 4.1, 4.2
	- Motorola Milestone 		- Android 2.2
	- LG P920 (Optimus 3D)
	- Samsung Galaxy S2
	- Samsung Galaxy S
	- Galaxy Nexus
	- LG Optimus P700			(does not work very well)
	- ...and so on.
	
iOS-crosscompile tested with:
	- iPad Mini
	- iPhone 4
	- iPad 2 (older GyroDroid version, not sure whether it still works!)



Simple Usage:
	- place the plugins folder and the GyroDroid folder in your project
	- drag the MinimalSensorCamera prefab in your project
	- done.
	
Debugging in the editor:
	- you cannot access the real sensors in the editor, not even with Unity Remote
	- you can place Sensor.cs somewhere in your scene and manipulate the sensor values that are used in the editor
	- e.g. change the SensorDebugValues.light value to see if your light handling works fine
	- even if you don't do that, on scene startup, a SensorHolder gameObject gets created which provides the same functionality as putting the
	  Sensor.cs on an object - so you can test and change values whenever you like
	
Technical Explanation and API usage:
	- Sensor.cs connects to a native Java library which reads all the sensors
		- for this purpose, there is a folder called "Android" in your plugins folder which you need.
	
	SensorHelper API:
	
	- if you want to use the device rotation, consider using the SensorHelper class:
		1) Call SensorHelper.ActivateRotation(); on Start
		2) Use 
			transform.rotation = SensorHelper.rotation
		   in your Update function to rotate your camera the same way your device is rotated.
	
	Sensor API:
	
	- Sensor.cs provides a lot of constants and functions to make it as easy as possible to use it:
		1) Call Sensor.Activate(Sensor.Type.<theTypeYouWant>):
		    Sensor.Type.Accelerometer
		    Sensor.Type.MagneticField
		    Sensor.Type.Orientation
		    Sensor.Type.Gyroscope
		    Sensor.Type.Light
		    Sensor.Type.Pressure
		    Sensor.Type.Temperature
		    Sensor.Type.Proximity
		    Sensor.Type.Gravity
		    Sensor.Type.LinearAcceleration
		    Sensor.Type.RotationVector
		    Sensor.Type.RelativeHumidity
		    Sensor.Type.AmbientTemperature
		    
		2) Then, simply use the predefined variable for this type from anywhere inside your project:
			For faster access to the rotation quaternion:
			(this uses the Sensor.Type.RotationVector, so make sure to first call Sensor.Activate(Sensor.Type.RotationVector like described above)
			Quaternion rotationQuaternion // you can directly assign that to transform.rotation
		
		 	Vector3 accelerometer		// [meters/second^2]
			Vector3 magneticField		// [uT] (micro-Tesla)
			Vector3 orientation			// [degrees]
			Vector3 gyroscope			// [radians/second]
			float light 				// [lux]
			float pressure 				// [atm]
			float temperature 			// [°C]
			float proximity				// [cm] 
			Vector3 gravity 			// [meters/second^2]
			Vector3 linearAcceleration	// [meters/second^2]
			Vector3 rotationVector 		// [last three values of a quaternion]
			float relativeHumidity		// [percent]
			float ambientTemperature	// [°C]
			
		3) You can get information about a particlar sensor (availability, vendor, accuracy, maximum range ...):
			Sensor.Get(Sensor.Type.Light).maximumRange
			
			See the Statistics Demo for reference!
	
		4) There are most of the constants provided by the Android SDK available, like
			Sensor.LightValue.Sunlight
			Sensor.GravityValue.Neptune (to detect if your user is on Neptune, yeah)
			Sensor.PressureValue.StandardAthmosphere
			
			You can compare the value you got from the sensor to this ones to know e.g. what kind of light situation currently is.
			 
	- Try the demo scene to see many sensors in action!
	  There is also a Statistics demo, which shows you what sensors are supported by your device.

Known Issues:
Some sensors (like magnetic field) are really unreliable. Compare to the north direction reported by GoogleMaps;
they have exactly the same problem. MagneticField seems to fluctuate, sometimes randomly. This is device dependant.

Some sensors (at least LinearAcceleration, RotationVector) weren't available prior to Android 2.3.