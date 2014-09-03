using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Navigation
{
    public class Neighbor
    {
        #region Propiedades

        public Node Node { get; set; }
        public int Distance { get; set; }

        #endregion

        #region Constructor

        public Neighbor() { }
        
        #endregion
    }
}
