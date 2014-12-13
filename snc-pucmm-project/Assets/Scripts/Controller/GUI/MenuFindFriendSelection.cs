using SncPucmm.Controller;
using SncPucmm.Controller.Control;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.GUI
{
    class MenuFindFriendSelection : IMenu, IButton
    {
        #region Atributos

        string name;
        List<Button> buttonList;
        
        #endregion

        #region Constructor

        public MenuFindFriendSelection(string name)
        {
            this.name = name;
            Initializer();
        }

        #endregion

        #region Metodos

        private void Initializer()
        {
            buttonList = new List<Button>();

            var buttonExit = new Button("ButtonExit");
            buttonExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
            buttonList.Add(buttonExit);

            UIUtils.DestroyChilds(name + "/ScrollView", true);
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
        }

        public void OnTouchButton(object sender, TouchEventArgs e)
        {
            var obj = (sender as Button).ObjectTag;
            MenuManager.GetInstance().AddMenu(new MenuFindFriend("MenuFindFriend", obj));
        }

        #region Implementado

        public string GetMenuName()
        {
            return name;
        }

        public void Update()
        {
            
        }

        public List<Button> GetButtonList()
        {
            return buttonList;
        }

        #endregion

        #endregion        
    
    }
}
