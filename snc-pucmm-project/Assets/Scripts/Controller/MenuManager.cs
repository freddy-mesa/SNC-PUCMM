using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using SncPucmm.View;
using SncPucmm.Controller.Control;

namespace SncPucmm.Controller
{
	public class MenuManager
	{
		#region Atributos

		private List<IMenu> _menuList;
		private static MenuManager _menuManager;

		#endregion

		#region Constructor

		private MenuManager()
		{
			_menuList = new List<IMenu>();
		}
		
		#endregion

		#region Metodos

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		public static MenuManager GetInstance()
		{
			if(_menuManager == null)
				_menuManager = new MenuManager();

			return _menuManager;
		}

		/// <summary>
		/// Adds the menu.
		/// </summary>
		/// <param name="menuName">Menu name.</param>
		public void AddMenu(IMenu newMenu)
		{
			_menuList.Add(newMenu);
			ActivateCurrentMenu(true);
		}

		/// <summary>
		/// Removes the current menu.
		/// </summary>
		public void RemoveCurrentMenu(){
			if (this._menuList.Count > 1)
			{
				ActivateCurrentMenu(false);
				_menuList.Remove(GetCurrentMenu());
			}
		}

		/// <summary>
		/// Gets the current menu.
		/// </summary>
		/// <returns>The current menu.</returns>
		public IMenu GetCurrentMenu(){
			return _menuList[_menuList.Count-1];
		}

		/// <summary>
		/// No menu left.
		/// </summary>
		/// <returns><c>true</c>, if no menu left, <c>false</c> otherwise.</returns>
		public bool NoMenuLeft(){
			if(_menuList.Count == 1)
				return true;

			//Active Last Menu
			ActivateCurrentMenu(true);
			return false;
		}

		/// <summary>
		/// Activates the current menu.
		/// </summary>
		/// <param name="activate">If set to <c>true</c> activate.</param>
		public void ActivateCurrentMenu(bool activate){
			var menu = UIUtils.FindGUI(GetCurrentMenu().GetMenuName());
			menu.SetActive(activate);
		}

		#endregion

	}
}