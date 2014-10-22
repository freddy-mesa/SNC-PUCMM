using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Tour
    {
        #region Atributos

        public int idtour;
        public String nombretour;
        public DateTime fechacreacion;
        public DateTime fechainicio;
        public DateTime fechafin;
        public Usuario idusuario;

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
            for (int i = 0; i < json.list.Count; i++)
            {
                string key = (string) json.keys[i];

                if (key == "idtour")
                {
                    this.idtour = Convert.ToInt32(json.list[i].n);
                }
                else if (key == "nombretour")
                {
                    this.nombretour = json.list[i].str;
                }
                else if (key == "fechacreacion")
                {
                    this.fechacreacion = Convert.ToDateTime(json.list[i].str);
                }
                else if (key == "fechainicio")
                {
                    this.fechainicio = Convert.ToDateTime(json.list[i].str);
                }
                else if (key == "fechafin")
                {
                    this.fechafin = Convert.ToDateTime(json.list[i].str);
                }
                else
                {
                    this.idusuario = new Usuario(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            json.AddField("idtour", idtour);
            
            if (nombretour != null)
            {
                json.AddField("nombretour", nombretour);
            }
            if (fechacreacion != null)
            {
                json.AddField("fechacreacion", fechacreacion.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            if (fechainicio != null)
            {
                json.AddField("fechainicio", fechainicio.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            if (fechafin != null)
            {
                json.AddField("fechafin", fechafin.ToString("dd/MM/yyyy HH:mm:ss"));
            }
            if (idusuario != null)
            {
                json.AddField("idusuario", idusuario.ToJson());
            }

            return json;
        }

        public override string ToString()
        {
            return String.Format("Tour [ idtour: {0}, nombreTour: {1} ]", idtour, nombretour);
        }

        #endregion
    }
}
