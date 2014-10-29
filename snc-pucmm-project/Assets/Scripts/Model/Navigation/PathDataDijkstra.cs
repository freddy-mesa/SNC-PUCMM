using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Navigation
{
    public class PathDataDijkstra
    {
        #region Propiedades

        public NodeDijkstra StartNode { get; set; }

        public NodeDijkstra EndNode { get; set; }

        public float DistanceToNeighbor { get; set; }

        public float DistancePathed { get; set; }
        
        #endregion

        #region Metodos

        public new bool Equals(object obj)
        {
            PathDataDijkstra path = (PathDataDijkstra)obj;

            if (path.StartNode.Name == this.StartNode.Name && path.EndNode.Name == this.EndNode.Name)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return StartNode.Name + " to " + EndNode.Name; 
        }

        #endregion
    }
}
