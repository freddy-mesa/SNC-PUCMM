﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Domain
{
    public class UsuarioTour : IJson
    {
        #region Atributos

        public int? usuarioTour;
        public string estadoUsuarioTour;
        public DateTime? fechaInicio;
        public DateTime? fechaFin;
        public Tour tour;
        public Usuario usuario;


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

                    if (key == "usuarioTour")
                        this.usuarioTour = Convert.ToInt32(json.list[i].n);
                    else if (key == "estadoUsuarioTour")
                        this.estadoUsuarioTour = json.list[i].str;
                    else if (key == "fechaInicio")
                        this.fechaInicio = Convert.ToDateTime(json.list[i].str);
                    else if (key == "fechaFin")
                        this.fechaFin = Convert.ToDateTime(json.list[i].str);
                    else if (key == "tour")
                        this.tour = new Tour(json.list[i]);
                    else
                        this.usuario = new Usuario(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();
            
            if(usuarioTour.HasValue)
                json.AddField("usuarioTour", usuarioTour.Value);
            if(estadoUsuarioTour != null)
                json.AddField("estadoUsuarioTour", estadoUsuarioTour);
            if(fechaInicio.HasValue)
                json.AddField("fechaInicio", fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if(fechaFin.HasValue)
                json.AddField("fechaFin", fechaFin.Value.ToString("dd/MM/yyyy HH:mm:ss"));
            if(tour != null)
                json.AddField("tour", tour.ToJson());
            if(usuario != null)
                json.AddField("usuario", usuario.ToJson());

            return json;
        }

        public override string ToString()
        {
            return String.Format("Usuariotour [ usuarioTour: {0}, estadousuario: {1} ]", 
                usuarioTour.HasValue ? usuarioTour.Value.ToString() : string.Empty, estadoUsuarioTour
            );
        }

        #endregion
    }
}