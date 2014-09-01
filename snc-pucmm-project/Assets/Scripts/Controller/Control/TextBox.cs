using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.Control
{
    public class TextBox : Control
    {
        #region Atributos

        public Label label;
        
        #endregion

        #region Constructor

        public TextBox(string name, string labelName) : base(name) 
        {
            label = new Label(labelName);
            label.Text = String.Empty;
        }
        
        #endregion

        #region Metodos

        #endregion
    }
}
