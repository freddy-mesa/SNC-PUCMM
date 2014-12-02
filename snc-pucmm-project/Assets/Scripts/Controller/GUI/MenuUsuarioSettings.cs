using SncPucmm.Controller.Control;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.GUI
{
    class MenuUsuarioSettings : IMenu, ICheckBox, IButton
    {
        #region Atributos

        string name;
        List<Button> buttonList;
        List<CheckBox> checkboxList;

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
            
            var buttonSave = new Button("ButtonSave");
            buttonSave.OnTouchEvent += new OnTouchEventHandler(OnToucnSaveButton);
            buttonList.Add(buttonSave);

            var buttonExit = new Button("ButtonExit");
            buttonExit.OnTouchEvent += new OnTouchEventHandler(OnToucnExitButton);
            buttonList.Add(buttonExit);

            checkboxList = new List<CheckBox>();

            var chkPreferencia1 = new CheckBox("CheckBoxPreferencia1");
            chkPreferencia1.OnChangeEvent += new OnChangeEventHandler(OnChangeCheckBox);
            checkboxList.Add(chkPreferencia1);

            var chkPreferencia2 = new CheckBox("CheckBoxPreferencia2");
            chkPreferencia2.OnChangeEvent += new OnChangeEventHandler(OnChangeCheckBox);
            checkboxList.Add(chkPreferencia2);
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

        private void OnToucnPendingFollowingButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().AddMenu(new MenuReceiveFollowingRequest("MenuReceiveFollowingRequest"));
            GetPendingFriendRequest();
        }

        private void OnToucnSendFollowingButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().AddMenu(new MenuSendFollowingRequest("MenuSendFollowingRequest"));
        }

        private void GetPendingFriendRequest()
        {
            //UINotification.StartNotificationLoading = true;
            var idUser = Convert.ToInt64("10152587482388668");
            WebService.Instance.GetUserFriendsPendingToFollow(idUser);
        }

        private void OnToucnSaveButton(object sender, TouchEventArgs e)
        {
            throw new NotImplementedException();
        }

        #region Implemented

        public string GetMenuName()
        {
            return this.name;
        }

        public void Update()
        {
            
        }

        public List<CheckBox> GetCheckBoxList()
        {
            return this.checkboxList;
        }

        public List<Button> GetButtonList()
        {
            return this.buttonList;
        }

        #endregion

        #endregion
    }
}
