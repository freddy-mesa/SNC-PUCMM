using SncPucmm.Controller.Control;
using SncPucmm.Controller.Tours;
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
		public int currentPathFreeMode;

		bool isBackButtonActive;
		bool isNextButtonActive;
		string tourName;

		public bool isFreeModeActive;

		#endregion

		#region Constructor

		public MenuNavigation()
		{
			isBackButtonActive = false;
			isNextButtonActive = false;
		}

		public MenuNavigation(string name, List<PathDataDijkstra> nodesDirectionPath) 
			: this ()
		{
			this.name = name;
			this.directionPath = nodesDirectionPath;

			currentDirectionPath = currentPathFreeMode = 0;
			
			Initializer();
		}

		public MenuNavigation(string name, List<PathDataDijkstra> nodesDirectionPath, int currentPathIndex, string tourName)
			: this()
		{
			this.name = name;
			this.directionPath = nodesDirectionPath;
			this.tourName = tourName;
			this.currentDirectionPath = currentPathFreeMode = currentPathIndex;

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

			Button buttonFree = new Button("ButtonFreeMode");
			buttonFree.OnTouchEvent += new OnTouchEventHandler(OnTouchFreeMode);
			buttonList.Add(buttonFree);

			Button buttonOk = new Button("ButtonOk");
			buttonOk.OnTouchEvent += new OnTouchEventHandler(OnTouchOKButton);
			buttonList.Add(buttonOk);

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

			ShowDirectionMenu(directionPath[currentDirectionPath]);
			ShowNavigationDirection(directionPath[currentDirectionPath]);
		}

		private void OnTouchOKButton(object sender, TouchEventArgs e)
		{
			var tourCtrl = ModelPoolManager.GetInstance().GetValue("tourCtrl") as TourController;
			if (!tourCtrl.isEndTour)
			{
				var tourNotification = UIUtils.FindGUI("MenuNavigation/NotificationSeccionTourCompletada");
				tourNotification.SetActive(false);
			}
			else
			{
				Exit();
			}
		}

		private void OnTouchFreeMode(object sender, TouchEventArgs e)
		{
			isFreeModeActive = true;
			currentPathFreeMode = currentDirectionPath;
			UIUtils.FindGUI("MenuNavigation/FreeMode").SetActive(true);

			if (currentPathFreeMode <= 0)
			{
				UIUtils.FindGUI("MenuNavigation/FreeMode/" + buttonList[0].Name).SetActive(false);
				isBackButtonActive = false;
			}
			else
			{
				UIUtils.FindGUI("MenuNavigation/FreeMode/" + buttonList[0].Name).SetActive(true);
				isBackButtonActive = true;
			}

			if (currentPathFreeMode + 1 >= directionPath.Count)
			{
				UIUtils.FindGUI("MenuNavigation/FreeMode/" + buttonList[1].Name).SetActive(false);
				isNextButtonActive = false;
			}
			else
			{
				UIUtils.FindGUI("MenuNavigation/FreeMode/" + buttonList[1].Name).SetActive(true);
				isNextButtonActive = true;
			}
		}

		public void OnTouchDirectionBack(object sender, TouchEventArgs e)
		{
			currentPathFreeMode--;

			if (currentPathFreeMode <= 0)
			{
				currentPathFreeMode = 0;
				UIUtils.FindGUI("MenuNavigation/FreeMode/" + buttonList[0].Name).SetActive(false);
				isBackButtonActive = false;
			}

			if (!isNextButtonActive)
			{
				UIUtils.FindGUI("MenuNavigation/FreeMode/" + buttonList[1].Name).SetActive(true);
				isNextButtonActive = true;
			}

			ShowDirectionMenu(directionPath[currentPathFreeMode]);
			ShowNavigationDirection(directionPath[currentPathFreeMode]);
			MoveCameraToNode();
		}

		public void OnTouchDirectionNext(object sender, TouchEventArgs e)
		{
			currentPathFreeMode++;

			if (currentPathFreeMode + 1 >= directionPath.Count)
			{
				currentPathFreeMode = directionPath.Count - 1;
				UIUtils.FindGUI("MenuNavigation/FreeMode/" + buttonList[1].Name).SetActive(false);
				isNextButtonActive = false;
			}

			if (!isBackButtonActive)
			{
				UIUtils.FindGUI("MenuNavigation/FreeMode/" + buttonList[0].Name).SetActive(true);
				isBackButtonActive = true;
			}

			ShowDirectionMenu(directionPath[currentPathFreeMode]);
			ShowNavigationDirection(directionPath[currentPathFreeMode]);
			MoveCameraToNode();
		}

		public void OnTouchResume(object sender, TouchEventArgs e) 
		{
			UIUtils.FindGUI("MenuNavigation/FreeMode").SetActive(false);
			isFreeModeActive = false;
			
			ShowDirectionMenu(directionPath[currentDirectionPath]);
			ShowNavigationDirection(directionPath[currentDirectionPath]);
			
			UIUtils.MoveCameraToUser();
		}

		public void OnTouchExit(object sender, TouchEventArgs e) 
		{
			Exit();
		}

		private void Exit()
		{
			UIUtils.ShowAllBuildingExterior(directionPath);
			UIUtils.DestroyChilds("/PUCMM/Directions", false);
			MenuManager.GetInstance().RemoveCurrentMenu();
		}

		private void MoveCameraToNode()
		{
			PathDataDijkstra path = directionPath[currentPathFreeMode];

			float nodePosX = path.StartNode.Longitude - 40f;
			float nodePosZ = path.StartNode.Latitude;

			Transform camera = UIUtils.Find("/Vista3erPersona").camera.transform;
			camera.eulerAngles = new Vector3(camera.eulerAngles.x, 90, 0f);
			camera.position = new Vector3(camera.position.x, 30f, camera.position.z);

			UICamaraControl.targetTransitionPosition = new Vector3(nodePosX, camera.position.y, nodePosZ);
			UICamaraControl.isTransitionAnimated = true;
		}

		public void ShowDirectionMenu(PathDataDijkstra path)
		{
			string labelText = string.Empty;
			if (Application.platform == RuntimePlatform.WindowsEditor)
			{
				labelText = string.Format("{0:F2} metros desde {1} hacia {2}. Metros Recorridos: {3:F2} metros", path.DistanceToNeighbor, path.StartNode.Name, path.EndNode.Name, path.DistancePathed);
			}
			else
			{
				labelText = string.Format("{0:F2} metros hasta la próxima interseccion. Metros Recorridos: {1:F2} metros", path.DistanceToNeighbor, path.DistancePathed - path.DistanceToNeighbor);
			}

			UILabel label = UIUtils.FindGUI("MenuNavigation/StatusBar/Label").GetComponent<UILabel>();
			label.text = UIUtils.FormatStringLabel(labelText, ' ', 20);
		}

		public void ShowNavigationDirection(PathDataDijkstra path)
		{
			UIUtils.EnableInsideFloorCaminosBuilding(directionPath);

			if (path.StartNode.IsInsideBuilding)
			{
				UIUtils.ShowInsidePlaneBuilding(path.StartNode.BuildingName, "Planta" + path.StartNode.PlantaBuilding);
			}

			var directions = UIUtils.Find("/PUCMM/Directions").GetComponent<UIDirections>();
			directions.PrintPath(directionPath, path);

			UIUtils.DisableInsideFloorCaminosBuilding(directionPath, path);
		}

		#region Implemented methods

		public string GetMenuName()
		{
			return name;
		}

		public void Update()
		{
			
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
