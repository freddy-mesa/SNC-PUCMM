using System;
using System.Collections.Generic;

using UnityEngine;
using SncPucmm.Database;
using SncPucmm.Controller;
using SncPucmm.Controller.Control;

namespace SncPucmm.View
{
    public class UITextBoxControl : UITouch
    {
        #region Atributos

        UILabel Label;
        UIKeyboard keyboardManager;
        TextBox currentTextBox;

        public static bool isEqualToPreviousText;

        #endregion

        #region Metodos

        void Start()
        {
            Label = this.transform.FindChild("Label").GetComponent<UILabel>();
        }
        
        new void Update()
        {
            base.Update();

            if (UIKeyboard.IsTouchKeyboardOpen)
            {
                if (!IsPreviousTextEquals())
                {
                    Label.text = keyboardManager.GetText();
                    currentTextBox.OnChange(Label.text);
                }

                if (keyboardManager.Keyboard.done)
                {
                    keyboardManager.Close();
                    keyboardManager = null;
                }
            }          
        }

        /// <summary>
        /// Initialize the Keyboard Manager
        /// </summary>
        void InitializeKeyboard() 
        {
            keyboardManager = new UIKeyboard();
            keyboardManager.Open(Label.text, "", false);
        }

        public void OnClick() 
        {
            isTapped = true;

            //Buscando el TextBox
            var menu = MenuManager.GetInstance().GetCurrentMenu() as ITextBox;
            currentTextBox = menu.GetTextBoxList().Find(x => x.Name == this.name);

            //Initializando el Keyboard
            InitializeKeyboard();

            //Activando el TreeView
            UIUtils.FindGUI("MenuMain/TreeView").SetActive(true);


            //Quitando los labels de la camara
            UIUtils.ActivateCameraLabels(false);
        }

        bool IsPreviousTextEquals() 
        {
            isEqualToPreviousText = Label.text.Equals(keyboardManager.GetText());
            return isEqualToPreviousText;
        }

        public UIKeyboard GetUIKeyBoard()
        {
            return keyboardManager;
        }

        #endregion

    }
}
