using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    class LocalizacionUsuario : IJson
    {
        #region Atributos

        public int? idLocalizacionUsuario;
        public DateTime? fechaLocalizacion;
        public int? idNodo;
        public string idUsuarioFacebook;
        
        #endregion

        #region Constructor

        public LocalizacionUsuario()
        {

        }
        
        #endregion

        #region Metodos

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            if (idLocalizacionUsuario.HasValue)
                json.AddField("id", idLocalizacionUsuario.Value);
            if (fechaLocalizacion.HasValue)
                json.AddField("fechaLocalizacion", fechaLocalizacion.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if (idNodo.HasValue)
                json.AddField("idNodo", idNodo.Value);
            if (idUsuarioFacebook != null)
                json.AddField("idUsuario", idUsuarioFacebook);

            return json;
        }

        public override string ToString()
        {
            return String.Format("LocalizacionUsuario[ idUsuarioLocalizacion = {0} ]", idLocalizacionUsuario);
        }

        #endregion
    }
}