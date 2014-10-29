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
        public TipoUsuario tipoUsuario;
        public CuentaFacebook cuentaFacebook;

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

                    if (key == "idUsuario")
                        this.idUsuario = (int)json.list[i].n;
                    else if (key == "nombre")
                        this.nombre = json.list[i].str;
                    else if (key == "apellido")
                        this.apellido = json.list[i].str;
                    else if (key == "contrasena")
                        this.contrasena = json.list[i].str;
                    else if (key == "contrasena")
                        this.usuario = json.list[i].str;
                    else if (key == "tipoUsuario")
                        this.tipoUsuario = new TipoUsuario(json.list[i]);
                    else
                        this.cuentaFacebook = new CuentaFacebook(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            if(idUsuario.HasValue)
                json.AddField("idUsuario", idUsuario.Value);
            if(nombre != null)
                json.AddField("nombre", nombre);
            if(apellido != null)
                json.AddField("apellido", apellido);
            if(usuario != null)
                json.AddField("usuario", usuario);
            if(contrasena != null)
                json.AddField("contrasena", contrasena);
            if (tipoUsuario != null)
                json.AddField("tipoUsuario", tipoUsuario.ToJson());
            if (cuentaFacebook != null)
                json.AddField("cuentaFacebook", cuentaFacebook.ToJson());
            
            return json;
        }

        public override string ToString()
        {
            return String.Format("Usuario [idUsuario: {0}, Usuario: {1}]", idUsuario.HasValue ? idUsuario.Value.ToString() : string.Empty, usuario);
        }

        #endregion
    }
}
