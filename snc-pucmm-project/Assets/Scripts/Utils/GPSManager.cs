using UnityEngine;
using System.Collections;

namespace SncPucmm.Utils
{
	public class GPSManager : MonoBehaviour {
		public TextMesh lblAltitude;
		public TextMesh lblLongitude;
		public TextMesh lblLatitude;
		public TextMesh lblComponentX;
		public TextMesh lblComponentZ;
		
		public float planeAxeZ;
		public float planeAxeX;
		
		public GameObject character;
		
		const float ORIGEN_LAT = 19.448918f;
		const float ORIGEN_LON = -70.687180f;
		const float totalMetrosAncho = (950/2);
		const float totalMetrosLargo = (1150/2);
		
		const string CAMARA_NAME_1 = "Vista_1er_Persona";
		const string CAMARA_NAME_2 = "Vista_3er_Persona";
		
		float latitude = 0f;
		float longitude = 0f;
		float altitude = 0f;
		
		AndroidJavaClass gpsActivityJavaClass;
		
		void Start () {
			//AndroidJNI.AttachCurrentThread();
			//gpsActivityJavaClass = new AndroidJavaClass("com.test.app.GPSTest");
			
			lblAltitude.text = "Altitude: 0";
			lblLongitude.text = "Longitude: 0";
			lblLatitude.text = "Latitude: 0";
		}
		
		void Update() {
			//latitude = gpsActivityJavaClass.CallStatic<float>("GetLatitude");
			//longitude = gpsActivityJavaClass.CallStatic<float>("GetLongitude");
			//altitude = gpsActivityJavaClass.CallStatic<float>("GetAltitude");
			//var accuracy = gpsActivityJavaClass.CallStatic<float>("GetAccuracy");
			
			//longitude = -70.684535f;
			//latitude = 19.443905f;
			
			//planeAxeX = ORIGEN_LON + Mathf.Abs (ORIGEN_LON - longitude);
			//planeAxeY = ORIGEN_LAT + Mathf.Abs (ORIGEN_LAT - latitude);
			
			lblAltitude.text = "Altitude: " + altitude.ToString ();
			lblLongitude.text = "Longitude: " + longitude.ToString ();
			lblLatitude.text = "Latitude: " + latitude.ToString ();
			
			planeAxeX = Mathf.Abs(getDistanciaComponentX());
			planeAxeZ = Mathf.Abs(getDistanciaComponentZ());
			
			lblComponentX.text = planeAxeX.ToString();
			lblComponentZ.text = planeAxeZ.ToString();
			
			if(planeAxeX >= totalMetrosAncho)
			{
				planeAxeX -= totalMetrosAncho;
			} 
			else
			{
				planeAxeX = -(totalMetrosAncho - planeAxeX);
			}
			
			if(planeAxeZ >= totalMetrosLargo)
			{
				planeAxeZ = -(planeAxeZ - totalMetrosLargo);
			}
			else
			{
				planeAxeZ = totalMetrosLargo - planeAxeZ;
			}
			
			lblComponentX.text = "Pos X: " + planeAxeX.ToString();
			lblComponentZ.text = "Pos Z: " + planeAxeZ.ToString();
			
			character.transform.position = new Vector3(planeAxeX, 0.1f, planeAxeZ);
			
			if(this.name.Equals("Vista_1er_Persona"))
			{
				this.transform.position = new Vector3(
					planeAxeX, this.transform.position.y, planeAxeZ
					);
			}
		}
		
		public double Distance(float lat1, float lon1, float lat2, float lon2){
			var R = 6378.137; // Radius of earth in KM
			var dLat = (lat2 - lat1) * Mathf.PI / 180;
			var dLon = (lon2 - lon1) * Mathf.PI / 180;
			var a = Mathf.Sin(dLat/2) * Mathf.Sin(dLat/2) +
				Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
					Mathf.Sin(dLon/2) * Mathf.Sin(dLon/2);
			var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1-a));
			var d = R * c;
			return d * 1000f; // meters
		}
		
		public float getDistanciaComponentX(){
			var componentAxeX = Distance(ORIGEN_LAT,ORIGEN_LON,ORIGEN_LAT,longitude);
			return (float)componentAxeX;
		}
		
		public float getDistanciaComponentZ(){
			var componentAxeZ = Distance(ORIGEN_LAT,ORIGEN_LON,latitude,ORIGEN_LON);
			return (float)componentAxeZ;
		}
	}	
}