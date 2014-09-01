using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace SncPucmm.Controller.Control
{
    public class Label
    {
        #region Atributos

        string _name;
        
        #endregion

        #region Propiedades

        public String Name { get { return _name; } set { _name = value; } }
        
        public String Text { get; set; }

        #endregion

        #region Contructor

        public Label(string name) 
        {
            this._name = name;
        }
        
        #endregion
    }
}
