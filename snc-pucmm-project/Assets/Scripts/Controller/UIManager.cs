using System;
using System.Collections.Generic;
using UnityEngine;
using SncPucmm.Utils;
using SncPucmm.Database;
using SncPucmm.Model;

namespace SncPucmm.Controller
{
	public class UIManager : TouchManager
	{
		#region Metodos

		void Awake(){
			UIMenuManager.GetInstance().AddMenu("GUIMainMenu");
		}

		/// <summary>
		/// Raises the touch building event.
		/// </summary>
		/// <param name="buildingAbbreviationName">Building abbreviation name.</param>
		public void OnTouchBuilding(string buildingAbbreviationName)
		{
			UIMenuManager.GetInstance().AddMenu("GUIMenuDescriptor");

//			var dataReader = SQLiteService.GetInstance().SelectQuery(
//				"ubicacion", null, new Dictionary<string, object> {
//					{"abreviacion", buildingAbbreviationName}
//				}
//			);
//
//			while (dataReader.Read()){
//				ModelPool.GetInstance().Add("building", new Building(
//					Convert.ToInt32(dataReader["idUbicacion"]),
//					Convert.ToString(dataReader["nombre"]), 
//					Convert.ToString(dataReader["abreviacion"]),
//					UIMenuManager.GetInstance().GetCurrentMenu().Name
//				));
//			}
//
//			var lblBuildingName = UIMenuManager.GetInstance().Find(UIMenuManager.GetInstance().GetCurrentMenu().Name)
//				.transform.FindChild("LabelBuildingName");

//			lblBuildingName.guiText.text = FormatStringLabel(((Building) ModelPool.GetInstance().GetValue("building")).Name);

			State.ChangeState(eState.GUISystem);
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
		/// Raises the touch normal button event.
		/// </summary>
		public virtual void OnTouchNormalButton(){}

		/// <summary>
		/// Formats string of a Label for Menu Descriptor.
		/// </summary>
		/// <returns>The building name label.</returns>
		/// <param name="stringToFormat">String to format.</param>
		public String FormatStringLabel(String stringToFormat) {
			char delimitor = ' ';
			string[] words = stringToFormat.Split(delimitor); //Split the string into seperate words
			string result = "";
			int runningLength = 0;

			foreach (string word in words) {
				if (runningLength + word.Length+1 <= 20) {
					result += " " + word;
					runningLength += word.Length+1;
				}
				else {
					result += "\n" + word;
					runningLength = word.Length;
				}
			}
			return result.Remove(0,1); //Remove the first space
		}

		#endregion
	}
}

