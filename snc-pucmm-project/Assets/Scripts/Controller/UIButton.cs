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

		void Start()
		{
			this.guiTexture.texture = textureOnNormalEvent;
		}

		public override void OnTouchButton ()
		{
			if(this.name.Equals("ButttonExit")){
				UIMenuManager.GetInstance().RemoveCurrentMenu();

				if(UIMenuManager.GetInstance().NoMenuLeft())
					State.ChangeState(eState.Exploring);
			}

			if(this.name.Equals("ButtonMainMenu")){
				var sidebarGameObject = GameObject.Find("Sidebar");
				float position;

				if(sidebarGameObject.transform.localPosition.x > 17.70){
					position = -0.75f;
				} else {
					position = 0.75f;
				}
				AnimationManager.Move(sidebarGameObject, new Dictionary<string,object>{
					{"x", position},{"easeType", "easeInOutExpo"},{"loopType", "none"},{"delay", .1}
				});
			}
		}

		public override void OnTouchHoverButton ()
		{
			this.guiTexture.texture = textureOnHoverEvent;
		}

		public override void OnTouchNormalButton ()
		{
			this.guiTexture.texture = textureOnNormalEvent;
		}
	}
}

