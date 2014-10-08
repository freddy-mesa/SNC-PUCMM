using UnityEngine;
using System.Collections;

namespace SncPucmm.View
{
	public class UIGPS : MonoBehaviour
	{
		#region Atributos
		public GUIText lblAltitude;
		public GUIText lblLongitude;
		public GUIText lblLatitude;
		public GUIText lblAccuracy;
		public GUIText lblComponentX;
		public GUIText lblComponentZ;

		public GameObject character;

		static float latitude;
		static float longitude;
		static float altitude;
		static float accuracy;

		float planeAxeZ;
		float planeAxeX;

		AndroidJavaClass gpsActivityJavaClass;
		#endregion

		#region Propiedades

		public static float Latitude { get { return latitude; } }
		public static float Altitude { get { return altitude; } }
		public static float Accuracy { get { return accuracy; } }
		public static float Longitude { get { return longitude; } }

		#endregion

		#region Metodos

		void Start()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				AndroidJNI.AttachCurrentThread();
				gpsActivityJavaClass = new AndroidJavaClass("com.sncpucmm.app.GPSPlugin");
			}

			lblAltitude.text = "Altitude: 0";
			lblLongitude.text = "Longitude: 0";
			lblLatitude.text = "Latitude: 0";
			lblAccuracy.text = "Accuracy: 0";

			latitude = 0f;
			longitude = 0f;
			altitude = 0f;
			accuracy = 0f;
			planeAxeZ = 0f;
			planeAxeX = 0f;
		}

		void Update()
		{
			//yield return new WaitForSeconds(2.5f);

			if (Application.platform == RuntimePlatform.Android)
			{
				latitude = gpsActivityJavaClass.CallStatic<float>("GetLatitude");
				longitude = gpsActivityJavaClass.CallStatic<float>("GetLongitude");
				altitude = gpsActivityJavaClass.CallStatic<float>("GetAltitude");
				accuracy = gpsActivityJavaClass.CallStatic<float>("GetAccuracy");

				planeAxeX = UIUtils.getXDistance(longitude);
				planeAxeZ = UIUtils.getZDistance(latitude);

				character.transform.position = new Vector3(planeAxeX, 0.1f, planeAxeZ);

				character.transform.FindChild("Range").localScale = new Vector3(
				    (float)(accuracy / 2), 0.1f, (float)(accuracy / 2)
				);

			}

			lblAltitude.text = "Altitude: " + altitude.ToString();
			lblLongitude.text = "Longitude: " + longitude.ToString();
			lblLatitude.text = "Latitude: " + latitude.ToString();
			lblAccuracy.text = "Accuracy: " + accuracy.ToString();

			lblComponentX.text = "Pos X: " + planeAxeX.ToString();
			lblComponentZ.text = "Pos Z: " + planeAxeZ.ToString();
		}

		#endregion
	}
}