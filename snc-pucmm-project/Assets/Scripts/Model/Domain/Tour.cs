using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Tour : IJson
    {
        #region Atributos

        public int? idTour;
        public string nombreTour;
        public DateTime? fechaCreacion;
        public DateTime? fechaInicio;
        public DateTime? fechaFin;
        public int? idUsuario;

        #endregion

        #region Constructor

        public Tour()
        {

        }

        public Tour(JSONObject json)
        {
            Decoding(json);
        }

        #endregion

        #region Metodos

        private void Decoding(JSONObject json)
        {
            DateTime temp = new DateTime();
            for (int i = 0; i < json.list.Count; i++)
            {
                if (!json.list[i].IsNull)
                {
                    string key = (string)json.keys[i];

                    if (key == "id")
                        this.idTour = Convert.ToInt32(json.list[i].n);
                    else if (key == "nombreTour")
                        this.nombreTour = json.list[i].str;
                    else if (key == "fechaCreacion" && DateTime.TryParseExact(json.list[i].str, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                        this.fechaCreacion = temp;
                    else if (key == "fechaInicio" && DateTime.TryParseExact(json.list[i].str, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                        this.fechaInicio = temp;
                    else if (key == "fechaFin" && DateTime.TryParseExact(json.list[i].str, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                        this.fechaFin = temp;
                    else
                        this.idUsuario = Convert.ToInt32(json.list[i].n);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            if (idTour.HasValue)
                json.AddField("id", idTour.Value);
            if (nombreTour != null)
                json.AddField("nombreTour", nombreTour);
            if (fechaCreacion.HasValue)
                json.AddField("fechaCreacion", fechaCreacion.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if (fechaInicio.HasValue)
                json.AddField("fechaInicio", fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if (fechaFin.HasValue)
                json.AddField("fechaFin", fechaFin.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if (idUsuario.HasValue)
                json.AddField("idUsuarioCreador", idUsuario.Value);

            return json;
        }

        public override string ToString()
        {
            return String.Format("Tour [idTour: {0}, nombreTour: {1}]", idTour.HasValue ? idTour.Value.ToString() : string.Empty, nombreTour);
        }

        public static List<Tour> ToTourList(JSONObject json)
        {
            List<Tour> tourList = new List<Tour>();
            for (int i = 0; i < json.Count; ++i)
            {
                tourList.Add(new Tour(json.list[i]));
            }

            return tourList;
        }

        #endregion
    }
}
