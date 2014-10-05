using SncPucmm.Controller.GUI;
using SncPucmm.Database;
using SncPucmm.Model;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.Control
{
    public class ScrollTreeView : Control
    {

        #region Propiedades

        public int ButtonCount { get; set; }

        #endregion

        #region Contructor

        public ScrollTreeView(string name) : base(name) { }

        #endregion

    }
}
