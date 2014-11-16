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
	class MenuNavigation : IMenu, IButton
	{
		#region Atributos

		string name;

		List<Button> buttonList;
		public List<PathDataDijkstra> directionPath;
		public int currentDirectionPath;

		bool isBackButtonActive;
		bool isNextButtonActive;
		string tourName;

		#endregion

		#region Constructor

		public MenuNavigation()
		{
			isBackButtonActive = false;
			isNextButtonActive = true;
		}

		public MenuNavigation(string name, List<PathDataDijkstra> nodesDirectionPath) 
			: this ()
		{
			this.name = name;
			this.directionPath = nodesDirectionPath;

			currentDirectionPath = 0;
			
			Initializer();
		}

		public MenuNavigation(string name, List<PathDataDijkstra> nodesDirectionPath, int currentPathIndex, string tourName)
			: this()
		{
			this.name = name;
			this.directionPath = nodesDirectionPath;
			this.tourName = tourName;
			this.currentDirectionPath = currentPathIndex;

			Initializer();
		}
		
		#endregion

		#region Metodos

		private void Initializer()
		{
			buttonList = new List<Button>();

			Button buttonBack = new Button("ButtonBack");
			buttonBack.OnTouchEvent += new OnTouchEventHandler(OnTouchDirectionBack);
			buttonList.Add(buttonBack);

			Button buttonNext = new Button("ButtonNext");
			buttonNext.OnTouchEvent += new OnTouchEventHandler(OnTouchDirectionNext);
			buttonList.Add(buttonNext);

			Button buttonResume = new Button("ButtonResume");
			buttonResume.OnTouchEvent += new OnTouchEventHandler(OnTouchResume);
			buttonList.Add(buttonResume);

			Button buttonExit = new Button("ButtonExit");
			buttonExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExit);
			buttonList.Add(buttonExit);

			Button buttonOk = new Button("ButtonOk");
			buttonOk.OnTouchEvent += new OnTouchEventHandler(OnTouchExit);
			buttonList.Add(buttonOk);

			UIUtils.FindGUI("MenuNavigation/" + buttonList[0].Name).SetActive(false);
			UIUtils.FindGUI("MenuNavigation/" + buttonList[1].Name).SetActive(true);

			var tourBar = UIUtils.FindGUI("MenuNavigation/NameTourBar"); 
			if (State.GetCurrentState() == eState.Tour)
			{
				tourBar.SetActive(true);
				var label = tourBar.transform.FindChild("Label").GetComponent<UILabel>();
				label.text = tourName;
			}
			else
			{
				tourBar.SetActive(false);
			}

			UIUtils.FindGUI("MenuNavigation/TourNotification").SetActive(false);

			ShowDirectionMenu(directionPath[currentDirectionPath]);
			ShowNavigationDirection(directionPath[currentDirectionPath]);
		}

		public void OnTouchDirectionBack(object sender, TouchEventArgs e)
		{
			currentDirectionPath--;

			if (currentDirectionPath <= 0)
			{
				currentDirectionPath = 0;
				UIUtils.FindGUI("MenuNavigation/" + buttonList[0].Name).SetActive(false);
				isBackButtonActive = false;
			}

			if (!isNextButtonActive)
			{
				UIUtils.FindGUI("MenuNavigation/" + buttonList[1].Name).SetActive(true);
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
				UIUtils.FindGUI("MenuNavigation/" + buttonList[1].Name).SetActive(false);
				isNextButtonActive = false;
			}

			if (!isBackButtonActive)
			{
				UIUtils.FindGUI("MenuNavigation/" + buttonList[0].Name).SetActive(true);
				isBackButtonActive = true;
			}

			ShowDirectionMenu(directionPath[currentDirectionPath]);
			ShowNavigationDirection(directionPath[currentDirectionPath]);
		}

		public void OnTouchResume(object sender, TouchEventArgs e) 
		{
			PathDataDijkstra path = directionPath[currentDirectionPath];

			float nodePosX = UIUtils.getXDistance(path.StartNode.Longitude) - 50f;
			float nodePosZ = UIUtils.getZDistance(path.StartNode.Latitude);

			Transform camera = UIUtils.Find("/Vista3erPersona").camera.transform;
			camera.eulerAngles = new Vector3(camera.eulerAngles.x, 90, 0f);

			UICamaraControl.targetTransitionPosition = new Vector3(nodePosX, camera.position.y, nodePosZ);
			UICamaraControl.isTransitionAnimated = true;
		}

		public void OnTouchExit(object sender, TouchEventArgs e) 
		{
			UIUtils.DestroyChilds("/PUCMM/Directions", false);
			MenuManager.GetInstance().RemoveCurrentMenu();

			if (ModelPoolManager.GetInstance().Contains("tourCtrl"))
			{
				State.ChangeState(eState.Tour);
				ModelPoolManager.GetInstance().Remove("tourCtrl");
			}
			else
			{
				State.ChangeState(eState.MenuBuilding);
			}
		}

		private void ShowDirectionMenu(PathDataDijkstra path)
		{
			String labelText = String.Format("{0:F2} metros desde {1} hacia {2}. Metros Recorridos: {3:F2} metros", path.DistanceToNeighbor, path.StartNode.Name, path.EndNode.Name, path.DistancePathed);
			UILabel label = UIUtils.FindGUI("MenuNavigation/StatusBar/Label").GetComponent<UILabel>();
			label.text = UIUtils.FormatStringLabel(labelText, ' ', 20);
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

		#region Destructor

		~MenuNavigation()
		{
			this.buttonList = null;
			this.directionPath = null;
			this.name = null;
		}

		#endregion
	}
}
