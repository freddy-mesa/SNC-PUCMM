using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class DetalleUsuarioTour : IJson
    {
        #region Atributos

        public int? idDetalleUsuarioTour;
        public string estado;
        public DateTime? fechaInicio;
        public DateTime? fechaLlegada;
        public PuntoReunionTour puntoReunionTour;
        public UsuarioTour usuarioTour;

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
            for (int i = 0; i < json.list.Count; i++)
            {
                if (!json.list[i].IsNull)
                {
                    string key = (string)json.keys[i];

                    if (key == "idDetalleUsuarioTour")
                        this.idDetalleUsuarioTour = Convert.ToInt32(json.list[i].n);
                    else if (key == "estado")
                        this.estado = json.list[i].str;
                    else if (key == "fechaInicio")
                        this.fechaInicio = Convert.ToDateTime(json.list[i].str);
                    else if (key == "fechaLlegada")
                        this.fechaLlegada = Convert.ToDateTime(json.list[i].str);
                    else if (key == "puntoReunionTour")
                        this.puntoReunionTour = new PuntoReunionTour(json.list[i]);
                    else
                        this.usuarioTour = new UsuarioTour(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            if (idDetalleUsuarioTour.HasValue)
                json.AddField("idDetalleUsuarioTour", idDetalleUsuarioTour.Value);
            if(estado != null)
                json.AddField("estado", estado);
            if (fechaInicio.HasValue)
                json.AddField("fechaInicio", fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if(fechaLlegada.HasValue)
                json.AddField("fechaLlegada", fechaLlegada.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if(puntoReunionTour != null)
                json.AddField("puntoReunionTour", puntoReunionTour.ToJson());
            if (usuarioTour != null)
                json.AddField("usuarioTour", usuarioTour.ToJson());

            return json;
        }

        public override string ToString()
        {
            return String.Format("DetalleUsuarioTour [ idDetalleUsuarioTour: {0}, estado: {1} ]",
                idDetalleUsuarioTour.HasValue ? idDetalleUsuarioTour.Value.ToString() : string.Empty, estado
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
