using SncPucmm.Controller.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.GUI
{
    class MenuSignIn : IMenu, IButton
    {
        #region Atributos

        string name;
        List<Button> buttonList;
        
        #endregion

        #region Constructor

        public MenuSignIn(string name)
        {
            this.name = name;

            Initializer();
        }
        
        #endregion

        #region Metodos

        void Initializer()
        {
            buttonList = new List<Button>();

            var buttonIniciarSesion = new Button("ButtonSignIn");
            buttonIniciarSesion.OnTouchEvent += new OnTouchEventHandler(OnTouchSingInButton);
            buttonList.Add(buttonIniciarSesion);

            var buttonSignInFacebook = new Button("ButtonSignInFacebook");
            buttonSignInFacebook.OnTouchEvent += new OnTouchEventHandler(OnTouchSignInFacebookButton);
            buttonList.Add(buttonSignInFacebook);

            var buttonExit = new Button("ButtonSignInFacebook");
            buttonExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
            buttonList.Add(buttonExit);
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnTouchSignInFacebookButton(object sender, TouchEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnTouchSingInButton(object sender, TouchEventArgs e)
        {
            throw new NotImplementedException();
        }

        public string GetMenuName()
        {
            return this.name;
        }

        public void Update()
        {
            
        }

        public List<Button> GetButtonList()
        {
            return this.buttonList;
        }
        
        #endregion

        #region Destructor

        ~MenuSignIn()
        {
            this.name = null;
            this.buttonList = null;
        }
        
        #endregion


    }
}
