using SncPucmm.Controller.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SncPucmm.Controller
{
    class KeyboardManager : MonoBehaviour
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

            UIUtils.ActivateCameraLabels(false);

            var horizontalBar = UIUtils.FindGUI("GUIMainMenu/HorizontalBar").gameObject;
            var searchBox = horizontalBar.transform.FindChild("SearchBoxMainMenu").gameObject;
            var imgSearchBox = searchBox.transform.FindChild("SearchImgMainMenu").gameObject;

            imgSearchBox.SetActive(false);

            this.StartCoroutine("WaitSeconds", 2);
        }

        public void Close()
        {
            _keyboard = null;

            _isTouchKeyboardOpen = false;

            var horizontalBar = UIUtils.FindGUI("GUIMainMenu/HorizontalBar").gameObject;
            var searchBox = horizontalBar.transform.FindChild("SearchBoxMainMenu").gameObject;
            var imgSearchBox = searchBox.transform.FindChild("SearchImgMainMenu").gameObject;

            imgSearchBox.SetActive(true);

            this.StartCoroutine("WaitSeconds", 2);
        }

        public String GetText()
        {
            return _keyboard.text;
        }

        IEnumerable WaitSeconds(int time)
        {
            yield return new WaitForSeconds(time);
        }

        #endregion
    }
}