using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Navigation
{
    public class PathDijkstra
    {
        #region Propiedades

        public String Nodes { get; set; }

        #endregion

        #region Constructores

        public PathDijkstra() { }

        public PathDijkstra(String nodes)
        {
            this.Nodes = nodes;
        }

        #endregion
    }
}
