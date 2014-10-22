using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class Followusuario
    {
        #region Atributos

        public int idfollow;
        public String estadosolicitud;
        public DateTime fecharegistrosolicitud;
        public DateTime fecharespuestasolicitud;
        public Usuario idusuariofollower;
        public Usuario idusuariofollowed;

        #endregion

        #region Constructor

        public Followusuario()
        {

        }

        public Followusuario(JSONObject json)
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

                if (key == "idfollow")
                {
                    this.idfollow = Convert.ToInt32(json.list[i].n);
                }
                else if (key == "estadosolicitud")
                {
                    this.estadosolicitud = json.list[i].str;
                }
                else if (key == "fecharegistrosolicitud")
                {
                    this.fecharegistrosolicitud = Convert.ToDateTime(json.list[i].str);
                }
                else if (key == "fecharespuestasolicitud")
                {
                    this.fecharespuestasolicitud = Convert.ToDateTime(json.list[i].str);
                }
                else if (key == "idusuariofollower")
                {
                    this.idusuariofollower = new Usuario(json.list[i]);
                }
                else
                {
                    this.idusuariofollowed = new Usuario(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            json.AddField("idfollow", idfollow);
            json.AddField("estadosolicitud", estadosolicitud);
            json.AddField("fecharegistrosolicitud", fecharegistrosolicitud.ToString("dd/MM/yyyy HH:mm:ss"));
            json.AddField("fecharespuestasolicitud", fecharespuestasolicitud.ToString("dd/MM/yyyy HH:mm:ss"));

            json.AddField("idusuariofollower", idusuariofollower.ToJson());
            json.AddField("idusuariofollowed", idusuariofollowed.ToJson());

            return json;
        }

        public override string ToString()
        {
            return String.Format("FollowUsuario [ idFollow: {0}, estadoSolicitud: {1} ]", idfollow,estadosolicitud);
        }

        #endregion
    }
}
