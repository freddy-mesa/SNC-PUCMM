using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Ubicacion : IJson
    {
        #region Atributos

        public int? idUbicacion;
        public int? cantidadPlantas;
        public string nombre;
        public string abreviacion;
    
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
                if (!json.list[i].IsNull)
                {
                    string key = (string)json.keys[i];

                    if (key == "id")
                        this.idUbicacion = Convert.ToInt32(json.list[i].n);
                    else if (key == "cantidadPlantas")
                        this.cantidadPlantas = Convert.ToInt32(json.list[i].n);
                    else if (key == "nombre")
                        this.nombre = json.list[i].str;
                    else
                        this.abreviacion = json.list[i].str;
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            if(idUbicacion.HasValue)
                json.AddField("id", idUbicacion.Value);
            if (cantidadPlantas.HasValue)
                json.AddField("cantidadPlantas", cantidadPlantas.Value);
            if(nombre != null)
                json.AddField("nombre", nombre);
            if(abreviacion != null)
                json.AddField("abreviacion", abreviacion);

            return json;
        }

        public override string ToString()
        {
            return String.Format("Ubicacion [idUbicacion: {0}, nombre: {1}]",idUbicacion.HasValue ? idUbicacion.Value.ToString() : string.Empty,nombre);
        }

        #endregion
    }
}
