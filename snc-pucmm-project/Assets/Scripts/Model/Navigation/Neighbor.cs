using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Serialization;

namespace SncPucmm.Model.Navigation
{
    public class Neighbor
    {
        #region Propiedades

        public Node Node { get; set; }

        public float Distance { get; set; }

        #endregion

        #region Constructor

        public Neighbor() { }
        public Neighbor(Neighbor neighbor) : this(neighbor.Node, neighbor.Distance) { }
        public Neighbor(Node node, float distance) 
        {
            this.Node = node;
            this.Distance = distance;
        }
        
        #endregion
    }
}
