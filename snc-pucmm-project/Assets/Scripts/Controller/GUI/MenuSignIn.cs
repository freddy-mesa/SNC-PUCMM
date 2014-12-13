using SncPucmm.Controller.Control;
using SncPucmm.View;
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

            var buttonSignInFacebook = new Button("ButtonSignIn");
            buttonSignInFacebook.OnTouchEvent += new OnTouchEventHandler(OnTouchSignInFacebookButton);
            buttonList.Add(buttonSignInFacebook);

            var buttonExit = new Button("ButtonExit");
            buttonExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
            buttonList.Add(buttonExit);
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
        }

        private void OnTouchSignInFacebookButton(object sender, TouchEventArgs e)
        {
            if (FB.IsInitialized)
            {
                FacebookController.Login();
                MenuManager.GetInstance().RemoveCurrentMenu();
            }
            else
            {
                //FacebookController.Init();

                //if (FB.IsInitialized)
                //{
                //    FacebookController.Login();
                //}
                //else
                //{
                //    UINotification.StartNotificationNoInternet = true;
                //}

                UINotification.StartNotificationNoInternet = true;
            }
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
