using UnityEngine;
using System.Collections;
using SncPucmm.View;

namespace SncPucmm.View
{
	public class UIGPS : UITouch 
    {
		public TextMesh lblAltitude;
		public TextMesh lblLongitude;
		public TextMesh lblLatitude;
		public TextMesh lblComponentX;
		public TextMesh lblComponentZ;
		
		public float planeAxeZ;
		public float planeAxeX;
		
		public GameObject character;
		
		const string CAMARA_NAME_1 = "Vista_1er_Persona";
		const string CAMARA_NAME_2 = "Vista_3er_Persona";
		
		float latitude = 0f;
		float longitude = 0f;
		float altitude = 0f;
		
		//AndroidJavaClass gpsActivityJavaClass;
		
		void Start () 
        {
			//AndroidJNI.AttachCurrentThread();
			//gpsActivityJavaClass = new AndroidJavaClass("com.test.app.GPSTest");
			
			lblAltitude.text = "Altitude: 0";
			lblLongitude.text = "Longitude: 0";
			lblLatitude.text = "Latitude: 0";
		}
		
		new void Update() 
        {
            base.Update();
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

            planeAxeX = Mathf.Abs(UIUtils.getXDistance(longitude));
            planeAxeZ = Mathf.Abs(UIUtils.getZDistance(latitude));
			
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
	}	
}