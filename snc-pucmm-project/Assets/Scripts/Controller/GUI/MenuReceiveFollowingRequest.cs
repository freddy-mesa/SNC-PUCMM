using SncPucmm.Controller.Control;
using SncPucmm.Controller.Facebook;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller.GUI
{
    class MenuReceiveFollowingRequest : IMenu, IButton
    {
        #region Atributos

        string name;
        List<Button> buttonList;

        #endregion

        #region Constructor

        public MenuReceiveFollowingRequest(string name)
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

            var buttonAccept = new Button("ButtonAccept");
            buttonAccept.OnTouchEvent += new OnTouchEventHandler(OnTouchAcceptButton);
            buttonList.Add(buttonAccept);

            var buttonDecline = new Button("ButtonDecline");
            buttonDecline.OnTouchEvent += new OnTouchEventHandler(OnTouchDeclineButton);
            buttonList.Add(buttonDecline);

            UIUtils.DestroyChilds(name + "/ScrollView", true);
        }

        private void OnTouchDeclineButton(object sender, TouchEventArgs e)
        {
            var obj = (sender as Button).ObjectTag;

            //var idFollowed = Convert.ToInt64(FB.UserId);
            var idFollowed = Convert.ToInt64("10152587482388668");
            var idFollower = Convert.ToInt64(obj.GetType().GetProperty("follower").GetValue(obj, null));

            WebService.Instance.SendFollowingAcceptance(idFollowed, idFollower, "denied");
        }

        private void OnTouchAcceptButton(object sender, TouchEventArgs e)
        {
            var obj = (sender as Button).ObjectTag;

            //var idFollowed = Convert.ToInt64(FB.UserId);
            var idFollowed = Convert.ToInt64("10152587482388668");
            var idFollower = Convert.ToInt64(obj.GetType().GetProperty("follower").GetValue(obj, null));

            WebService.Instance.SendFollowingAcceptance(idFollowed, idFollower, "accepted");
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
        }

        public void OnTouchButton(object sender, TouchEventArgs e)
        {
            var obj = (sender as Button).ObjectTag;
            var idFollower = Convert.ToInt64(obj.GetType().GetProperty("follower").GetValue(obj, null));
            var nombre = Convert.ToString(obj.GetType().GetProperty("name").GetValue(obj, null));
            //var Texture = (Texture2D)(obj.GetType().GetProperty("texture").GetValue(obj, null));

            var acceptance = UIUtils.FindGUI(name).transform.FindChild("NotificationFollowingAcceptance");
            acceptance.gameObject.SetActive(true);

            //acceptance.FindChild("Image").GetComponent<UITexture>().mainTexture = Texture;
            acceptance.FindChild("Label").GetComponent<UILabel>().text = nombre;

            buttonList[1].ObjectTag = new { follower = idFollower };
            buttonList[2].ObjectTag = new { follower = idFollower };
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

        #endregion

        #endregion
    }
}
