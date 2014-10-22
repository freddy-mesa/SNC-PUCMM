using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Ubicacion
    {
        #region Atributos

        public int idubicacion;
        public String nombre;
        public String abreviacion;
    
        #endregion

        #region Constructor

        public Ubicacion()
        {

        }

        public Ubicacion(JSONObject json)
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

                if (key == "idubicacion")
                {
                    this.idubicacion = Convert.ToInt32(json.list[i].n);
                }
                else if (key == "nombre")
                {
                    this.nombre = json.list[i].str;
                }
                else
                {
                    this.abreviacion = json.list[i].str;
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            json.AddField("idubicacion", idubicacion);
            json.AddField("nombre", nombre);
            json.AddField("abreviacion", abreviacion);

            return json;
        }

        public override string ToString()
        {
            return String.Format("Ubicacion [ idubicacion: {0}, nombre: {1} ]",idubicacion,nombre);
        }

        #endregion
    }
}
