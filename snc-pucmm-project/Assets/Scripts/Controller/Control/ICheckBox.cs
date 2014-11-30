using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.Control
{
    public interface ICheckBox
    {
        /// <summary>
        /// Gets the TreeView
        /// </summary>
        /// <returns>The TreeView Class</returns>
        List<CheckBox> GetCheckBoxList();
    }
}
