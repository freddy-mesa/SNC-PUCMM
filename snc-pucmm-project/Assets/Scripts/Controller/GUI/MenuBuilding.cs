using SncPucmm.Controller.Navigation;
using SncPucmm.Controller;
using SncPucmm.Controller.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SncPucmm.View;
using SncPucmm.Model;
using UnityEngine;

namespace SncPucmm.Controller.GUI
{
    public class MenuBuilding : IMenu, IButton
    {
        #region Atributos

        private string name;
        private ModelNode modelNode;

        public List<Button> buttonList;

        #endregion

        #region Constructor

        public MenuBuilding(string name, ModelNode modelNode)
        {
            this.name = name;
            this.modelNode = modelNode;
            Initializer();
        }

        #endregion

        #region Metodos

        private void Initializer()
        {
            buttonList = new List<Button>();

            var ExitButton = new Button("ButtonExit");
            ExitButton.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
            buttonList.Add(ExitButton);

            var NavigationButton = new Button("ButtonNavigation");
            NavigationButton.OnTouchEvent += new OnTouchEventHandler(OnTouchNavigationButton);
            buttonList.Add(NavigationButton);

            var ShowInsideButton = new Button("ButtonShowInside");
            ShowInsideButton.OnTouchEvent += new OnTouchEventHandler(OnTouchShowInsideButton);
            buttonList.Add(ShowInsideButton);

            if (!this.modelNode.isBuilding)
            {
                var boxCollider = UIUtils.FindGUI("MenuBuilding/ButtonShowInside").GetComponent<BoxCollider>();
                GameObject.Destroy(boxCollider);
            }
        }

        public void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            UIUtils.ActivateCameraLabels(true);
            Exit();
        }

        public void OnTouchNavigationButton(object sender, TouchEventArgs e)
        {
            NavigationController controller = ModelPoolManager.GetInstance().GetValue("navigationCtrl") as NavigationController;
            controller.StartNavigation(this.modelNode.name);
        }

        public void OnTouchShowInsideButton(object sender, TouchEventArgs e)
        {
            //disable el box collider del edificio
            UIUtils.Find("/PUCMM/Model3D/" + modelNode.abreviacion).GetComponent<BoxCollider>().enabled = false;

            MenuManager.GetInstance().AddMenu(new MenuInsideBuilding("MenuInsideBuilding", modelNode));
            State.ChangeState(eState.Navigation);
        }

        private void Exit()
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
            State.ChangeState(eState.Navigation);
        }

        public string GetMenuName()
        {
            return name;
        }

        public List<Button> GetButtonList()
        {
            return buttonList;
        }

        #endregion

        #region Destructor

        ~MenuBuilding()
        {
            this.buttonList = null;
            this.modelNode = null;
            this.name = null;
        }

        #endregion
    }
}
