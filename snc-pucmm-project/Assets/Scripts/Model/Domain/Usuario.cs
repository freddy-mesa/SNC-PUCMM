using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Usuario
    {
        #region Atributos

        public int idusuario;
        public string nombre;
        public string apellido;
        public string contrasena;
        public string usuario;
        public Tipousuario tipousuario;
        public Cuentafacebook cuentafacebook;

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
                string key = (string)json.keys[i];

                if (key == "idUsuario")
                {
                    this.idusuario = (int)json.list[i].n;
                }
                else if (key == "nombre")
                {
                    this.nombre = json.list[i].str;
                }
                else if (key == "apellido")
                {
                    this.apellido = json.list[i].str;
                }
                else if (key == "contrasena")
                {
                    this.contrasena = json.list[i].str;
                }
                else if (key == "contrasena")
                {
                    this.usuario = json.list[i].str;
                }
                else if (key == "tipousuario")
                {
                    this.tipousuario = new Tipousuario(json.list[i]);
                }
                else
                {
                    this.cuentafacebook = new Cuentafacebook(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            json.AddField("idusuario", idusuario);
            json.AddField("nombre", nombre);
            json.AddField("apellido", apellido);
            json.AddField("usuario", usuario);
            json.AddField("contrasena", contrasena);
            if (tipousuario != null)
            {
                json.AddField("tipousuario", tipousuario.ToJson());
            }
            if (cuentafacebook != null)
            {
                json.AddField("cuentafacebook", cuentafacebook.ToJson());
            }
            
            return json;
        }

        public override string ToString()
        {
            return String.Format("Usuario [idUsuario = {0}]", this.idusuario);
        }

        #endregion
    }
}
