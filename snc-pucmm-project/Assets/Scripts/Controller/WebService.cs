using SncPucmm.Controller.Navigation;
using SncPucmm.Database;
using SncPucmm.Model.Domain;
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
        
        public static Coroutine GET(string url, System.Action<bool, string> callback)
        {
            return Instance.StartCoroutine(GetRequest(url, callback));
        }

        public static Coroutine POST(string url, string json, System.Action<bool, string> callback)
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

            //if (IsEnterToUpdateTours)
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
            yield return WebService.GET(
                "http://localhost:8080/", 
                (status, response) => 
                {
                    if (status) responseJson = response;
                    IsConnectionAvailable = status;
                }
            );

            //Esperar que se refresque el valor de responseJson
            yield return new WaitForSeconds(0.5f);

            if (IsConnectionAvailable)
            {
                IsEnterToUpdateModel = false;

                //Enconding de string a Json
                JSONObject json = new JSONObject(responseJson);
                Debug.Log(responseJson);

                //Hacer un update a la base de datos
                using (var sqlService = new SQLiteService())
                {
                    sqlService.UpdateModel(json);
                }

                var navigation = ModelPoolManager.GetInstance().GetValue("navigationCtrl") as NavigationController;

                //Crear el grafo con la base de datos actualizada.
                navigation.CreateGraph();
            }
            else
            {
                if (!IsEnterToUpdateModel)
                {
                    StartCoroutine(CountDown(600f,value => IsEnterToUpdateModel = value));
                    yield return new WaitForSeconds(0.5f);
                }
            }

            //Salir de la corotutina
            yield return null;
        }

        private IEnumerator UpdateToursService()
        {
            bool isConnectionAvailable = false, accessControlWebService = false;
            string responseJson = string.Empty;
            //JSONObject json = SQLiteService.GetInstance().DataSynchronization();

            //Obtener la ultima actualizacion del servidor
            yield return WebService.GET(
                "http://localhost:8080/SNCWeb/tour/list",
                (status, response) =>
                {
                    if (status) { responseJson = response; }
                    isConnectionAvailable = status;
                    accessControlWebService = true;
                }
            );

            //Esperar que se refresque el valores
            yield return new WaitForSeconds(0.5f);

            if (accessControlWebService)
            {
                if (isConnectionAvailable)
                {
                    //Enconding de string a Json
                    JSONObject json = new JSONObject(responseJson);

                    Debug.Log("Peticion de UpdateTours al WebService: " + json.ToString());

                    //Hacer un update a la base de datos
                    using (var sqlService = new SQLiteService())
                    {
                        sqlService.UpdateTours(json);
                    }
                }

            }

            if (!IsEnterToUpdateTours)
            {
                StartCoroutine(CountDown(600, value => IsEnterToUpdateTours = value));
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }

        public IEnumerator UsuarioSignIn(string username, string password)
        {
            bool IsConnectionAvailable = false;
            string responseJson = string.Empty;
            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("username", username);
            json.AddField("password", password);

            //Obtener la ultima actualizacion del servidor
            yield return WebService.POST(
                "http://localhost:8080/SNCWeb/usuario/signin",
                json.ToString(),
                (status, response) =>
                {
                    if (status) responseJson = response;
                    IsConnectionAvailable = status;
                }
            );

            //Esperar que se refresque el valor de responseJson
            yield return new WaitForSeconds(0.5f);

            if (IsConnectionAvailable)
            {
                //Enconding de string a Json
                json = new JSONObject(responseJson);
                Debug.Log(responseJson);

                Usuario user = new Usuario(json);

                ModelPoolManager.GetInstance().Add("Usuario", user);
            }
        }

        private IEnumerator CountDown(float seconds, System.Action<bool> callback)
        {
            //Esperar 10 min
            yield return new WaitForSeconds(seconds);
            callback(true);
        }
        
        #endregion
    }
}
