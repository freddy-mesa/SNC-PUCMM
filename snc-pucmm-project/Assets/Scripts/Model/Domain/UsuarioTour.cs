using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class UsuarioTour : IJson
    {
        #region Atributos

        public int? idUsuarioTour;
        public string estado;
        public DateTime? fechaInicio;
        public DateTime? fechaFin;
        public int? idTour;
        public long? idUsuarioFacebook;
        public string request;

        #endregion

        #region Constructor

        public UsuarioTour()
        {

        }

        public UsuarioTour(JSONObject json)
        {
            Decoding(json);
        }

        #endregion

        #region Metodos

        private void Decoding(JSONObject json)
        {
            DateTime temp;
            for (int i = 0; i < json.list.Count; i++)
            {
                if (!json.list[i].IsNull)
                {
                    string key = (string)json.keys[i];

                    if (key == "id")
                        this.idUsuarioTour = Convert.ToInt32(json.list[i].n);
                    else if (key == "estado")
                        this.estado = json.list[i].str;
                    else if (key == "fechaInicio" && DateTime.TryParseExact(json.list[i].str, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                        this.fechaInicio = temp;
                    else if (key == "fechaFin" && DateTime.TryParseExact(json.list[i].str, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                        this.fechaFin = temp;
                    else if (key == "idTour")
                        this.idTour = Convert.ToInt32(json.list[i].n);
                    else if (key == "idUsuarioFacebook")
                        this.idUsuarioFacebook = Convert.ToInt64(json.list[i].str);
                    else
                        this.request = json.list[i].str;
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            if (idUsuarioTour.HasValue)
                json.AddField("id", idUsuarioTour.Value);
            if(estado != null)
                json.AddField("estado", estado);
            if(fechaInicio.HasValue)
                json.AddField("fechaInicio", fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if(fechaFin.HasValue)
                json.AddField("fechaFin", fechaFin.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if (idTour.HasValue)
                json.AddField("idTour", idTour.Value);
            if (idUsuarioFacebook.HasValue)
                json.AddField("idUsuarioFacebook", idUsuarioFacebook.Value.ToString());
            if (request != null)
                json.AddField("request", request);

            return json;
        }

        public override string ToString()
        {
            return String.Format("Usuariotour [ idUsuarioTour: {0}, estadousuario: {1} ]",
                idUsuarioTour.HasValue ? idUsuarioTour.Value.ToString() : string.Empty, estado
            );
        }

        #endregion
    }
}
