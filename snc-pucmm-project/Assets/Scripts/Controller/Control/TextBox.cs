using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.Control
{
    public class TextBox : Control
    {
        #region Atributos

        public string Text { get; set; }
        
        #endregion

        #region Constructor

        public TextBox(string name) : base(name) 
        {
            Text = String.Empty;
        }
        
        #endregion

        #region Metodos

        #endregion
    }
}
