using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller
{
    #region Argumentos de Eventos 
    
    public class InitEventArgs : EventArgs
    {
        public object Mensaje;
    }

    public class TouchEventArgs : EventArgs 
    {
        public object Mensaje;
    }

    public class ChangeEventArgs : EventArgs
    {
        public object Mensaje;
    }

    public class CloseEventArgs : EventArgs
    {
        public object Mensaje;
    }

    #endregion

    #region Delegados

    public delegate void OnInitEventHandler(object sender, InitEventArgs e);
    public delegate void OnTouchEventHandler(object sender, TouchEventArgs e);
    public delegate void OnChangeEventHandler(object sender, ChangeEventArgs e);
    public delegate void OnCloseEventHandler(object sender, CloseEventArgs e);

    #endregion
}
