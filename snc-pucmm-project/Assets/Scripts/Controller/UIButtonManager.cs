using System;
using UnityEngine;
using SncPucmm.Utils;

namespace SncPucmm.Controller
{
	public class UIButtonManager : UIManager
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

