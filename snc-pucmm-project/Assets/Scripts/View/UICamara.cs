using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.View
{
	public class UICamara : MonoBehaviour
	{
		#region Atributos

		static Camera PrimeraPersona;
		static Camera TerceraPersona;

		public static bool Vista_1era_Persona;
		public static bool Vista_3era_Persona;
		
		#endregion

		#region Metodos

		void Start()
		{
			SensorHelper.ActivateRotation();

			PrimeraPersona = UIUtils.Find("Vista1erPersona").camera;
			TerceraPersona = UIUtils.Find("Vista3erPersona").camera;

			CambiarCamaraTerceraPersona();
		}

		void Update()
		{
			if (Vista_1era_Persona)
			{
				var rotation = SensorHelper.rotation;
				PrimeraPersona.transform.eulerAngles = new Vector3(rotation.x, 0f, rotation.z);

				float planePosX = UIUtils.getXDistance(UIGPS.Longitude); 
				float planePosY = UIUtils.getZDistance(UIGPS.Latitude);

				PrimeraPersona.transform.position = new Vector3(
					planePosX, this.transform.position.y, planePosY
				);
			}

			if (Vista_3era_Persona)
			{

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
		
		#endregion
	}
}
