using System.Collections.Generic;

namespace SncPucmm.Model.Domain
{
    class CoordenadaNodo : IJson
    {
        #region Atributos

        public int? idCoordenadaNodo;
        public Nodo nodo;
        public float? latitud;
        public float? longitud;

        #endregion

        #region Constructor

        public CoordenadaNodo()
        {

        }

        public CoordenadaNodo(JSONObject json)
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
                        this.idCoordenadaNodo = System.Convert.ToInt32(json.list[i].n);
                    else if (key == "nodo")
                        this.nodo = new Nodo(json.list[i]);
                    else if (key == "latitud")
                        this.latitud = json.list[i].n;
                    else
                        this.longitud = json.list[i].n;
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            if (idCoordenadaNodo.HasValue)
                json.AddField("id", idCoordenadaNodo.Value);
            if (nodo != null)
                json.AddField("nodo", nodo.ToJson());
            if (longitud.HasValue)
                json.AddField("longitud", longitud.Value);
            if (latitud.HasValue)
                json.AddField("latitud", latitud.Value);

            return json;
        }

        public override string ToString()
        {
            return string.Format("CoodernadaNodo [Nodo: {0}, Longitud(X): {1}, Latitud(Y): {2}]",
                nodo != null ? nodo.nombre : string.Empty, 
                longitud.HasValue ? longitud.Value.ToString() : string.Empty, 
                latitud.HasValue ? latitud.Value.ToString() : string.Empty
            );
        }
        
        #endregion
    }
}
