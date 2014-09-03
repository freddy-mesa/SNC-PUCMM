using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Navigation
{
    public class Path
    {
        #region Propiedades

        public String Nodes { get; set; }
        public int Distance { get; set; }

        #endregion

        #region Constructores

        public Path() { }

        public Path(String nodes, int distance)
        {
            this.Nodes = nodes;
            this.Distance = distance;
        }

        #endregion
    }
}
