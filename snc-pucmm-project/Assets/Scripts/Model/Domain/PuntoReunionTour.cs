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
        public int? secuencia;
        public int? idNodo;
        public int? idTour;

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

                    if (key == "id")
                        this.idPuntoReunionTour = Convert.ToInt32(json.list[i].n);
                    else if (key == "secuencia")
                        this.secuencia = Convert.ToInt32(json.list[i].n);
                    else if (key == "idTour")
                        this.idTour = Convert.ToInt32(json.list[i].n);
                    else
                        this.idNodo = Convert.ToInt32(json.list[i].n);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            if(idPuntoReunionTour.HasValue)
                json.AddField("id", idPuntoReunionTour.Value);
            if (secuencia.HasValue)
                json.AddField("secuencia", secuencia.Value);
            if (idTour.HasValue)
                json.AddField("idTour", idTour.Value);
            if (idNodo.HasValue)
                json.AddField("idNodo", idNodo.Value);

            return json;
        }

        public override string ToString()
        {
            return String.Format("PuntoReunionTour [idPuntoReunionTour: {0}, secuencia: {1}]", 
                idPuntoReunionTour.HasValue ? idPuntoReunionTour.Value.ToString() : string.Empty,
                secuencia.HasValue ? secuencia.Value.ToString() : string.Empty
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
