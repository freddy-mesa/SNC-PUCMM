using SncPucmm.Controller.Navigation;
using SncPucmm.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller
{
    class WebService : MonoBehaviour
    {
        #region Atributos

        private static WebService instance = null;
        
        #endregion

        #region Propiedades

        public static bool IsConnectionAvailable { get; set; }
        private static bool IsEnter { get; set; }

        public static WebService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<WebService>();
                    if (instance == null)
                    {
                        instance = new GameObject("WebService").AddComponent<WebService>();
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Constructor

        private WebService()
        {

        }

        #endregion

        #region Metodos
        
        public static Coroutine Get(string url, System.Action<bool, string> callback)
        {
            return Instance.StartCoroutine(GetRequest(url, callback));
        }

        public static Coroutine Post(string url, string json, System.Action<bool, string> callback)
        {
            return Instance.StartCoroutine(PostRequest(url, json, callback));
        }

        private static IEnumerator GetRequest(string url, System.Action<bool, string> callback)
        {
            var webService = new HTTP.Request("GET", url);
            yield return webService.Send();
            if (webService.exception != null)
            {
                Debug.Log(webService.exception);
                if (callback != null)
                {
                    callback(false, webService.exception.Message);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(true, webService.response.Text);
                }
            }
        }

        private static IEnumerator PostRequest(string url, string json, System.Action<bool, string> callback)
        {
            var form = new WWWForm();
            form.AddField("json", json);

            var webService = new HTTP.Request(url, form);
            yield return webService.Send();
            if (webService.exception != null)
            {
                Debug.Log(webService.exception);
                if (callback != null)
                {
                    callback(false, webService.exception.Message);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(true, webService.response.Text);
                }
            }
        }

        
        void Start()
        {
            IsConnectionAvailable = false;
            IsEnter = false;
        }

        void Update()
        {
            //Mientras no haya conexion a internet
            //if (!IsConnectionAvailable && !IsEnter)
            //{
            //    IsEnter = true;
            //    StartCoroutine(UpdateService());
            //}
        }

        private IEnumerator UpdateService()
        {
            yield return new WaitForSeconds(0.1f);

            //Verificar si se puede acceder a google.com
            yield return WebService.Get("https://www.google.com", (status, response) => IsConnectionAvailable = status);

            //Esperar que se refresque el valor de IsConnectionAvailable
            yield return new WaitForSeconds(0.1f);

            //Debug.Log(IsConnectionAvailable);
            //Valor que se obtiene si el fue success el acceder a google.com
            if (IsConnectionAvailable)
            {
                string responseJson = string.Empty;

                //Obtener la ultima actualizacion del servidor
                yield return WebService.Get(
                    "http://localhost:8080/snc-pucmm-web/webservices/SncPucmmWS/updates/", 
                    (status, response) => { if (status) responseJson = response; }
                );

                //Esperar que se refresque el valor de responseJson
                yield return new WaitForSeconds(0.1f);

                //Enconding de string a Json
                //JSONObject json = new JSONObject(responseJson);
                Debug.Log(responseJson);

                ////Hacer un update a la base de datos
                //SQLiteService.GetInstance().UpdateDataBase(json);

                ////Esperar si se esta usando la navegacion
                //while (NavigationController.isUsingNavigation) 
                //    yield return new WaitForSeconds(1f);

                //var navigation = (NavigationController) ModelPoolManager.GetInstance().GetValue("navigationCtrl");
                
                ////Crear el grafo con la base de datos actualizada.
                //navigation.CreateGraph();

            }

            //Condicion para volver a entrar
            IsEnter = false;

            //Salir de la corotutina
            yield return null;
        }
        
        #endregion
    }
}
