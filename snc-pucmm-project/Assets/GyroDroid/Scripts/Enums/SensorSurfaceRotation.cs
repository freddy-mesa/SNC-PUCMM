public partial class Sensor
{
		// orientation of surface (relative to native orientation)
	// e.g. Tablet PCs usually report Rotation0 in landscape mode because they are native landscape devices,
	// phones usually report Rotation90 in landscape mode because they are native portrait devices.
	// Sensors report differently in different orientations
	public enum SurfaceRotation
	{
	    Rotation0 = 0,
	    Rotation90 = 1,
	    Rotation180 = 2,
	    Rotation270 = 3
	}
	
	/*
	public enum SensorSurfaceRotation
	{
	    R0 		= 0,
	    R90 	= 1,
	    R180 	= 2,
	    R270 	= 3
	}*/
}