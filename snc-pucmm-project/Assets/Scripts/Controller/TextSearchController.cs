using System;
using System.Collections.Generic;
using UnityEngine;
using SncPucmm.Utils;

namespace SncPucmm.Controller
{
    class TextSearchController : TouchManager
    {
        #region Atributos

        public GUIText textSearch;
        public KeyboardManager keyboardManager;

        #endregion   

        #region Propiedades
        
        #endregion        

        #region Metodos
        
        void Update()
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
            else 
            {
                base.Update();
            }
        }

        void InitializeKeyboard() 
        {
            keyboardManager = new KeyboardManager();
            keyboardManager.Open(textSearch.text,"Buscar Edificio",false);
        }

        #endregion

    }
}
