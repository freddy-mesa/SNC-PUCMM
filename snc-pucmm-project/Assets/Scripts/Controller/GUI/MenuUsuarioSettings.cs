using SncPucmm.Controller.Control;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.GUI
{
    class MenuUsuarioSettings : IMenu, IButton
    {
        #region Atributos

        string name;
        List<Button> buttonList;
        #endregion

        #region Constructor

        public MenuUsuarioSettings(string name)
        {
            this.name = name;
            Initializer();
        }
        
        #endregion

        #region Metodos

        private void Initializer()
        {
            buttonList = new List<Button>();

            var buttonPendingFollowing = new Button("ButtonPendingFollowingRequest");
            buttonPendingFollowing.OnTouchEvent += new OnTouchEventHandler(OnToucnPendingFollowingButton);
            buttonList.Add(buttonPendingFollowing);

            var buttonSendFollowing = new Button("ButtonSendFollowingRequest");
            buttonSendFollowing.OnTouchEvent += new OnTouchEventHandler(OnToucnSendFollowingButton);
            buttonList.Add(buttonSendFollowing);

            var buttonReceiveShareLocation = new Button("ButtonReceiveShareLocationRequest");
            buttonReceiveShareLocation.OnTouchEvent += new OnTouchEventHandler(OnToucnReceiveShareLocationButton);
            buttonList.Add(buttonReceiveShareLocation);

            var buttonSendShareLocation = new Button("ButtonSendShareLocationRequest");
            buttonSendShareLocation.OnTouchEvent += new OnTouchEventHandler(OnToucnSendShareLocationButton);
            buttonList.Add(buttonSendShareLocation);

            var buttonExit = new Button("ButtonExit");
            buttonExit.OnTouchEvent += new OnTouchEventHandler(OnToucnExitButton);
            buttonList.Add(buttonExit);
        }

        private void OnToucnSendShareLocationButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().AddMenu(new MenuShareLocationFriendSelection("MenuShareLocationFriendSelection"));
        }

        private void OnToucnReceiveShareLocationButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().AddMenu(new MenuReceiveShareLocationRequest("MenuReceiveShareLocationRequest"));
            UINotification.StartNotificationLoading = true;
            WebService.Instance.GetFriends();
        }

        private void OnToucnExitButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
        }

        private void OnChangeCheckBox(object sender, ChangeEventArgs e)
        {
            var selectedCheckBox = sender as CheckBox;
            selectedCheckBox.active = !selectedCheckBox.active;
        }

        private void OnToucnSendFollowingButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().AddMenu(new MenuSendFollowingRequest("MenuSendFollowingRequest"));
        }

        private void OnToucnPendingFollowingButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().AddMenu(new MenuReceiveFollowingRequest("MenuReceiveFollowingRequest"));
            UINotification.StartNotificationLoading = true;
            WebService.Instance.GetUserFriendsPendingToFollow();
        }

        #region Implemented

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

        #endregion
    }
}
