using System.Collections.Generic;
using UnityEngine;
using SncPucmm.Database;
using SncPucmm.Model;
using SncPucmm.Controller;
using SncPucmm.Controller.GUI;
using SncPucmm.Controller.Control;
using System;

namespace SncPucmm.View
{
	public class UIModel : UITouch
	{
		#region Metodos

		/// <summary>
		/// Raises the touch building event.
		/// </summary>
		/// <param name="buildingAbbreviationName">Building abbreviation name.</param>
		public void OnTouchBuilding(object obj)
		{
			var idLocation = obj.GetType().GetProperty("location").GetValue(obj, null);
			var button = Convert.ToString(obj.GetType().GetProperty("button").GetValue(obj, null));

			var menu = MenuManager.GetInstance().GetCurrentMenu() as IButton;
			menu.GetButtonList().ForEach(x => {
				if (x.Name.Equals(button))
				{ 
					x.OnTouch(idLocation); 
				}
			});
		}

		#endregion
	}
}

