using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Puntoreuniontour
    {
        #region Atributos

        public int idpuntoreunion;
        public int secuenciapuntoreunion;
        public Localizacion idlocalizacion;
        public Tour idtour;

        #endregion

        #region Constructor

        public Puntoreuniontour()
        {

        }

        public Puntoreuniontour(JSONObject json)
        {
            Decoding(json);
        }

        #endregion

        #region Metodos

        private void Decoding(JSONObject json)
        {
            for (int i = 0; i < json.list.Count; i++)
            {
                string key = (string) json.keys[i];

                if (key == "idpuntoreunion")
                {
                    this.idpuntoreunion = Convert.ToInt32(json.list[i].n);
                }
                else if (key == "secuenciapuntoreunion")
                {
                    this.secuenciapuntoreunion = Convert.ToInt32(json.list[i].n);
                }
                else if (key == "idlocalizacion")
                {
                    this.idlocalizacion = new Localizacion(json.list[i]);
                }
                else
                {
                    this.idtour = new Tour(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            json.AddField("idpuntoreunion", idpuntoreunion);
            json.AddField("secuenciapuntoreunion", secuenciapuntoreunion);

            json.AddField("idlocalizacion", idlocalizacion.ToJson());
            json.AddField("idtour", idtour.ToJson());

            return json;
        }

        public override string ToString()
        {
            return String.Format("PuntoReunionTour [ idPuntoReunion: {0}, secuenciaPuntoReunion: {1} ]", idpuntoreunion, secuenciapuntoreunion);
        }

        public static JSONObject ToJsonArray(List<Puntoreuniontour> puntoReunionTourList)
        {
            JSONObject[] jsonObjs = new JSONObject[puntoReunionTourList.Count]; 
            for (int i = 0; i < puntoReunionTourList.Count; ++i)
            {
                jsonObjs[i] = puntoReunionTourList[i].ToJson();
            }

            JSONObject json = new JSONObject(jsonObjs);
            return json;
        }

        public static List<Puntoreuniontour> ToPuntoReunionTourList(JSONObject json){
            List<Puntoreuniontour> puntoReunionTourList = new List<Puntoreuniontour>();
            for (int i = 0; i < json.Count; ++i)
            {
                puntoReunionTourList.Add(new Puntoreuniontour(json.list[i])); 
            }

            return puntoReunionTourList;
        }

        #endregion
    }
}
