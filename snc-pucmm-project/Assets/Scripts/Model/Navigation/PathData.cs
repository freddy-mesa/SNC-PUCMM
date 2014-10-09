using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Navigation
{
    public class PathData
    {
        #region Propiedades

        public Node StartNode { get; set; }

        public Node EndNode { get; set; }

        public float DistanceToNeighbor { get; set; }

        public float DistancePathed { get; set; }

        public eDirection Direction { get; set; }
        
        #endregion
    }

    public enum eDirection
    {
        Right,
        Left,
        Straight
    }
}
