using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.Control
{
    public class Control
    {
        #region Atributo

        private string _name;
        
        #endregion

        #region Propiedad

        public String Name { get { return _name; } }

        public object Tag { get; set; }
        
        #endregion

        #region Constructor

        public Control(string name)
        {
            this._name = name;
        }
        
        #endregion

        #region Eventos

        public event OnInitEventHandler OnInitEvent;
        public event OnTouchEventHandler OnTouchEvent;
        public event OnChangeEventHandler OnChangeEvent;
        public event OnCloseEventHandler OnCloseEvent;

        #endregion

        #region Metodos

        public void OnInit(object mensaje)
        {
            InitEventArgs e = new InitEventArgs();
            e.Mensaje = mensaje;

            if (OnTouchEvent != null)
            {
                OnInitEvent(this, e);
            }
        }

        public void OnTouch(object mensaje)
        {
            TouchEventArgs e = new TouchEventArgs();
            e.Mensaje = mensaje;

            if (OnTouchEvent != null)
            {
                OnTouchEvent(this, e);
            }
        }

        public void OnChange(object mensaje) 
        {
            ChangeEventArgs e = new ChangeEventArgs();
            e.Mensaje = mensaje;

            if (OnTouchEvent != null)
            {
                OnChangeEvent(this, e);
            }
        }

        public void OnClose(object mensaje)
        {
            CloseEventArgs e = new CloseEventArgs();
            e.Mensaje = mensaje;

            if (OnTouchEvent != null)
            {
                OnCloseEvent(this, e);
            }
        }

        #endregion
    }
}
