using System.Collections.Generic;

namespace SncPucmm.Model.Domain
{
    class Neighbor : IJson
    {
        #region Atributos

        public int? idNeighbor;
        public Nodo nodo;
        public Nodo nodoNeighbor;

        #endregion

        #region Constructor

        public Neighbor()
        {

        }

        public Neighbor(JSONObject json)
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
                        this.idNeighbor = System.Convert.ToInt32(json.list[i].n);
                    else if (key == "nodo")
                        this.nodo = new Nodo (json.list[i]);
                    else 
                        this.nodoNeighbor = new Nodo(json.list[i]);
                }
            }
        }

        public JSONObject ToJson()
        {
            JSONObject json = new JSONObject();

            if (idNeighbor.HasValue)
                json.AddField("id", idNeighbor.Value);
            if (nodo != null)
                json.AddField("nodo", nodo.ToJson());
            if (nodoNeighbor != null)
                json.AddField("nodoNeighbor", nodoNeighbor.ToJson());

            return json;
        }

        public override string ToString()
        {
            return string.Format("Neighbor [Node: {0}, Neighbor: {1}]",
                nodo != null ? nodo.nombre : "",
                nodoNeighbor != null ? nodoNeighbor.nombre : ""
            );
        }

        #endregion
    }
}
