using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Navigation
{
    public class Node
    {
        #region Propiedades

        public string Name { get; set; }
        public Boolean Visited { get; set; }
        public List<Neighbor> Neighbors { get; set; }
        public int Distance { get; set; }
        public Path Path { get; set; }
        
        #endregion

        #region Constructor

        public Node()
        {
            Neighbors = new List<Neighbor>();
        }
        
        #endregion
    }
}
