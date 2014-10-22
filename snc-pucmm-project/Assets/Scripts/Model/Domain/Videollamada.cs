using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Videollamada
    {
        #region Atributos

        public int idvideollamada;
        public DateTime fechainicio;
        public DateTime fechafin;
        public String plataforma;
        public float longitud;
        public float latitud;
        public Usuario idusuario;        

    
        #endregion

        #region Constructor

        public Videollamada()
        {

        }

        public Videollamada(JSONObject json)
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

                if (key == "idvideollamada")
                {
                    this.idvideollamada = Convert.ToInt32(json.list[i].n);
                }
                else if (key == "fechainicio")
                {
                    this.fechainicio = Convert.ToDateTime(json.list[i].str);
                }
                else if (key == "fechafin")
                {
                    this.fechafin = Convert.ToDateTime(json.list[i].str);
                }
                else if (key == "plataforma")
                {
                    this.plataforma = json.list[i].str;
                }
                if (key == "longitud")
                {
                    this.longitud = Convert.ToSingle(json.list[i].n);
                }
                if (key == "latitud")
                {
                    this.latitud = Convert.ToSingle(json.list[i].n);
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
            json.AddField("idvideollamada", idvideollamada);
            json.AddField("fechainicio", fechainicio.ToString("dd/MM/yyyy HH:mm:ss"));
            json.AddField("fechafin", fechafin.ToString("dd/MM/yyyy HH:mm:ss"));
            json.AddField("plataforma", plataforma);
            json.AddField("longitud", longitud);
            json.AddField("latitud", latitud);
            json.AddField("idusuario", idusuario.ToJson());

            return json;
        }

        public override string ToString()
        {
            return String.Format("Ubicacion [ idvideollamada: {0} ]", idvideollamada);
        }

        #endregion
    }
}
