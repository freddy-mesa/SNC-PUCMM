using SncPucmm.Controller.Control;
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
        bool isAcceptanceFollowing;
        Button selectButton;

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

            UIUtils.DestroyChilds(name + "/ScrollView", true);

            isAcceptanceFollowing = false;
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
        }

        public void OnTouchButton(object sender, TouchEventArgs e)
        {
            selectButton = sender as Button;
            var obj = (sender as Button).ObjectTag;
            isAcceptanceFollowing = true;
            MenuManager.GetInstance().AddMenu(new MenuAcceptanceFollowingRequest("MenuAcceptanceFollowingRequest", obj));
        }

        #region Implementado

        public string GetMenuName()
        {
            return name;
        }

        public void Update()
        {
            if (isAcceptanceFollowing && MenuAcceptanceFollowingRequest.DeletePendingRequest)
            {
                isAcceptanceFollowing = false;
                var index = Convert.ToInt32(selectButton.ObjectTag.GetType().GetProperty("index").GetValue(selectButton.ObjectTag, null));
                buttonList.Remove(selectButton);

                RefreshScrollView(index);
            }
        }

        private void RefreshScrollView(int index)
        {
            var scrollView = UIUtils.FindGUI(name + "/ScrollView").transform;

            int i = 0;
            foreach (Transform item in scrollView)
            {
                if(i++ >= index)
                {
                    item.localPosition = new Vector3(item.localPosition.x, item.localPosition.y - 60f, item.localPosition.z);
                }
            }

            var itemScrollView = scrollView.GetChild(index);
            itemScrollView.parent = null;

            GameObject.Destroy(itemScrollView);
        }

        public List<Button> GetButtonList()
        {
            return buttonList;
        }

        #endregion

        #endregion
    }
}
