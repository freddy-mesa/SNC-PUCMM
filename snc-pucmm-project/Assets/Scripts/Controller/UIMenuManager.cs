using System;
using System.Collections.Generic;
using UnityEngine;
using SncPucmm.Model;

namespace SncPucmm.Controller
{
	public class UIMenuManager
	{
		#region Atributos
		private List<UIMenu> _menuList;
		private GameObject _guiObject;

		private static UIMenuManager _menuManager;
		#endregion

		#region Constructor
		private UIMenuManager()
		{
			_menuList = new List<UIMenu>();
			_guiObject = GameObject.Find("GUI");

           this.AddMenu("GUIMainMenu");
		}
		#endregion

		#region Metodos

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		public static UIMenuManager GetInstance()
		{
			if(_menuManager == null)
				_menuManager = new UIMenuManager();

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
		/// Activates the GU.
		/// </summary>
		/// <param name="Activate">If set to <c>true</c> activate.</param>
		public void ActivateGUI(bool activate){
			_guiObject.SetActive(activate);
		}

		/// <summary>
		/// Activates the current menu.
		/// </summary>
		/// <param name="activate">If set to <c>true</c> activate.</param>
		public void ActivateCurrentMenu(bool activate){
			Find(GetCurrentMenu().Name).SetActive(activate);
		}

		/// <summary>
		/// Finds the child.
		/// </summary>
		/// <returns>The child.</returns>
		/// <param name="Name">Name.</param>
		public GameObject Find(String name){
			return _guiObject.transform.FindChild(name).gameObject;
		}

		#endregion

	}
}