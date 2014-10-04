public partial class Sensor
{
	// how fast the sensor should be fetched internally // default is Game
	public enum Delay
	{
	    Fastest = 0, // 0 ms
	    Game = 1, // 20 ms
	    UI = 2, // 60 ms
	    Normal = 3  // 200 ms
	}
}