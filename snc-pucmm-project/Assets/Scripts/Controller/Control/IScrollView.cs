using SncPucmm.Controller.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.Control
{
    public interface IScrollView
    {
        /// <summary>
        /// Gets the TreeView
        /// </summary>
        /// <returns>The TreeView Class</returns>
        ScrollView GetScrollView();
    }
}
