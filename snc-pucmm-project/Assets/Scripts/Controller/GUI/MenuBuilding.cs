using SncPucmm.Controller.Navigation;
using SncPucmm.Controller;
using SncPucmm.Controller.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SncPucmm.View;
using SncPucmm.Model;

namespace SncPucmm.Controller.GUI
{
    public class MenuBuilding : IMenu, IButton
    {
        #region Atributos

        private string name;
        private ModelNode location;

        public List<Button> buttonList;

        #endregion

        #region Constructor

        public MenuBuilding(string name, ModelNode location)
        {
            this.name = name;
            this.location = location;
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

            var AditionalInformationButton = new Button("ButtonAditionalInformation");
            AditionalInformationButton.OnTouchEvent += new OnTouchEventHandler(OnTouchAditionalInformationButton);
            buttonList.Add(AditionalInformationButton);
        }

        public void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            UIUtils.ActivateCameraLabels(true);
            Exit();
        }

        public void OnTouchNavigationButton(object sender, TouchEventArgs e)
        {
            NavigationController controller = ModelPoolManager.GetInstance().GetValue("navigationCtrl") as NavigationController;
            controller.StartNavigation(this.location.name);
        }

        public void OnTouchPhotosButton(object sender, TouchEventArgs e)
        {

        }

        public void OnTouchDescriptionButton(object sender, TouchEventArgs e)
        {

        }

        public void OnTouchAditionalInformationButton(object sender, TouchEventArgs e)
        {

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
            this.location = null;
            this.name = null;
        }

        #endregion
    }
}
