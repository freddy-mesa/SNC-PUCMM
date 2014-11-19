using UnityEngine;
using System.Collections.Generic;
using SncPucmm.Controller;
using SncPucmm.Controller.Control;
using System;
using System.Text;

namespace SncPucmm.View     
{
	public class UIButtonControl : UITouch
	{
		#region Metdos

		/// <summary>
		/// Raise a touch event on button
		/// </summary>
		public void OnClick()
		{
			if (!isMoving && !isZooming && !isRotating)
			{
				isButtonTapped = true;

				var menu = MenuManager.GetInstance().GetCurrentMenu() as IButton;

				menu.GetButtonList().ForEach(button =>
				{
					if (button.Name.Equals(this.name))
					{
						button.OnTouch(null);
					}
				});
			}
		}

		#endregion
	}
}

