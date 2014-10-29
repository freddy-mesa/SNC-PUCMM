using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class VideoLlamada : IJson
    {
        #region Atributos

        public int? idVideoLlamada;
        public DateTime? fechaInicio;
        public DateTime? fechaFin;
        public string plataforma;
        public float? longitud;
        public float? latitud;
        public Usuario usuario;        

    
        #endregion

        #region Constructor

        public VideoLlamada()
        {

        }

        public VideoLlamada(JSONObject json)
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

                    if (key == "idVideoLlamada")
                        this.idVideoLlamada = Convert.ToInt32(json.list[i].n);
                    else if (key == "fechaInicio")
                        this.fechaInicio = Convert.ToDateTime(json.list[i].str);
                    else if (key == "fechaFin")
                        this.fechaFin = Convert.ToDateTime(json.list[i].str);
                    else if (key == "plataforma")
                        this.plataforma = json.list[i].str;
                    if (key == "longitud")
                        this.longitud = Convert.ToSingle(json.list[i].n);
                    if (key == "latitud")
                        this.latitud = Convert.ToSingle(json.list[i].n);
                    else
                        this.usuario = new Usuario(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            if(idVideoLlamada.HasValue)
                json.AddField("idVideoLlamada", idVideoLlamada.Value);
            if(fechaInicio.HasValue)
                json.AddField("fechaInicio", fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if(fechaFin.HasValue)
                json.AddField("fechaFin", fechaFin.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if(plataforma != null)
                json.AddField("plataforma", plataforma);
            if(longitud.HasValue)
                json.AddField("longitud", longitud.Value);
            if(latitud.HasValue)
                json.AddField("latitud", latitud.Value);
            if(usuario != null)
                json.AddField("usuario", usuario.ToJson());

            return json;
        }

        public override string ToString()
        {
            return String.Format("Ubicacion [idVideoLlamada: {0}, Usuario: {1}]", 
                idVideoLlamada.HasValue ? idVideoLlamada.Value.ToString() : string.Empty,
                usuario != null ? usuario.usuario : string.Empty
            );
        }

        #endregion
    }
}
