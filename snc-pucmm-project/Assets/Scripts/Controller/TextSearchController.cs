using System;
using System.Collections.Generic;
using UnityEngine;
using SncPucmm.Utils;
using SncPucmm.Database;
using SncPucmm.Controller.Utils;

namespace SncPucmm.Controller
{
    public class TextSearchController : TouchManager
    {
        #region Atributos

        public GUIText textSearch;
        KeyboardManager keyboardManager;

        #endregion      

        #region Metodos
        
        new void Update()
        {
            if (KeyboardManager.IsTouchKeyboardOpen)
            {
                textSearch.text = keyboardManager.GetText();

                if (keyboardManager.Keyboard.done)
                {
                    keyboardManager.Close();
                    keyboardManager = null;
                }
            }                
                
            base.Update();
        }

        /// <summary>
        /// Initialize the Keyboard Manager
        /// </summary>
        void InitializeKeyboard() 
        {
            keyboardManager = new KeyboardManager();
            keyboardManager.Open(textSearch.text,"Buscar Edificio",false);
        }

        #endregion

    }
}
