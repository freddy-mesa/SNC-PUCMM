using SncPucmm.Controller.Control;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller.GUI
{
    class MenuSendFollowingRequest : IMenu, IButton, ICheckBox
    {
        #region Atributos

        string name;
        List<Button> buttonList;
        List<CheckBox> checkBoxList;

        #endregion

        #region Constructor

        public MenuSendFollowingRequest(string name)
        {
            this.name = name;
            Initializer();
        }

        #endregion

        #region Methods

        private void Initializer()
        {
            buttonList = new List<Button>();

            var buttonExit = new Button("ButtonExit");
            buttonExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
            buttonList.Add(buttonExit);

            var buttonSend = new Button("ButtonSend");
            buttonSend.OnTouchEvent += new OnTouchEventHandler(OnTouchSendButton);
            buttonList.Add(buttonSend);

            checkBoxList = new List<CheckBox>();

            UIUtils.DestroyChilds("MenuSendFollowingRequest/ScrollView", true);
            GetUserFriendList();
        }

        private void OnTouchSendButton(object sender, TouchEventArgs e)
        {
           //Seleccionar los CheckBox selecionados para enviar
            List<long> userToSendFollowingRequest  = new List<long>();
            foreach (var checkBox in checkBoxList)
            {
                if (checkBox.active)
                {
                    long idUsuarioFacebook = Convert.ToInt64(checkBox.ObjectTag.GetType().GetProperty("idUsuario").GetValue(checkBox.ObjectTag, null));
                    userToSendFollowingRequest.Add(idUsuarioFacebook);
                }
            }

            if (userToSendFollowingRequest.Count > 0) 
            {
                WebService.Instance.SendFollowingRequest(userToSendFollowingRequest);

                MenuManager.GetInstance().RemoveCurrentMenu();
            }
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
        }

        private void GetUserFriendList()
        {
            UINotification.StartNotificationLoading = true;
            WebService.Instance.GetUserFriendsForFollow();
        }

        public void OnChangeCheckBox(object sender, ChangeEventArgs e)
        {
            var selectedCheckBox = sender as CheckBox;
            selectedCheckBox.active = !selectedCheckBox.active;
        }

        #region Implement

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

        public List<CheckBox> GetCheckBoxList()
        {
            return checkBoxList;
        }

        #endregion

        #endregion
    }
}
