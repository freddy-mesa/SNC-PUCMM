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
        public long? idUsuarioFacebook;
        public string nombre;
        public string apellido;
        public string email = string.Empty;
        public string gender;

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
                        this.idUsuarioFacebook = Convert.ToInt64(json.list[i].str);
                    else if (key == "first_name")
                        this.nombre = json.list[i].str;
                    else if (key == "last_name")
                        this.apellido = json.list[i].str;
                    else if (key == "email")
                        this.email = json.list[i].str;
                    else if (key == "gender")
                        this.gender = json.list[i].str;
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            if (idUsuarioFacebook.HasValue)
                json.AddField("idUsuarioFacebook", idUsuarioFacebook.Value);
            if(nombre != null)
                json.AddField("first_name", nombre);
            if(apellido != null)
                json.AddField("last_name", apellido);
            if (email != null)
                json.AddField("email", email);
            if (gender != null)
                json.AddField("gender", gender);
            
            return json;
        }

        public override string ToString()
        {
            return String.Format("Usuario [idUsuarioFacebook: {0}, Email: {1}]", idUsuarioFacebook.HasValue ? idUsuarioFacebook.Value.ToString() : string.Empty, email);
        }

        #endregion
    }
}
