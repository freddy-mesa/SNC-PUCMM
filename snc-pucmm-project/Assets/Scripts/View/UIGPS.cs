using UnityEngine;
using System.Collections;

namespace SncPucmm.View
{
	public class UIGPS : MonoBehaviour
	{
		#region Atributos
		public UILabel lblAltitude;
		public UILabel lblLongitude;
		public UILabel lblLatitude;
		public UILabel lblAccuracy;
		public UILabel lblPosX;
		public UILabel lblPosZ;

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
		public static bool GPSEnterAccessControl { get; set; }

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

			GPSEnterAccessControl = true;
		}

		void Update()
		{

			if (GPSEnterAccessControl)
			{
				GPSEnterAccessControl = false;
				StartCoroutine(UpdateGPS());
			}			

			lblAltitude.text = "Altitude: " + altitude.ToString();
			lblLongitude.text = "Longitude: " + longitude.ToString();
			lblLatitude.text = "Latitude: " + latitude.ToString();
			lblAccuracy.text = "Accuracy: " + accuracy.ToString();

			lblPosX.text = "Pos X: " + planeAxeX.ToString();
			lblPosZ.text = "Pos Z: " + planeAxeZ.ToString();
		}

		IEnumerator UpdateGPS()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				latitude = gpsActivityJavaClass.CallStatic<float>("GetLatitude");
				longitude = gpsActivityJavaClass.CallStatic<float>("GetLongitude");
				altitude = gpsActivityJavaClass.CallStatic<float>("GetAltitude");
				accuracy = gpsActivityJavaClass.CallStatic<float>("GetAccuracy");

				planeAxeX = UIUtils.getXDistance(longitude);
				planeAxeZ = UIUtils.getZDistance(latitude);

			}

			StartCoroutine(CountDown());

			yield return null;
		}

		IEnumerator CountDown()
		{
			yield return new WaitForSeconds(2f);
			GPSEnterAccessControl = true;
		}

		#endregion
	}
}