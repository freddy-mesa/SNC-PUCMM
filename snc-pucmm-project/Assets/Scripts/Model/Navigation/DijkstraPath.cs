using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model.Navigation
{
    public class DijkstraPath
    {
        #region Propiedades

        public String Nodes { get; set; }

        #endregion

        #region Constructores

        public DijkstraPath() { }

        public DijkstraPath(String nodes)
        {
            this.Nodes = nodes;
        }

        #endregion
    }
}
