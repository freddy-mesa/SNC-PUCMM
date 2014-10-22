using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Detalleusuariotour
    {
        #region Atributos

        public int iddetalleusuariotour;
        public String estado;
        public DateTime fechainicio;
        public DateTime fechallegada;
        public Puntoreuniontour idpuntoreunion;
        public Usuariotour idusuariotour;

        #endregion

        #region Constructor

        public Detalleusuariotour()
        {

        }

        public Detalleusuariotour(JSONObject json)
        {
            Decoding(json);
        }

        #endregion

        #region Metodos

        private void Decoding(JSONObject json)
        {
            for (int i = 0; i < json.list.Count; i++)
            {
                string key = (string) json.keys[i];

                if (key == "iddetalleusuariotour")
                {
                    this.iddetalleusuariotour = Convert.ToInt32(json.list[i].n);
                }
                else if (key == "estado")
                {
                    this.estado = json.list[i].str;
                }
                else if (key == "fechainicio")
                {
                    this.fechainicio = Convert.ToDateTime(json.list[i].str);
                }
                else if (key == "fechallegada")
                {
                    this.fechallegada = Convert.ToDateTime(json.list[i].str);
                }
                else if (key == "idpuntoreunion")
                {
                    this.idpuntoreunion = new Puntoreuniontour(json.list[i]);
                }
                else
                {
                    this.idusuariotour = new Usuariotour(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            json.AddField("iddetalleusuariotour", iddetalleusuariotour);
            json.AddField("estado", estado);
            json.AddField("fechainicio", fechainicio.ToString("dd/MM/yyyy HH:mm:ss"));
            json.AddField("fechallegada", fechallegada.ToString("dd/MM/yyyy HH:mm:ss"));

            json.AddField("idpuntoreunion", idpuntoreunion.ToJson());
            json.AddField("idusuariotour", idusuariotour.ToJson());

            return json;
        }

        public override string ToString()
        {
            return String.Format("DetalleUsuarioTour [ idDetalleUsuarioTour: {0}, estado: {1} ]",iddetalleusuariotour, estado);
        }

        #endregion
    }
}
