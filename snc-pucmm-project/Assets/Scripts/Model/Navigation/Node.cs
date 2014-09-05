using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Serialization;

namespace SncPucmm.Model.Navigation
{
    public class Node
    {
        #region Propiedades

        public string Name { get; set; }

        public List<Neighbor> Neighbors { get; set; }

        public bool Active { get; set; }

        public Boolean Visited { get; set; }

        public float DijkstraDistance { get; set; }

        public DijkstraPath DijkstraPath { get; set; }
        
        #endregion

        #region Constructor

        public Node() { }

        public Node(Node node) : this(node.Name, node.Neighbors, node.Active) { }

        public Node(string name, List<Neighbor> neighbors, bool active)
        {
            this.Name = name;
            this.Neighbors = neighbors;
            this.Active = active;
        }
        
        #endregion
    }
}
