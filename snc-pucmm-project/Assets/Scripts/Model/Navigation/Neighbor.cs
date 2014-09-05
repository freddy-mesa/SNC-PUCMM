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
        
        #endregion
    }
}
