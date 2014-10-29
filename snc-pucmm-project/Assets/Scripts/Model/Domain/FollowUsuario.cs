using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class FollowUsuario : IJson
    {
        #region Atributos

        public int? idFollow;
        public string estadoSolicitud;
        public DateTime? fechaRegistroSolicitud;
        public DateTime? fechaRespuestaSolicitud;
        public Usuario usuarioFollower;
        public Usuario usuarioFollowed;

        #endregion

        #region Constructor

        public FollowUsuario()
        {

        }

        public FollowUsuario(JSONObject json)
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

                    if (key == "idFollow")
                        this.idFollow = Convert.ToInt32(json.list[i].n);
                    else if (key == "estadoSolicitud")
                        this.estadoSolicitud = json.list[i].str;
                    else if (key == "fechaRegistroSolicitud")
                        this.fechaRegistroSolicitud = Convert.ToDateTime(json.list[i].str);
                    else if (key == "fechaRespuestaSolicitud")
                        this.fechaRespuestaSolicitud = Convert.ToDateTime(json.list[i].str);
                    else if (key == "usuarioFollower")
                        this.usuarioFollower = new Usuario(json.list[i]);
                    else
                        this.usuarioFollowed = new Usuario(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            if(idFollow.HasValue)
                json.AddField("idFollow", idFollow.Value);
            if (estadoSolicitud != null)
                json.AddField("estadoSolicitud", estadoSolicitud);
            if(fechaRegistroSolicitud.HasValue)
                json.AddField("fechaRegistroSolicitud", fechaRegistroSolicitud.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if(fechaRespuestaSolicitud.HasValue)
                json.AddField("fechaRespuestaSolicitud", fechaRespuestaSolicitud.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if (usuarioFollower != null)
                json.AddField("usuarioFollower", usuarioFollower.ToJson());
            if (usuarioFollowed != null)
                json.AddField("usuarioFollowed", usuarioFollowed.ToJson());

            return json;
        }

        public override string ToString()
        {
            return String.Format("FollowUsuario [idFollow: {0}, estadoSolicitud: {1}]", 
                idFollow.HasValue ? idFollow.Value.ToString() : string.Empty, estadoSolicitud
            );
        }

        #endregion
    }
}
