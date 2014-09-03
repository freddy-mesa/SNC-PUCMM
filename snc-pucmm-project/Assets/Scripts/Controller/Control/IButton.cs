using SncPucmm.Controller.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.Control
{
    interface IButton
    {
        /// <summary>
        /// Gets list of buttons
        /// </summary>
        /// <returns>List of buttons</returns>
        List<Button> GetButtonList();
    }
}
