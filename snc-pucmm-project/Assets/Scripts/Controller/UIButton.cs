using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SncPucmm.Utils;

namespace SncPucmm.Controller
{
	public class UIButton : UIManager
	{
		public Texture2D textureOnNormalEvent;
		public Texture2D textureOnHoverEvent;

		/// <summary>
		/// Start the instance
		/// </summary>
        void Start()
		{
			this.guiTexture.texture = textureOnNormalEvent;
		}

        /// <summary>
        /// Raise a touch event on button
        /// </summary>
		public override void OnTouchButton ()
		{
			if(this.name.Equals("ButttonExit"))
            {
                var menuManager = UIMenuManager.GetInstance();
                UIMenuManager.GetInstance().RemoveCurrentMenu();
                
                var modelPool = ModelPool.GetInstance();
                modelPool.Remove("building");

                if (UIMenuManager.GetInstance().NoMenuLeft())
                {
                    State.ChangeState(eState.Exploring);
                }
			}

			if(this.name.Equals("ButtonMainMenu"))
            {
				var sidebarGameObject = GameObject.Find("Sidebar");
				float position;

				if(sidebarGameObject.transform.localPosition.x > 17.70){
					position = -0.75f;
                    State.ChangeState(eState.Exploring);
				} else {
					position = 0.75f;
                    State.ChangeState(eState.GUISystem);
				}

				AnimationManager.Move(sidebarGameObject, new Dictionary<string,object> {
					{"x", position},{"easeType", "easeInOutExpo"},{"loopType", "none"},{"delay", .05}
				});
			}
		}

        /// <summary>
        /// Raise a touch hover event on button
        /// </summary>
		public override void OnTouchHoverButton ()
		{
			this.guiTexture.texture = textureOnHoverEvent;
		}

        /// <summary>
        /// Raise a touch normal event on button
        /// </summary>
		public override void OnTouchNormalButton ()
		{
			this.guiTexture.texture = textureOnNormalEvent;
		}
	}
}

