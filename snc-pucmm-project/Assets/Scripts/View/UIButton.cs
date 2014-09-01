using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SncPucmm.Controller.GUI;
using SncPucmm.Controller;

namespace SncPucmm.View
{
	public class UIButton : UITouch
    {
        #region Atributos

        public Texture textureBoxOnNormal;
        public Texture textureBoxOnHover;
        
        #endregion

        #region Metdos

        /// <summary>
        /// Start the instance
        /// </summary>
        void Start()
        {
            this.guiTexture.texture = textureBoxOnNormal;
        }

        /// <summary>
        /// Raise a touch event on button
        /// </summary>
        public void OnTouchButton(String buttonName)
        {
            var menu = MenuManager.GetInstance().GetCurrentMenu() as IButton;

            menu.GetButtonList().ForEach(x => {
                if (x.Name.Equals(buttonName))
                {
                    x.OnTouch(null);
                }
            });
        }

        /// <summary>
        /// Raise a touch hover event on button
        /// </summary>
        public void OnTouchHoverButton()
        {
            this.guiTexture.texture = textureBoxOnHover;
        }

        /// <summary>
        /// Raise a touch normal event on button
        /// </summary>
        public void OnTouchNormalButton()
        {
            this.guiTexture.texture = textureBoxOnNormal;
        }

        #endregion
	}
}

