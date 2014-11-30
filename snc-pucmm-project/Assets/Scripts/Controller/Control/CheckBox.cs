using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.Control
{
    public class CheckBox : Control
    {
        #region Atributos

        public bool active;

        #endregion

        #region COnstructor

        public CheckBox(string Nombre) : base(Nombre) { active = false; }

        #endregion

    }
}
