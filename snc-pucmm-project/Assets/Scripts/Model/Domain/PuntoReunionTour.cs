using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class PuntoReunionTour : IJson
    {
        #region Atributos

        public int? idPuntoReunionTour;
        public int? secuenciaPuntoReunion;
        public Nodo nodo;
        public Tour tour;

        #endregion

        #region Constructor

        public PuntoReunionTour()
        {

        }

        public PuntoReunionTour(JSONObject json)
        {
            Decoding(json);
        }

        #endregion

        #region Metodos

        private void Decoding(JSONObject json)
        {
            for (int i = 0; i < json.list.Count; i++)
            {
                if (!json.list[i].IsNull)
                {
                    string key = (string)json.keys[i];

                    if (key == "idPuntoReunionTour")
                        this.idPuntoReunionTour = Convert.ToInt32(json.list[i].n);
                    else if (key == "secuenciaPuntoReunion")
                        this.secuenciaPuntoReunion = Convert.ToInt32(json.list[i].n);
                    else if (key == "tour")
                        this.tour = new Tour(json.list[i]);
                    else 
                        this.nodo = new Nodo(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            if(idPuntoReunionTour.HasValue)
                json.AddField("idPuntoReunionTour", idPuntoReunionTour.Value);
            if(secuenciaPuntoReunion.HasValue)
                json.AddField("secuenciaPuntoReunion", secuenciaPuntoReunion.Value);
            if (tour != null)
                json.AddField("tour", tour.ToJson());
            if(nodo != null)
                json.AddField("nodo", nodo.ToJson());

            return json;
        }

        public override string ToString()
        {
            return String.Format("PuntoReunionTour [idPuntoReunionTour: {0}, secuenciaPuntoReunion: {1}]", 
                idPuntoReunionTour.HasValue ? idPuntoReunionTour.Value.ToString() : string.Empty, 
                secuenciaPuntoReunion.HasValue ? secuenciaPuntoReunion.Value.ToString() : string.Empty
            );
        }

        public static JSONObject ToJsonArray(List<PuntoReunionTour> puntoReunionTourList)
        {
            JSONObject json = new JSONObject(JSONObject.Type.ARRAY);

            foreach(var puntoReunion in puntoReunionTourList)
            {
                json.Add(puntoReunion.ToJson());
            }

            return json;
        }

        public static List<PuntoReunionTour> ToPuntoReunionTourList(JSONObject json){
            List<PuntoReunionTour> puntoReunionTourList = new List<PuntoReunionTour>();
            for (int i = 0; i < json.Count; ++i)
            {
                puntoReunionTourList.Add(new PuntoReunionTour(json.list[i])); 
            }

            return puntoReunionTourList;
        }

        #endregion
    }
}
