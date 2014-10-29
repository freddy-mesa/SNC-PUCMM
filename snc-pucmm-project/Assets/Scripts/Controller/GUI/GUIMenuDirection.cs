using SncPucmm.Controller.Control;
using SncPucmm.Model.Navigation;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller.GUI
{
	class GUIMenuDirection : IMenu, IButton
	{
		#region Atributos

		string name;

		List<Button> buttonList;
		List<PathDataDijkstra> directionPath;
		int currentDirectionPath;

		bool isBackButtonActive;
		bool isNextButtonActive;
		
		#endregion

		#region Constructor

		public GUIMenuDirection(string name, List<PathDataDijkstra> nodesDirectionPath)
		{
			this.name = name;
			this.directionPath = nodesDirectionPath;

			currentDirectionPath = 0;
			isBackButtonActive = false;
			isNextButtonActive = true;

			Initializer();
		}
		
		#endregion

		#region Metodos

		private void Initializer()
		{
			buttonList = new List<Button>();

			Button buttonBack = new Button("ButtonDirectionBack");
			buttonBack.OnTouchEvent += new OnTouchEventHandler(OnTouchDirectionBack);
			buttonList.Add(buttonBack);

			Button buttonNext = new Button("ButtonDirectionNext");
			buttonNext.OnTouchEvent += new OnTouchEventHandler(OnTouchDirectionNext);
			buttonList.Add(buttonNext);

			Button buttonResume = new Button("ButtonResume");
			buttonResume.OnTouchEvent += new OnTouchEventHandler(OnTouchResume);
			buttonList.Add(buttonResume);

			Button buttonExit = new Button("ButtonExit");
			buttonExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExit);
			buttonList.Add(buttonExit);

			UIUtils.FindGUI("GUIMenuDirection/" + buttonList[0].Name).SetActive(false);
			UIUtils.FindGUI("GUIMenuDirection/" + buttonList[1].Name).SetActive(true);

			ShowDirectionMenu(directionPath[currentDirectionPath]);
			ShowNavigationDirection(directionPath[currentDirectionPath]);
		}

		public void OnTouchDirectionBack(object sender, TouchEventArgs e)
		{
			currentDirectionPath--;

			if (currentDirectionPath <= 0)
			{
				currentDirectionPath = 0;
				UIUtils.FindGUI("GUIMenuDirection/" + buttonList[0].Name).SetActive(false);
				isBackButtonActive = false;
			}

			if (!isNextButtonActive)
			{
				UIUtils.FindGUI("GUIMenuDirection/" + buttonList[1].Name).SetActive(true);
				isNextButtonActive = true;
			}

			ShowDirectionMenu(directionPath[currentDirectionPath]);
			ShowNavigationDirection(directionPath[currentDirectionPath]);
		}

		public void OnTouchDirectionNext(object sender, TouchEventArgs e)
		{
			currentDirectionPath++;

			if (currentDirectionPath + 1 >= directionPath.Count)
			{
				currentDirectionPath = directionPath.Count - 1;
				UIUtils.FindGUI("GUIMenuDirection/" + buttonList[1].Name).SetActive(false);
				isNextButtonActive = false;
			}

			if (!isBackButtonActive)
			{
				UIUtils.FindGUI("GUIMenuDirection/" + buttonList[0].Name).SetActive(true);
				isBackButtonActive = true;
			}

			ShowDirectionMenu(directionPath[currentDirectionPath]);
			ShowNavigationDirection(directionPath[currentDirectionPath]);
		}

		public void OnTouchResume(object sender, TouchEventArgs e) 
		{
			PathDataDijkstra path = directionPath[currentDirectionPath];
			float posX = UIUtils.getXDistance(path.StartNode.Longitude) - 50f;
			float posZ = UIUtils.getZDistance(path.StartNode.Latitude);

			Transform camera = UIUtils.Find("/Vista3erPersona").camera.transform;
			camera.eulerAngles = new Vector3(camera.eulerAngles.x, 90, 0f);
			camera.position = new Vector3(posX, camera.position.y, posZ);
		}

		public void OnTouchExit(object sender, TouchEventArgs e) 
		{
			UIUtils.DestroyChilds("/PUCMM/Directions", false);
			MenuManager.GetInstance().RemoveCurrentMenu();
			
			State.ChangeState(eState.MenuBuildingDescriptor);
		}

		private void ShowDirectionMenu(PathDataDijkstra path)
		{
			String label = String.Format("{0} metros desde {1} hacia {2}. Metros Recorridos: {3}", path.DistanceToNeighbor, path.StartNode.Name, path.EndNode.Name, path.DistancePathed);
			UIUtils.FindGUI("GUIMenuDirection/Text").guiText.text = UIUtils.FormatStringLabel(label, ' ', 20);
		}

		private void ShowNavigationDirection(PathDataDijkstra path)
		{
			var directions = UIUtils.Find("/PUCMM/Directions").GetComponent<UIDirections>();
			directions.PrintPath(directionPath, path);
		}

		#region Implemented methods

		public string GetMenuName()
		{
			return name;
		}

		public List<Button> GetButtonList()
		{
			return buttonList;
		}

		#endregion

		#endregion
		
	}
}
