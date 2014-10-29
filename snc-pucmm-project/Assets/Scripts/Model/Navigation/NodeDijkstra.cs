using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Serialization;

namespace SncPucmm.Model.Navigation
{
    public class NodeDijkstra
    {
        #region Propiedades

        public int IdNode { get; set; }

        public string Name { get; set; }

        public List<NeighborDijkstra> Neighbors { get; set; }

        public bool Active { get; set; }

        public Boolean Visited { get; set; }
        
        public float DijkstraDistance { get; set; }

        public PathDijkstra DijkstraPath { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }
        
        #endregion

        #region Constructor

        public NodeDijkstra() { }

        public NodeDijkstra(NodeDijkstra node) : this(node.Name, node.Neighbors, node.DijkstraDistance, node.Active) { }

        public NodeDijkstra(string name, List<NeighborDijkstra> neighbors, float distance, bool active)
        {
            this.Name = name;
            this.Neighbors = neighbors;
            this.Active = active;
            this.DijkstraDistance = distance;
        }
        
        #endregion

        #region Metodos

        public override String ToString()
        {
            return Name;
        }

        #endregion
    }
}
