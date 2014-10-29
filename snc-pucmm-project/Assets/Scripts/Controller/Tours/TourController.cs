using SncPucmm.Model;
using SncPucmm.Model.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller.Tours
{
    class TourController
    {
        #region Atributos

        private static TourController instance;

        #endregion

        #region Constructor

        private TourController()
        {
            
        }

        #endregion

        #region Metodos

        public static TourController GetInstance()
        {
            if (instance == null)
            {
                instance = new TourController();
            }

            return instance;
        }

        public IEnumerator SelectTours()
        {
            bool isConnectionAvailable = false;

            //Revision si hay conexion a internet
            yield return WebService.Get("https://www.google.com", (status, response) => isConnectionAvailable = status);

            //Wait for refresh isConnectionAvailable
            yield return new WaitForSeconds(0.1f);

            if (isConnectionAvailable)
            {
                string url = "http://localhost:8080/snc-pucmm-web/webservices/SncPucmmWS/Tour/list/";

                string result = string.Empty;

                yield return WebService.Get(url, (success, response) =>
                {
                    if (success)
                    {
                        //Retrieving created tour from server
                        result = response;
                    }
                });

                yield return new WaitForSeconds(0.1f);

                JSONObject json = new JSONObject(result);

                ModelPoolManager.GetInstance().Add("TourList", Tour.ToTourList(json.GetField("Tour")));

                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }

        public IEnumerator SelectUsuarioTour(Tour tour, Usuario usuario) 
        {
            bool isConnectionAvailable = false;

            //Revision si hay conexion a internet
            yield return WebService.Get("https://www.google.com", (status, response) => isConnectionAvailable = status);

            //Wait for refresh isConnectionAvailable
            yield return new WaitForSeconds(0.1f);

            if (isConnectionAvailable)
            {
                string url = "http://localhost:8080/snc-pucmm-web/webservices/SncPucmmWS/Tour/select/";

                string result = string.Empty;

                JSONObject json = new JSONObject();
                json.AddField("Tour", tour.idTour.Value);
                json.AddField("Usuario", usuario.usuario);

                yield return WebService.Post(url, json.ToString(), (success, response) =>
                {
                    if (success)
                    {
                        //Retrieving created tour from server
                        result = response;
                    }
                });

                yield return new WaitForSeconds(0.1f);

                json = new JSONObject(result);

                ModelPoolManager.GetInstance().Add(
                    "PuntoReunionTourList", PuntoReunionTour.ToPuntoReunionTourList(json.GetField("PuntoReunionTourList"))
                );

                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }

        public IEnumerator CreateTourRoleCreator(Tour tour, List<ModelNode> modelNodeList)
        {
            bool isConnectionAvailable = false;
            
            //Revision si hay conexion a internet
            yield return WebService.Get("https://www.google.com", (status,response) => isConnectionAvailable = status);

            //Wait for refresh isConnectionAvailable
            yield return new WaitForSeconds(0.1f);

            Debug.Log(isConnectionAvailable);

            if (isConnectionAvailable)
            {
                string url = "http://localhost:8080/snc-pucmm-web/webservices/SncPucmmWS/Tour/create/";

                JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
                json.AddField("Tour", tour.ToJson());

                var puntosReunion = GetPuntosReunionTour(tour, modelNodeList);
                json.AddField("PuntoReunionTourList", PuntoReunionTour.ToJsonArray(puntosReunion));

                //string result = string.Empty;

                yield return WebService.Post(url, json.ToString(),
                    (success, response) =>
                    {
                        if (success)
                        {
                            //Retrieving created tour from server
                            Debug.Log("Usuario Tour Updated!");
                        }
                    }
                );

                //yield return new WaitForSeconds(0.1f);

                //json = new JSONObject(result);

                //ModelPoolManager.GetInstance().Add("Tour", new Tour(json.GetField("Tour")));
                //ModelPoolManager.GetInstance().Add(
                //    "PuntoReunionTourList", PuntoReunionTour.ToPuntoReunionTourList(json.GetField("PuntoReunionTourList"))
                //);

                yield return new WaitForSeconds(0.1f);
            }
       
            yield return null;
        }

        public IEnumerator CreateTourRoleSuscriber(UsuarioTour usuario, List<DetalleUsuarioTour> detalleList)
        {
            bool isConnectionAvailable = false;
            
            //Revision si hay conexion a internet
            yield return WebService.Get("https://www.google.com", (status,response) => isConnectionAvailable = status);

            //Wait for refresh isConnectionAvailable
            yield return new WaitForSeconds(0.1f);

            if (isConnectionAvailable)
            {
                string url = "http://localhost:8080/snc-pucmm-web/webservices/SncPucmmWS/Tour/create/suscriber";

                JSONObject json = new JSONObject();
                json.AddField("UsuarioTour", usuario.ToJson());
                json.AddField("DetalleUsuarioTourList", DetalleUsuarioTour.ToJsonArray(detalleList));

                string result = string.Empty;

                yield return WebService.Post(url, json.ToString(),
                    (success, response) =>
                    {
                        if (success)
                        {
                            //Retrieving created tour from server
                            result = response;
                        }
                    }
                );

                yield return new WaitForSeconds(0.1f);

                json = new JSONObject(result);

                ModelPoolManager.GetInstance().Add("UsuarioTour", new UsuarioTour(json.GetField("UsuarioTour")));
                ModelPoolManager.GetInstance().Add(
                    "DetalleUsuarioTourList", DetalleUsuarioTour.ToDetalleUsuarioTourList(json.GetField("DetalleUsuarioTourList"))
                );

                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }

        public IEnumerator UpdateTourSuscriber(UsuarioTour usuario, List<DetalleUsuarioTour> detalleList)
        {
            bool isConnectionAvailable = false;

            //Revision si hay conexion a internet
            yield return WebService.Get("https://www.google.com", (status, response) => isConnectionAvailable = status);

            //Wait for refresh isConnectionAvailable
            yield return new WaitForSeconds(0.1f);

            if (isConnectionAvailable)
            {
                string url = "http://localhost:8080/snc-pucmm-web/webservices/SncPucmmWS/Tour/update/suscriber";

                JSONObject json = new JSONObject();
                json.AddField("UsuarioTour", usuario.ToJson());
                json.AddField("DetalleUsuarioTourList", DetalleUsuarioTour.ToJsonArray(detalleList));

                string result = string.Empty;

                yield return WebService.Post(url, json.ToString(),
                    (success, response) =>
                    {
                        if (success)
                        {
                            //Retrieving created tour from server
                            result = response;
                        }
                    }
                );

                yield return new WaitForSeconds(0.1f);

                json = new JSONObject(result);

                ModelPoolManager.GetInstance().Add("UsuarioTour", new UsuarioTour(json.GetField("UsuarioTour")));
                ModelPoolManager.GetInstance().Add(
                    "DetalleUsuarioTourList", DetalleUsuarioTour.ToDetalleUsuarioTourList(json.GetField("DetalleUsuarioTourList"))
                );

                yield return new WaitForSeconds(0.1f);
            }
        }

        private List<PuntoReunionTour> GetPuntosReunionTour(Tour tour, List<ModelNode> modelNodeList)
        {
            List<PuntoReunionTour> PuntoReunionTourList = new List<PuntoReunionTour>();

            for (int i = 0; i < modelNodeList.Count; ++i)
            {
                PuntoReunionTourList.Add(new PuntoReunionTour() 
                {
                    nodo = new Nodo() { idNodo = modelNodeList[i].idNodo },
                    secuenciaPuntoReunion = i+1,
                    tour = tour
                });
            }

            return PuntoReunionTourList;
        }

        #endregion
    }
}
