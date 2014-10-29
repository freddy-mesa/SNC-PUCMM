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

        GUIText Text;
        UIKeyboard keyboardManager;
        TextBox currentTextBox;

        public static bool isEqualToPreviousText;

        #endregion

        #region Metodos

        void Start()
        {
            Text = this.transform.FindChild("Text").guiText;
        }
        
        new void Update()
        {
            if (UIKeyboard.IsTouchKeyboardOpen)
            {
                if (!IsPreviousTextEquals()) 
                {
                    Text.text = keyboardManager.GetText();
                    currentTextBox.OnChange(Text.text);
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
            keyboardManager.Open(Text.text, "", false);
        }

        public void OnTouchTextSearchBox(string name) 
        {
            //Buscando el TextBox
            var menu = MenuManager.GetInstance().GetCurrentMenu() as ITextBox;
            currentTextBox = menu.GetTextBoxList().Find(x => x.Name == name);

            //Initializando el Keyboard
            InitializeKeyboard();

            //Activando el ScrollTreeView
            UIUtils.FindGUI("GUIMenuMain/TreeView/ScrollTreeView").SetActive(true);

            //Quitando los labels de la camara
            UIUtils.ActivateCameraLabels(false);
        }

        bool IsPreviousTextEquals() 
        {
            isEqualToPreviousText = Text.text.Equals(keyboardManager.GetText());
            return isEqualToPreviousText;
        }

        public UIKeyboard GetUIKeyBoard()
        {
            return keyboardManager;
        }

        #endregion

    }
}
