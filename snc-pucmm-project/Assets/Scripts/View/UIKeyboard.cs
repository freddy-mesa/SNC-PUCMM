using SncPucmm.Controller.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SncPucmm.View
{
    public class UIKeyboard
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

        public UIKeyboard()
        {
            //TouchScreenKeyboard.hideInput = true;
        }

        #endregion
       
        #region Metodos

        public void Open(string text, string textPlaceHolder, bool isPassword)
        {            
            this._keyboard = TouchScreenKeyboard.Open(
                text, TouchScreenKeyboardType.Default, false, false, isPassword, false, textPlaceHolder
            );
            
            _isTouchKeyboardOpen = true;

            UIUtils.ActivateCameraLabels(false);

            var imgSearchBox = UIUtils.FindGUI("MenuMain/Bar/SearchBox/SearchIcon").gameObject;
            imgSearchBox.SetActive(false);
        }

        public void Close()
        {
            _keyboard = null;
            _isTouchKeyboardOpen = false;

            var imgSearchBox = UIUtils.FindGUI("MenuMain/Bar/SearchBox/SearchIcon").gameObject;
            imgSearchBox.SetActive(true);
        }

        public String GetText()
        {
            return _keyboard.text;
        }

        #endregion
    }
}