using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Usuario : IJson
    {
        #region Atributos

        public int? idUsuario;
        public string nombre;
        public string apellido;
        public string contrasena;
        public string usuario;
        public int? idTipoUsuario;
        public int? idCuentaFacebook;

        #endregion

        #region Constructor

        public Usuario()
        {

        }

        public Usuario(JSONObject json)
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
                        this.idUsuario = (int)json.list[i].n;
                    else if (key == "nombre")
                        this.nombre = json.list[i].str;
                    else if (key == "apellido")
                        this.apellido = json.list[i].str;
                    else if (key == "contrasena")
                        this.contrasena = json.list[i].str;
                    else if (key == "contrasena")
                        this.usuario = json.list[i].str;
                    else if (key == "idTipoUsuario")
                        this.idTipoUsuario = Convert.ToInt32(json.list[i].n);
                    else
                        this.idCuentaFacebook = Convert.ToInt32(json.list[i].n);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            if(idUsuario.HasValue)
                json.AddField("id", idUsuario.Value);
            if(nombre != null)
                json.AddField("nombre", nombre);
            if(apellido != null)
                json.AddField("apellido", apellido);
            if(usuario != null)
                json.AddField("usuario", usuario);
            if(contrasena != null)
                json.AddField("contrasena", contrasena);
            if (idTipoUsuario.HasValue)
                json.AddField("idTipoUsuario", idTipoUsuario.Value);
            if (idCuentaFacebook.HasValue)
                json.AddField("idCuentaFacebook", idCuentaFacebook.Value);
            
            return json;
        }

        public override string ToString()
        {
            return String.Format("Usuario [idUsuario: {0}, Usuario: {1}]", idUsuario.HasValue ? idUsuario.Value.ToString() : string.Empty, usuario);
        }

        #endregion
    }
}
