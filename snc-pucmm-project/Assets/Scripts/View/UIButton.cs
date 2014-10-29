using UnityEngine;
using System.Collections.Generic;
using SncPucmm.Controller;
using SncPucmm.Controller.Control;
using System;
using System.Text;

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
		public void OnTouchButton(string buttonName)
		{
			OnTouchNormalButton();

			var menu = MenuManager.GetInstance().GetCurrentMenu() as IButton;

			menu.GetButtonList().ForEach(button =>
			{
				if (button.Name.Equals(buttonName))
				{
					button.OnTouch(null);
				}
			});
		}

		public void OnTouchButton(string buttonName, object selectedModelObjectTour)
		{
			OnTouchNormalButton();
	
			var menu = MenuManager.GetInstance().GetCurrentMenu() as IButton;

			menu.GetButtonList().ForEach(button =>
			{
				if (button.Name.Equals(buttonName))
				{
					button.OnTouch(selectedModelObjectTour);
				}
			});
		}

		/// <summary>
		/// Raise a touch hover event on button
		/// </summary>
		public void OnTouchHoverButton()
		{
			if (this.guiTexture.texture)
			{
				this.guiTexture.texture = textureBoxOnHover;
			}
		}

		/// <summary>
		/// Raise a touch normal event on button
		/// </summary>
		public void OnTouchNormalButton()
		{
			if (this.guiTexture.texture)
			{
				this.guiTexture.texture = textureBoxOnNormal;
			}
		}

		#endregion
	}
}

