using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using SncPucmm.View;
using SncPucmm.Controller.Control;
using SncPucmm.Controller.GUI;

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

			if (this._menuList.Count > 1)
			{
				ActivatePreviousMenu(false);
			}
			
		}

		/// <summary>
		/// Removes the current menu.
		/// </summary>
		public void RemoveCurrentMenu()
		{
			if (this._menuList.Count > 1)
			{
				//Desactive Current Menu
				ActivateCurrentMenu(false);

				//Activate Last Menu
				ActivatePreviousMenu(true);

				//Remove Actual Menu
				_menuList.Remove(GetCurrentMenu());

				//Update Last Menu
				GetCurrentMenu().Update();
			}
		}

		/// <summary>
		/// Gets the current menu.
		/// </summary>
		/// <returns>The current menu.</returns>
		public IMenu GetCurrentMenu(){
			return _menuList[_menuList.Count - 1];
		}

		public IMenu GetPrevioudMenu()
		{
			return _menuList[_menuList.Count - 2];
		}

		/// <summary>
		/// No menu left.
		/// </summary>
		/// <returns><c>true</c>, if no menu left, <c>false</c> otherwise.</returns>
		public bool NoMenuLeft()
		{
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
		public void ActivateCurrentMenu(bool activate)
		{
			var menu = UIUtils.FindGUI(GetCurrentMenu().GetMenuName());
			menu.SetActive(activate);
		}

		private void ActivatePreviousMenu(bool activate)
		{
			var menu = UIUtils.FindGUI(GetPrevioudMenu().GetMenuName());
			menu.SetActive(activate);
		}

		#endregion

	}
}