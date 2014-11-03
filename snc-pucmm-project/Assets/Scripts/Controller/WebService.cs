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

        private bool IsEnterToUpdateModel { get; set; }
        private bool IsEnterToUpdateTours { get; set; }
        private bool IsUserLogIn
        {
            get
            {
                return ModelPoolManager.GetInstance().Contains("Usuario");
            }
        }
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
            IsEnterToUpdateModel = true;
            IsEnterToUpdateTours = true;
        }

        void Update()
        {
            //Mientras no haya conexion a internet
            //if (IsEnterToUpdateModel)
            //{
            //    IsEnterToUpdateModel = false;
            //    StartCoroutine(UpdateModelService());
            //}

            //if (IsUserLogIn && IsEnterToUpdateTours)
            //{
            //    IsEnterToUpdateTours = false;
            //    StartCoroutine(UpdateToursService());
            //}
        }

        private IEnumerator UpdateModelService()
        {
            bool IsConnectionAvailable = false;
            string responseJson = string.Empty;

            //Obtener la ultima actualizacion del servidor
            yield return WebService.Get(
                "http://localhost:8080/snc-pucmm-web/webservices/SncPucmmWS/model/updates/", 
                (status, response) => 
                {
                    if (status) responseJson = response;
                    IsConnectionAvailable = status;
                }
            );

            //Esperar que se refresque el valor de responseJson
            yield return new WaitForSeconds(0.1f);

            if (IsConnectionAvailable)
            {
                IsEnterToUpdateModel = false;

                //Enconding de string a Json
                JSONObject json = new JSONObject(responseJson);
                //Debug.Log(responseJson);

                ////Hacer un update a la base de datos
                SQLiteService.GetInstance().UpdateModel(json);

                var navigation = (NavigationController) ModelPoolManager.GetInstance().GetValue("navigationCtrl");

                ////Crear el grafo con la base de datos actualizada.
                navigation.CreateGraph();
            }
            else
            {
                //Condicion para volver a entrar
                IsEnterToUpdateModel = true;
            }

            //Salir de la corotutina
            yield return null;
        }

        private IEnumerator UpdateToursService()
        {
            bool IsConnectionAvailable = false;
            string responseJson = string.Empty;
            JSONObject json = SQLiteService.GetInstance().DataSynchronization();

            //Obtener la ultima actualizacion del servidor
            yield return WebService.Post(
                "http://localhost:8080/snc-pucmm-web/webservices/SncPucmmWS/tour/updates/",
                json.ToString(),
                (status, response) =>
                {
                    if (status) responseJson = response;
                    IsConnectionAvailable = status;
                }
            );

            //Esperar que se refresque el valor de responseJson
            yield return new WaitForSeconds(0.1f);

            if (IsConnectionAvailable)
            {
                IsEnterToUpdateTours = false;

                //Enconding de string a Json
                json = new JSONObject(responseJson);
                //Debug.Log(responseJson);

                ////Hacer un update a la base de datos
                SQLiteService.GetInstance().UpdateTours(json);
            }
            else
            {
                //Condicion para volver a entrar
                IsEnterToUpdateTours = true;
            }

            //Salir de la corotutina esperarando 10 min 
            yield return new WaitForSeconds(60 * 10);
        }
        
        #endregion
    }
}
