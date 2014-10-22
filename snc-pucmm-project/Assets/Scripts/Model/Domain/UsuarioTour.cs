using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Usuariotour
    {
        #region Atributos

        public int idusuariotour;
        public String estadousuariotour;
        public DateTime fechainicio;
        public DateTime fechafin;
        public Tour idtour;
        public Usuario idusuario;


        #endregion

        #region Constructor

        public Usuariotour()
        {

        }

        public Usuariotour(JSONObject json)
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

                if (key == "idusuariotour")
                {
                    this.idusuariotour = Convert.ToInt32(json.list[i].n);
                }
                else if (key == "estadousuariotour")
                {
                    this.estadousuariotour = json.list[i].str;
                }
                else if (key == "fechainicio")
                {
                    this.fechainicio = Convert.ToDateTime(json.list[i].str);
                }
                else if (key == "fechafin")
                {
                    this.fechafin = Convert.ToDateTime(json.list[i].str);
                }
                else if (key == "idtour")
                {
                    this.idtour = new Tour(json.list[i]);
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
            
            json.AddField("idusuariotour", idusuariotour);
            json.AddField("estadousuariotour", estadousuariotour);
            json.AddField("fechainicio", fechainicio.ToString("dd/MM/yyyy HH:mm:ss"));
            json.AddField("fechafin", fechafin.ToString("dd/MM/yyyy HH:mm:ss"));

            json.AddField("idtour", idtour.ToJson());
            json.AddField("idusuario", idusuario.ToJson());

            return json;
        }

        public override string ToString()
        {
            return String.Format("Usuariotour [ idusuariotour: {0}, estadousuario: {1} ]", idusuariotour, estadousuariotour);
        }

        #endregion
    }
}
