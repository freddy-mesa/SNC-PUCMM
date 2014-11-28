using SncPucmm.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.View
{
	public class UICamaraControl : MonoBehaviour
	{
		#region Atributos

		static Camera PrimeraPersona;
		static Camera TerceraPersona;

		public static bool Vista_1era_Persona;
		public static bool Vista_3era_Persona;

		public static bool isTransitionAnimated;
		public static Vector3 targetTransitionPosition;

		public static Vector3 lastPosition;
		
		#endregion

		#region Metodos

		void Start()
		{
			SensorHelper.ActivateRotation();

			PrimeraPersona = UIUtils.Find("/Vista1erPersona").camera;
			TerceraPersona = UIUtils.Find("/Vista3erPersona").camera;

			CambiarCamaraTerceraPersona();
		}

		void Update()
		{
			if (Vista_1era_Persona)
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					var rotation = SensorHelper.rotation;
					PrimeraPersona.transform.eulerAngles = new Vector3(rotation.x, 0f, rotation.z);

					float planePosX = UIUtils.getXDistance(UIGPS.Longitude);
					float planePosY = UIUtils.getZDistance(UIGPS.Latitude);

					PrimeraPersona.transform.position = new Vector3(
						planePosX, this.transform.position.y, planePosY
					);
				}
			}

			if (Vista_3era_Persona)
			{
				if (isTransitionAnimated)
				{
					isTransitionAnimated = false;
					StartCoroutine(Transition());
				}
			}
		}

		public static void CambiarCamaraPrimeraPersona()
		{
			Vista_1era_Persona = true;
			PrimeraPersona.enabled = true;

			Vista_3era_Persona = false;
			TerceraPersona.enabled = false;
		}

		public static void CambiarCamaraTerceraPersona()
		{
			Vista_1era_Persona = false;
			PrimeraPersona.enabled = false;
			
			Vista_3era_Persona = true;
			TerceraPersona.enabled = true;
		}

		public static void CambiarVistas()
		{
			Vista_1era_Persona = !Vista_1era_Persona;
			Vista_3era_Persona = !Vista_3era_Persona;
		}

		IEnumerator Transition()
		{
			float t = 0.0f, transitionDuration = 0.5f;
			Vector3 startingPos = Camera.main.transform.position;
			while (t < 1.0f)
			{
				t += Time.deltaTime * (Time.timeScale / transitionDuration);

				Camera.main.transform.position = Vector3.Lerp(startingPos, targetTransitionPosition, t);
				yield return 0;
			}
		}
		
		#endregion
	}
}
