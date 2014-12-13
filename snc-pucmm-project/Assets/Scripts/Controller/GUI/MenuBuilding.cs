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

            Update();
        }

        public void OnTouchExitButton(object sender, TouchEventArgs e)
        {
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
            var boxColliderList = UIUtils.Find("/PUCMM/Model3D/" + modelNode.abreviacion).GetComponents<BoxCollider>();
            foreach (var boxCollider in boxColliderList)
            {
                boxCollider.enabled = false;
            }

            MenuManager.GetInstance().AddMenu(new MenuInsideBuilding("MenuInsideBuilding", modelNode));
            State.ChangeState(eState.MenuInsideBuilding);
        }

        private void Exit()
        {
            MenuManager.GetInstance().RemoveCurrentMenu();

            if (this.modelNode.isBuilding)
            {
                UIUtils.ActivateCameraLabels(true);
                State.ChangeState(eState.Exploring);
            }
        }

        #region Implemented Methods
        
        public string GetMenuName()
        {
            return name;
        }

        public List<Button> GetButtonList()
        {
            return buttonList;
        }

        public void Update()
        {
            var label = UIUtils.FindGUI("MenuBuilding/LabelBuildingName");
            var lblBuildingName = label.GetComponent<UILabel>();
            lblBuildingName.text = modelNode.name;

            if (!this.modelNode.isBuilding)
            {
                UIUtils.FindGUI("MenuBuilding/ButtonShowInside").SetActive(false);
            }
            else
            {
                var menu = UIUtils.FindGUI("MenuBuilding").transform;
                menu.FindChild("ButtonShowInside").gameObject.SetActive(true);

                var boxColliderList = UIUtils.Find("/PUCMM/Model3D/" + modelNode.abreviacion).GetComponents<BoxCollider>();
                foreach (var boxCollider in boxColliderList)
                {
                    boxCollider.enabled = true;
                }
            }

            State.ChangeState(eState.MenuBuilding);
        }

        #endregion

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
