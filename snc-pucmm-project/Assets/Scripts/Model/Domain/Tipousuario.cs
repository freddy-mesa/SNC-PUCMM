using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Tipousuario
    {
        #region Atributos

        public int idtipousuario;
        public String nombre;
        public String descripcion;
    
        #endregion

        #region Constructor

        public Tipousuario()
        {

        }

        public Tipousuario(JSONObject json)
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

                if (key == "idtipousuario")
                {
                    this.idtipousuario = Convert.ToInt32(json.list[i].n);
                }
                else if (key == "nombre")
                {
                    this.nombre = json.list[i].str;
                }
                else
                {
                    this.descripcion = json.list[i].str;
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            json.AddField("idtipousuario", idtipousuario);
            json.AddField("nombre", nombre);
            json.AddField("descripcion", descripcion);

            return json;
        }

        public override string ToString()
        {
            return String.Format("Tipousuario [ idTipoUsuario: {0}, nombre: {1} ]", idtipousuario, nombre);
        }

        #endregion
    }
}
