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

        #region Metodos

        public new bool Equals(object obj)
        {
            PathData path = (PathData) obj;

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

    public enum eDirection
    {
        Straight,
        Right,
        Left,
        SlightRight,
        SlightLeft
    }
}
