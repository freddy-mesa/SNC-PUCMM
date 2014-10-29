using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Serialization;

namespace SncPucmm.Model.Navigation
{
    public class NeighborDijkstra
    {
        #region Propiedades

        public NodeDijkstra Node { get; set; }

        public float Distance { get; set; }

        #endregion

        #region Constructor

        public NeighborDijkstra() { }
        public NeighborDijkstra(NeighborDijkstra neighbor) : this(neighbor.Node, neighbor.Distance) { }
        public NeighborDijkstra(NodeDijkstra node, float distance) 
        {
            this.Node = node;
            this.Distance = distance;
        }
        
        #endregion
    }
}
