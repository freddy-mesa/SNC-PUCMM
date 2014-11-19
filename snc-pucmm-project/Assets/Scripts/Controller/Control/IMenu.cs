using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.Control
{
	public interface IMenu
	{
		/// <summary>
		/// Gets the menu name
		/// </summary>
		/// <value>The menu name.</value>
		string GetMenuName();

		void Update();
	}
}