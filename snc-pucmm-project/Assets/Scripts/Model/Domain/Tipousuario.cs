using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class TipoUsuario : IJson
    {
        #region Atributos

        public int? idTipoUsuario;
        public string nombre;
        public string descripcion;
    
        #endregion

        #region Constructor

        public TipoUsuario()
        {

        }

        public TipoUsuario(JSONObject json)
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

                    if (key == "idTipoUsuario")
                        this.idTipoUsuario = Convert.ToInt32(json.list[i].n);
                    else if (key == "nombre")
                        this.nombre = json.list[i].str;
                    else
                        this.descripcion = json.list[i].str;
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            if(idTipoUsuario.HasValue)
                json.AddField("idTipoUsuario", idTipoUsuario.Value);
            if(nombre != null)
                json.AddField("nombre", nombre);
            if(descripcion != null)
                json.AddField("descripcion", descripcion);

            return json;
        }

        public override string ToString()
        {
            return String.Format("TipoUsuario [idTipoUsuario: {0}, nombre: {1}]", 
                idTipoUsuario.HasValue ? idTipoUsuario.Value.ToString() : string.Empty, nombre
            );
        }

        #endregion
    }
}
