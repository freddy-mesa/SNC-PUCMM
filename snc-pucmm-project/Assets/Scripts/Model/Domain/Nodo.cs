using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Nodo : IJson
    {
        #region Atributos

        public int? idNodo;
        public string nombre;
        public bool? activo;
        public int? edificio;
        public int? idUbicacion;


        #endregion

        #region Constructor

        public Nodo()
        {

        }

        public Nodo(JSONObject json)
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
                        this.idNodo = Convert.ToInt32(json.list[i].n);
                    else if (key == "nombre")
                        this.nombre = json.list[i].str;
                    else if (key == "activo")
                        this.activo = json.list[i].b;
                    else if (key == "edificio")
                        this.edificio = Convert.ToInt32(json.list[i].n);
                    else
                        this.idUbicacion = Convert.ToInt32(json.list[i].n);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            if(idNodo.HasValue)
                json.AddField("id", idNodo.Value);
            if (nombre != null)
                json.AddField("nombre", nombre);
            if(activo.HasValue)
                json.AddField("activo", activo.Value);
            if (edificio.HasValue)
                json.AddField("edificio", edificio.Value);
            if (idUbicacion.HasValue)
                json.AddField("idUbicacion", idUbicacion.Value);

            return json;
        }

        public override string ToString()
        {
            return String.Format("Nodo [idNodo: {0}, nombre: {1}, activo {2}]", 
                idNodo.HasValue ? idNodo.Value.ToString() : string.Empty, nombre, activo.HasValue ? activo.Value.ToString() : string.Empty
            );
        }

        #endregion
    }
}
