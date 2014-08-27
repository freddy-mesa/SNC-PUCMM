using System;
using System.Collections.Generic;
using UnityEngine;
using SncPucmm.Model;
using SncPucmm.Controller.Utils;

namespace SncPucmm.Controller
{
	public class UIMenuController
	{
		#region Atributos
		private List<UIMenu> _menuList;

        private static UIMenuController _menuManager;

		#endregion

		#region Constructor
        
        private UIMenuController()
		{
			_menuList = new List<UIMenu>();
           this.AddMenu("GUIMainMenu");
		}
		
        #endregion

		#region Metodos

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		public static UIMenuController GetInstance()
		{
			if(_menuManager == null)
				_menuManager = new UIMenuController();

			return _menuManager;
		}

		/// <summary>
		/// Adds the menu.
		/// </summary>
		/// <param name="menuName">Menu name.</param>
		public void AddMenu(String menuName){
			UIMenu newMenu = new UIMenu(menuName);
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
		public UIMenu GetCurrentMenu(){
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
			UIUtils.FindGUI(GetCurrentMenu().Name).SetActive(activate);
		}

		#endregion

	}
}