using SncPucmm.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller
{
    class ConnectionChecker : MonoBehaviour
    {
        #region Atributos

        private static ConnectionChecker instance = null;
        
        #endregion

        #region Propiedades

        public static ConnectionChecker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ConnectionChecker>();
                    if (instance == null)
                    {
                        instance = new GameObject("WebService").AddComponent<ConnectionChecker>();
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Constructor

        private ConnectionChecker()
        {

        }

        #endregion

        #region Metodos
        
        public static Coroutine Get(string url, System.Action<bool> callback)
        {
            return Instance.StartCoroutine(GetRequest(url, callback));
        }

        private static IEnumerator GetRequest(string url, System.Action<bool> callback)
        {
            var webService = new HTTP.Request("GET", url);
            yield return webService.Send();
            if (webService.exception != null)
            {
                if (callback != null)
                {
                    callback(false);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback(true);
                }
            }
        }

        #endregion
    }
}
