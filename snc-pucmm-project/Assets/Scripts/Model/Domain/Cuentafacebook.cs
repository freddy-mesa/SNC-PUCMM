using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class CuentaFacebook : IJson
    {
        #region Atributos

        public int? idCuentaFacebook;
        public string usuarioFacebook;
        public string token;
    
        
        #endregion

        #region Constructores

        public CuentaFacebook()
        {

        }

        public CuentaFacebook(JSONObject json)
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

                    if (key == "idCuentaFacebook")
                        this.idCuentaFacebook = (int)json.list[i].n;
                    else if (key == "usuariofacebook")
                        this.usuarioFacebook = json.list[i].str;
                    else
                        this.token = json.list[i].str;
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            if (idCuentaFacebook.HasValue)
                json.AddField("idCuentaFacebook", idCuentaFacebook.Value);
            if (usuarioFacebook != null)
                json.AddField("usuarioFacebook", usuarioFacebook);
            if (token != null)
                json.AddField("token", token);

            return json;
        }

        public override string ToString()
        {
            return String.Format("Cuentafacebook [ idCuentaFacebook: {0}, usuarioFacebook: {1} ]", 
                idCuentaFacebook.HasValue ? idCuentaFacebook.Value.ToString() : string.Empty, usuarioFacebook);
        }

        #endregion
    }
}
