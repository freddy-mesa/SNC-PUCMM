using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SncPucmm.Controller
{
    class KeyboardManager
    {
        #region Atributos
        private static bool _isTouchKeyboardOpen;
        private TouchScreenKeyboard _keyboard;

        #endregion
        
        #region Propiedades

        public static bool IsTouchKeyboardOpen { get { return _isTouchKeyboardOpen; } }

        public TouchScreenKeyboard Keyboard { get { return _keyboard; } }

        #endregion

        #region Constructores

        public KeyboardManager()
        {
        
        }

        #endregion
       
        #region Metodos

        public void Open(string text, string textPlaceHolder, bool isPassword)
        {            
            this._keyboard = TouchScreenKeyboard.Open(
                text, TouchScreenKeyboardType.Default, false, false, isPassword, false, textPlaceHolder
            );
            
            _isTouchKeyboardOpen = true;

            var camera = Camera.main;
            foreach (Transform label in camera.transform)
            {
                label.gameObject.SetActive(false);
            }

            var guiMainMenu = UIMenuController.GetInstance().Find("GUIMainMenu").gameObject;
            var horizontalBar = guiMainMenu.transform.FindChild("HorizontalBar").gameObject;
            var searchBox = horizontalBar.transform.FindChild("SearchBoxMainMenu").gameObject;
            var imgSearchBox = horizontalBar.transform.FindChild("SearchImgMainMenu").gameObject;

            imgSearchBox.SetActive(false);

            //horizontalBar.transform.localScale.Set(
            //    horizontalBar.transform.localScale.x,
            //    0.141f,
            //    horizontalBar.transform.localScale.z
            //);

            //horizontalBar.transform.localPosition.Set(
            //    horizontalBar.transform.localPosition.x,
            //    36.31999f,
            //    horizontalBar.transform.localPosition.z
            //);
        }

        public void Close()
        {
            _keyboard = null;

            _isTouchKeyboardOpen = false;

            var camera = Camera.main;
            foreach (Transform label in camera.transform)
            {
                label.gameObject.SetActive(true);
            }

            var guiMainMenu = UIMenuController.GetInstance().Find("GUIMainMenu").gameObject;
            var horizontalBar = guiMainMenu.transform.FindChild("HorizontalBar").gameObject;
            var searchBox = horizontalBar.transform.FindChild("SearchBoxMainMenu").gameObject;
            var imgSearchBox = horizontalBar.transform.FindChild("SearchImgMainMenu").gameObject;

            imgSearchBox.SetActive(true);

            //horizontalBar.transform.localScale.Set(
            //    horizontalBar.transform.localScale.x,
            //    0.087f,
            //    horizontalBar.transform.localScale.z
            //);

            //horizontalBar.transform.localPosition.Set(
            //    horizontalBar.transform.localPosition.x,
            //    36.347f,
            //    horizontalBar.transform.localPosition.z
            //);
        }

        public String GetText()
        {
            return _keyboard.text;
        }

        #endregion
    }
}