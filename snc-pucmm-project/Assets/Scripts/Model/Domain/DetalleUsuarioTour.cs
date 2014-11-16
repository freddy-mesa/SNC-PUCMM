using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class DetalleUsuarioTour : IJson
    {
        #region Atributos

        public int? idDetalleUsuarioTour;
        public DateTime? fechaInicio;
        public DateTime? fechaLlegada;
        public DateTime? fechaFin;
        public int? idPuntoReunionTour;
        public int? idUsuarioTour;

        #endregion

        #region Constructor

        public DetalleUsuarioTour()
        {

        }

        public DetalleUsuarioTour(JSONObject json)
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
                        this.idDetalleUsuarioTour = Convert.ToInt32(json.list[i].n);
                    else if (key == "fechaInicio" && DateTime.TryParseExact(json.list[i].str, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                        this.fechaInicio = temp;
                    else if (key == "fechaLlegada" && DateTime.TryParseExact(json.list[i].str, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                        this.fechaLlegada = temp;
                    else if (key == "fechaFin" && DateTime.TryParseExact(json.list[i].str, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                        this.fechaFin = temp;
                    else if (key == "idPuntoReunionTour")
                        this.idPuntoReunionTour = Convert.ToInt32(json.list[i].n);
                    else
                        this.idUsuarioTour = Convert.ToInt32(json.list[i].n);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            if (idDetalleUsuarioTour.HasValue)
                json.AddField("id", idDetalleUsuarioTour.Value);
            if (fechaInicio.HasValue)
                json.AddField("fechaInicio", fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if(fechaLlegada.HasValue)
                json.AddField("fechaLlegada", fechaLlegada.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if (fechaFin.HasValue)
                json.AddField("fechaFin", fechaFin.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if (idPuntoReunionTour.HasValue)
                json.AddField("idPuntoReunionTour", idPuntoReunionTour.Value);
            if (idUsuarioTour.HasValue)
                json.AddField(" ", idUsuarioTour.Value);

            return json;
        }

        public override string ToString()
        {
            return String.Format("DetalleUsuarioTour [ idDetalleUsuarioTour: {0} ]",
                idDetalleUsuarioTour.HasValue ? idDetalleUsuarioTour.Value.ToString() : string.Empty
            );
        }

        public static JSONObject ToJsonArray(List<DetalleUsuarioTour> detalleUsuarioList)
        {
            JSONObject json = new JSONObject(JSONObject.Type.ARRAY);

            foreach (var detalleUsuarioTour in detalleUsuarioList)
            {
                json.Add(detalleUsuarioTour.ToJson());
            }

            return json;
        }

        public static List<DetalleUsuarioTour> ToDetalleUsuarioTourList(JSONObject json)
        {
            List<DetalleUsuarioTour> detalleUsuarioList = new List<DetalleUsuarioTour>();
            for (int i = 0; i < json.Count; ++i)
            {
                detalleUsuarioList.Add(new DetalleUsuarioTour(json.list[i]));
            }

            return detalleUsuarioList;
        } 

        #endregion
    }
}
