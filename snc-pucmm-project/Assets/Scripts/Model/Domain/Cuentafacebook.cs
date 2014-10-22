using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Cuentafacebook
    {
        #region Atributos

        public int idcuentafacebook;
        public String usuariofacebook;
        public String token;
    
        
        #endregion

        #region Constructores

        public Cuentafacebook()
        {

        }

        public Cuentafacebook(JSONObject json)
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

                if (key == "idcuentafacebook")
                {
                    this.idcuentafacebook = (int)json.list[i].n;
                }
                else if (key == "usuariofacebook")
                {
                    this.usuariofacebook = json.list[i].str;
                }
                else
                {
                    this.token = json.list[i].str;
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            json.AddField("idcuentafacebook", idcuentafacebook);
            json.AddField("usuariofacebook", usuariofacebook);
            json.AddField("token", token);

            return json;
        }

        public override string ToString()
        {
            return String.Format("Cuentafacebook [ idCuentaFacebook: {0}, usuarioFacebook: {1} ]", idcuentafacebook, usuariofacebook);
        }

        #endregion
    }
}
