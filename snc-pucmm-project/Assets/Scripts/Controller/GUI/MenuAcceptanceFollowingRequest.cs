using SncPucmm.Controller.Control;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller.GUI
{
    class MenuAcceptanceFollowingRequest : IMenu, IButton
    {
        #region Atributos

        string name;
        List<Button> buttonList;
        long followerFacebookId;
        public static bool DeletePendingRequest;

        #endregion

        #region Constructor

        public MenuAcceptanceFollowingRequest(string name, object followerData)
        {
            this.name = name;
            this.followerFacebookId = Convert.ToInt64(followerData.GetType().GetProperty("follower").GetValue(followerData, null));

            var followerName = Convert.ToString(followerData.GetType().GetProperty("name").GetValue(followerData, null));
            var followerPhoto = (Texture2D) followerData.GetType().GetProperty("texture").GetValue(followerData, null);

            UIUtils.FindGUI(name + "/Image").GetComponent<UITexture>().mainTexture = followerPhoto;
            UIUtils.FindGUI(name + "/Label").GetComponent<UILabel>().text = followerName;

            DeletePendingRequest = false;

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

            var buttonAccept = new Button("ButtonAccept");
            buttonAccept.OnTouchEvent += new OnTouchEventHandler(OnTouchAcceptButton);
            buttonList.Add(buttonAccept);

            var buttonDecline = new Button("ButtonDecline");
            buttonDecline.OnTouchEvent += new OnTouchEventHandler(OnTouchDeclineButton);
            buttonList.Add(buttonDecline);
        }

        private void OnTouchDeclineButton(object sender, TouchEventArgs e)
        {
            WebService.Instance.SendFollowingAcceptance(followerFacebookId, "denied");
            DeletePendingRequest = true;
            Exit();
        }

        private void OnTouchAcceptButton(object sender, TouchEventArgs e)
        {
            WebService.Instance.SendFollowingAcceptance(followerFacebookId, "accepted");
            DeletePendingRequest = true;
            Exit();
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            Exit();
        }

        private void Exit()
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
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
