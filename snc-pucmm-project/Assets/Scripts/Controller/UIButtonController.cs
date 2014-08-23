using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SncPucmm.Utils;

namespace SncPucmm.Controller
{
	public class UIButtonController : UIController
    {
        #region Atributos

        public Texture2D textureBoxOnNormal;
        public Texture2D textureBoxOnHover;
        
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
        public override void OnTouchButton()
        {
            if (this.name.Equals("ButttonExit"))
            {
                var menuManager = UIMenuController.GetInstance();
                menuManager.RemoveCurrentMenu();

                var modelPool = ModelPoolController.GetInstance();
                modelPool.Remove("building");

                if (UIMenuController.GetInstance().NoMenuLeft())
                {
                    State.ChangeState(eState.Exploring);
                }
            }

            if (this.name.Equals("ButtonMainMenu"))
            {
                var sidebarGameObject = GameObject.Find("Sidebar");
                float position;

                if (sidebarGameObject.transform.localPosition.x > 17.70)
                {
                    position = -0.75f;
                    State.ChangeState(eState.Exploring);
                }
                else
                {
                    position = 0.75f;
                    State.ChangeState(eState.GUISystem);
                }

                AnimationManager.MoveBy(sidebarGameObject, new Dictionary<string, object> {
					{"x", position},{"easeType", iTween.EaseType.easeInOutExpo},{"time", 2}
				});
            }
        }

        /// <summary>
        /// Raise a touch hover event on button
        /// </summary>
        public override void OnTouchHoverButton()
        {
            this.guiTexture.texture = textureBoxOnHover;
        }

        /// <summary>
        /// Raise a touch normal event on button
        /// </summary>
        public override void OnTouchNormalButton()
        {
            this.guiTexture.texture = textureBoxOnNormal;
        }

        #endregion
	}
}

