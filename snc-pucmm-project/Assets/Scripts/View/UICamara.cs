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
            PrimeraPersona = UIUtils.Find("Vista1erPersona").camera;
            TerceraPersona = UIUtils.Find("Vista3erPersona").camera;

            CambiarCamaraTerceraPersona();
        }

        void Update()
        {
            if (Vista_1era_Persona)
            {

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

        public static void Cambiar()
        {
            Vista_1era_Persona = !Vista_1era_Persona;
            Vista_3era_Persona = !Vista_3era_Persona;
        }
        
        #endregion
    }
}
