using System;
using UnityEngine;

namespace UserInterface
{
	public class UIManager : TouchManager
	{
		#region Atributos

		#endregion

		#region Propiedades

		/// <summary>
		/// Gets the name of the building.
		/// </summary>
		/// <value>The name of the building.</value>
		public string BuildingName { get; set; }
			
		#endregion

		#region Metodos

		/// <summary>
		/// Raises the touch event.
		/// </summary>
		/// <param name="buildingName">Building name.</param>
		public void OnTouchBuilding(string buildingName)
		{
			Debug.Log("click " + buildingName);
			this.BuildingName = buildingName;
			var gameObject = GameObject.Find("GUI");
			gameObject.SetActive(true);

			//Encontrar a GUIMenuDescriptor y activarlo
			var menuDescriptor = gameObject.transform.FindChild("GUIMenuDescriptor");
			menuDescriptor.gameObject.SetActive(true);

			isGUIVisible = true;
//			var button = gameObject.GetComponent<UIButtonManager>();
		}

		/// <summary>
		/// Raises the touch button event.
		/// </summary>
		public virtual void OnTouchButton(){}

		/// <summary>
		/// Raises the touch hover button event.
		/// </summary>
		public virtual void OnTouchHoverButton(){}

		/// <summary>
		/// Raises the GU event.
		/// </summary>
		public virtual void OnGUI(){}

		#endregion
	}
}

