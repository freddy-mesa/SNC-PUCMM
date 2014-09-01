using System;
using System.Collections.Generic;

using UnityEngine;
using SncPucmm.Database;
using SncPucmm.Controller;
using SncPucmm.Controller.Control;

namespace SncPucmm.View
{
    public class UITextBox : UITouch
    {
        #region Atributos

        public GUIText textSearch;
        UIKeyboard keyboardManager;
        TextBox currentTextBox;

        public static bool isEqualToPreviousText;

        #endregion

        #region Metodos
        
        new void Update()
        {
            if (UIKeyboard.IsTouchKeyboardOpen)
            {
                if (!IsPreviousTextEquals()) 
                {
                    textSearch.text = keyboardManager.GetText();
                    currentTextBox.OnChange(textSearch.text);
                }

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
            keyboardManager = new UIKeyboard();
            keyboardManager.Open(textSearch.text,"Buscar Edificio",false);
        }

        void OnTouchTextSearchBox(string name) 
        {
            var menu = MenuManager.GetInstance().GetCurrentMenu() as ITextBox;
            currentTextBox = menu.GetTextBoxList().Find(x => x.Name == name);
            InitializeKeyboard();
        }

        bool IsPreviousTextEquals() 
        {
            isEqualToPreviousText = textSearch.text.Equals(keyboardManager.GetText());
            return isEqualToPreviousText;
        }

        #endregion

    }
}
