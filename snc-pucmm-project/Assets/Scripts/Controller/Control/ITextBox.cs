using SncPucmm.Controller.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.Control
{
    public interface ITextBox
    {
        /// <summary>
        /// Gets list of textBox
        /// </summary>
        /// <returns></returns>
        List<TextBox> GetTextBoxList();
    }
}
