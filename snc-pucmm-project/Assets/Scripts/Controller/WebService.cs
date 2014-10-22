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

        #endregion
    }
}
