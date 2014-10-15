using SncPucmm.Controller.Control;
using SncPucmm.Model.Navigation;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.GUI
{
	class GUIMenuDirection : IMenu, IButton
	{
		#region Atributos

		string name;

		List<Button> buttonList;
		List<PathData> directionPath;
		int currentDirectionPath;

		bool isBackButtonActive;
		bool isNextButtonActive;
		
		#endregion

		#region Constructor

		public GUIMenuDirection(string name, List<PathData> nodesDirectionPath)
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

		private void ShowDirectionMenu(PathData path)
		{
			String label = "Next " + path.Direction.ToString() + " from " + path.StartNode.Name + " to " + path.EndNode.Name;
			UIUtils.FindGUI("GUIMenuDirection/Text").guiText.text = UIUtils.FormatStringLabel(label, ' ', 20);
		}

		private void ShowNavigationDirection(PathData path)
		{
			var directions = UIUtils.Find("/PUCMM/Directions").GetComponent<UIDirections>();
			directions.PrintDirections(directionPath, path);
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
