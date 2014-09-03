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
    public class ScrollTreeView : Control, IButton
    {
        #region Atributos

        private List<Button> buttonList;

        #endregion

        #region Contructor

        public ScrollTreeView(string name) : base(name) 
        {
            buttonList = new List<Button>();
        }

        #endregion

        #region Metodos
        
        public List<Button> GetButtonList()
        {
            return buttonList;
        }

        #endregion
    }
}
