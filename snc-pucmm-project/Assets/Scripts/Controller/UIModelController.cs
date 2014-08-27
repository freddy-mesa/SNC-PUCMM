using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SncPucmm.Utils;
using SncPucmm.Database;
using SncPucmm.Model;
using SncPucmm.Controller.Utils;

namespace SncPucmm.Controller
{
	public class UIModelController : TouchManager
	{
		#region Metodos

		/// <summary>
		/// Raises the touch building event.
		/// </summary>
		/// <param name="buildingAbbreviationName">Building abbreviation name.</param>
		public void OnTouchBuilding(string buildingAbbreviationName)
		{
            var menuManager = UIMenuController.GetInstance();
            menuManager.AddMenu("GUIMenuDescriptor");
            
            var sqliteService = SQLiteService.GetInstance();
            var dataReader = sqliteService.SelectQuery(
				"Ubicacion", null, new Dictionary<string, object> {
					{"abreviacion", buildingAbbreviationName}
				},
                null
			);

            var modelPool = ModelPoolController.GetInstance();
			while (dataReader.Read())
            {
                modelPool.Add("building", new Building(
					Convert.ToInt32(dataReader["idUbicacion"]),
					Convert.ToString(dataReader["nombre"]), 
					Convert.ToString(dataReader["abreviacion"])
				));
			}

            var lblBuildingName = UIUtils.FindGUI(menuManager.GetCurrentMenu().Name + "/LabelBuildingName");

			lblBuildingName.guiText.text = UIUtils.FormatStringLabel(((Building) modelPool.GetValue("building")).Name, ' ', 20);

			State.ChangeState(eState.GUISystem);
		}

		#endregion
	}
}

