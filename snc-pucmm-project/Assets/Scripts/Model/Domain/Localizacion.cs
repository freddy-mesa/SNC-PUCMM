using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Localizacion
    {
        #region Atributos

        public int idlocalizacion;
        public String nombre;
        public Ubicacion idubicacion;


        #endregion

        #region Constructor

        public Localizacion()
        {

        }

        public Localizacion(JSONObject json)
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

                if (key == "idlocalizacion")
                {
                    this.idlocalizacion = Convert.ToInt32(json.list[i].n);
                }
                else if (key == "nombre")
                {
                    this.nombre = json.list[i].str;
                }
                else
                {
                    this.idubicacion = new Ubicacion(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            json.AddField("idlocalizacion", idlocalizacion);
            json.AddField("nombre", nombre);
            json.AddField("idubicacion", idubicacion.ToJson());

            return json;
        }

        public override string ToString()
        {
            return String.Format("Localizacion [ idLocalizacion: {0}, nombre: {1} ]", idlocalizacion, nombre);
        }

        #endregion
    }
}
