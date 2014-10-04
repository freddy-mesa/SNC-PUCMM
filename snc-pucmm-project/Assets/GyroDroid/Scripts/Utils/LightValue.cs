public partial class Sensor
{
	public static class LightValue
	{
	    // light constants
	    // use these to compare to the fetched "light" value
	    public const float Cloudy = 100.0f;
	    public const float Fullmoon = 0.25f;
	    public const float NoMoon = 0.0010f;
	    public const float Overcast = 10000.0f;
	    public const float Shade = 20000.0f;
	    public const float Sunlight = 110000.0f;
	    public const float SunlightMax = 120000.0f;
	    public const float Sunrise = 400.0f;
	}
}