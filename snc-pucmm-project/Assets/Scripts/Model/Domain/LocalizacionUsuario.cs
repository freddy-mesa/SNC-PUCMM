using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    class LocalizacionUsuario : IJson
    {
        #region Atributos

        public int? idUsuarioLocalizacion;
        public DateTime? fechaLocalizacion;
        public int? idNodo;
        public int? idUsuario;
        
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

            if (idUsuarioLocalizacion.HasValue)
                json.AddField("id", idUsuarioLocalizacion.Value);
            if (fechaLocalizacion.HasValue)
                json.AddField("fechaLocalizacion", fechaLocalizacion.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if (idNodo.HasValue)
                json.AddField("idNodo", idNodo.Value);
            if (idUsuario.HasValue)
                json.AddField("idUsuario", idUsuario.Value);

            return json;
        }

        #endregion
    }
}