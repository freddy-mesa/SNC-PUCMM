﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class UsuarioTour : IJson
    {
        #region Atributos

        public int? idUsuarioTour;
        public string estado;
        public DateTime? fechaInicio;
        public DateTime? fechaFin;
        public int? idTour;
        public int? idUsuario;
        public string request;

        #endregion

        #region Constructor

        public UsuarioTour()
        {

        }

        public UsuarioTour(JSONObject json)
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

                    if (key == "id")
                        this.idUsuarioTour = Convert.ToInt32(json.list[i].n);
                    else if (key == "estado")
                        this.estado = json.list[i].str;
                    else if (key == "fechaInicio")
                        this.fechaInicio = Convert.ToDateTime(json.list[i].str);
                    else if (key == "fechaFin")
                        this.fechaFin = Convert.ToDateTime(json.list[i].str);
                    else if (key == "idTour")
                        this.idTour = Convert.ToInt32(json.list[i].n);
                    else if (key == "idUsuario")
                        this.idUsuario = Convert.ToInt32(json.list[i].n);
                    else
                        this.request = json.list[i].str;
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            if (idUsuarioTour.HasValue)
                json.AddField("id", idUsuarioTour.Value);
            if(estado != null)
                json.AddField("estado", estado);
            if(fechaInicio.HasValue)
                json.AddField("fechaInicio", fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if(fechaFin.HasValue)
                json.AddField("fechaFin", fechaFin.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if (idTour.HasValue)
                json.AddField("idTour", idTour.Value);
            if (idUsuario.HasValue)
                json.AddField("idUsuario", idUsuario.Value);
            if (request != null)
                json.AddField("request", request);

            return json;
        }

        public override string ToString()
        {
            return String.Format("Usuariotour [ idUsuarioTour: {0}, estadousuario: {1} ]",
                idUsuarioTour.HasValue ? idUsuarioTour.Value.ToString() : string.Empty, estado
            );
        }

        #endregion
    }
}
