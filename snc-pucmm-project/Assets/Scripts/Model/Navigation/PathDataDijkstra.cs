using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Navigation
{
    public class PathDataDijkstra : IEquatable<PathDataDijkstra>
    {
        #region Propiedades

        public NodeDijkstra StartNode { get; set; }

        public NodeDijkstra EndNode { get; set; }

        public float DistanceToNeighbor { get; set; }

        public float DistancePathed { get; set; }

        public bool IsInsideBuilding { get; set; }

        public int PlantaBuilding { get; set; }
        
        #endregion

        #region Metodos

        public bool Equals(PathDataDijkstra other)
        {
            if (other.StartNode.Name == this.StartNode.Name && other.EndNode.Name == this.EndNode.Name)
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
