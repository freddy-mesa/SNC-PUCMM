// PFC - prefrontal cortex
// Full Android Sensor Access for Unity3D
// Contact:
// 		contact.prefrontalcortex@gmail.com

using UnityEngine;
using System.Collections;

public class LightControl : MonoBehaviour {
	
	public Color dayBackgroundColor;
	public Color nightBackgroundColor;
	public Color darkAmbient = new Color(0.1f,0.1f,0.1f);
	public Color lightAmbient = new Color(0.7f,0.7f,0.7f);
	
	// Use this for initialization
	void Start () {
		// activate Light sensor
		Sensor.Activate(Sensor.Type.Light);
	}
	
	// Update is called once per frame
	void Update () {
		// fetch light sensor
		float lightValue = Sensor.light;
		
		// compare to predefined LightValue constants
		if(lightValue < Sensor.LightValue.Cloudy)
			ItIsNight(true);
		else
			ItIsNight(false);
	}
	
	void ItIsNight(bool on) {
		// slowly fade colors and ambientLight settings
		if(on) {
			Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, nightBackgroundColor, Time.deltaTime);
			RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, darkAmbient, Time.deltaTime);
		}
		else {
			Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, dayBackgroundColor, Time.deltaTime);
			RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, lightAmbient, Time.deltaTime);
		}		
	}
}