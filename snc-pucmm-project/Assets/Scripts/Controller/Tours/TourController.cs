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
    class TourController : MonoBehaviour
    {
        #region Atributos

        Tour currentTour;
        string url;

        #endregion

        #region Propiedades

        public string CreateTourResult { get; set; }
        public string SelectTourResult { get; set; }
        #endregion

        #region Constructor

        public TourController()
        {
            url = "http://localhost:8080/snc-pucmm-web/webservices/SncPucmmWS/tour/";
        }

        #endregion

        #region Metodos

        public IEnumerator CreateTour(Tour tour, List<ModelLocalizacion> modelLocalizacionList)
        {
            bool isConnectionAvailable = false;
            
            //Revision si hay conexion a internet
            yield return ConnectionChecker.Get("https://www.google.com", (status) => isConnectionAvailable = status );

            //Wait for refresh isConnectionAvailable
            yield return new WaitForSeconds(0.5f);

            if (isConnectionAvailable)
            {
                //Invocando el WebService para crear un tour pasandole el tour encode en JSON
                yield return WebService.Post(url + "create/", tour.ToJson().ToString(), 
                    (success, response) =>
                    {
                        if (success)
                        {
                            //Retrieving created tour from server
                            CreateTourResult = response;
                        }
                    }
                );

                //Wait for refresh
                yield return new WaitForSeconds(0.5f);

                Debug.Log("WebService Response: " + this.CreateTourResult);

                //Obtain JSON
                JSONObject jsonReceived = new JSONObject(this.CreateTourResult);

                //Created tour from server
                currentTour = new Tour(jsonReceived);

                //Converting ModelLocalizacion to Punto Reunion Tour
                var puntoReunionTourList = GetPuntoReunionTour(currentTour, modelLocalizacionList);

                //Convirtiendo a Lista de JSON
                var json = Puntoreuniontour.ToJsonArray(puntoReunionTourList).ToString();
                yield return WebService.Post(url + "create/", json, (success, response) =>
                {
                    if (success)
                    {
                        CreateTourResult = response;
                    }
                });
            }
        }

        public IEnumerator UpdateTour(Tour tour)
        {
            yield return new WaitForSeconds(1f);

            string json = tour.ToJson().ToString();

            yield return WebService.Post(url + "update/", json, (success, response) =>
            {
                if (success)
                {
                    Debug.Log(tour.ToString() + " Updated");
                }
            });

        }

        public List<Puntoreuniontour> GetPuntoReunionTour(Tour tour, List<ModelLocalizacion> modelLocalizacionList)
        {
            List<Puntoreuniontour> puntoReunionTourList = new List<Puntoreuniontour>();

            for (int i = 0; i < modelLocalizacionList.Count; ++i)
            {
                puntoReunionTourList.Add(new Puntoreuniontour() 
                {
                    idlocalizacion = new Localizacion() { idlocalizacion = modelLocalizacionList[i].idLocalizacion},
                    secuenciapuntoreunion = i+1,
                    idtour = new Tour() { idtour = tour.idtour }
                });
            }

            return puntoReunionTourList;
        }

        #endregion
    }
}
