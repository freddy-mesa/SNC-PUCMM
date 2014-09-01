﻿using SncPucmm.Controller;
using SncPucmm.Controller.Control;
using SncPucmm.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.GUI
{
    public class GUIMenuBuildingDescriptor : IMenu, IButton
    {
        #region Atributos

        private string name;

        public List<Button> buttonList;

        #endregion

        #region Constructor

        public GUIMenuBuildingDescriptor(string name) 
        {
            this.name = name;
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

            var PhotosButton = new Button("ButtonPhotos");
            PhotosButton.OnTouchEvent += new OnTouchEventHandler(OnTouchPhotosButton);
            buttonList.Add(PhotosButton);

            var DescriptionButton = new Button("ButtonDescription");
            DescriptionButton.OnTouchEvent += new OnTouchEventHandler(OnTouchDescriptionButton);
            buttonList.Add(DescriptionButton);

            var AditionalInformationButton = new Button("ButtonAditionalInformation");
            AditionalInformationButton.OnTouchEvent += new OnTouchEventHandler(OnTouchAditionalInformationButton);
            buttonList.Add(AditionalInformationButton);
        }
		
        public void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            var menuManager = MenuManager.GetInstance();
            menuManager.RemoveCurrentMenu();

            if (MenuManager.GetInstance().NoMenuLeft())
            {
                var modelPool = ModelPoolManager.GetInstance();
                modelPool.Remove("localizacion");

                State.ChangeState(eState.Exploring);
            }
        }

        public void OnTouchNavigationButton(object sender, TouchEventArgs e)
        {

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

        public string GetMenuName()
        {
            return name;
        }

        public List<Button> GetButtonList()
        {
            return buttonList;
        }

        #endregion
    }
}