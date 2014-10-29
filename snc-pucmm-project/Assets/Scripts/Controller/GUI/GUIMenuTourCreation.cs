using SncPucmm.Controller.Control;
using SncPucmm.Controller.Tours;
using SncPucmm.Model;
using SncPucmm.Model.Domain;
using SncPucmm.View;
using System.Collections.Generic;

namespace SncPucmm.Controller.GUI
{
	class GUIMenuTourCreation : IMenu, IButton
	{
		#region Atributos

		string name;

		List<Button> butttonList;
		List<ModelNode> listModelNode;

		#endregion

		#region Constructor

		public GUIMenuTourCreation(string name)
		{
			this.name = name;
			Initializer();
		}

		#endregion

		#region Metodos

		private void Initializer()
		{
			listModelNode = new List<ModelNode>();

			butttonList = new List<Button>();

			Button addButton = new Button("ButtonAddBuilding");
			addButton.OnTouchEvent += new OnTouchEventHandler(OnTouchAddButton);
			butttonList.Add(addButton);

			Button removeButton = new Button("ButtonRemoveBuilding");
			removeButton.OnTouchEvent += new OnTouchEventHandler(OnTouchRemoveButton);
			butttonList.Add(removeButton);

			Button clearButton = new Button("ButtonClearAllBuilding");
			clearButton.OnTouchEvent += new OnTouchEventHandler(OnTouchClearButton);
			butttonList.Add(clearButton);

			Button saveButton = new Button("ButtonCreateTour");
			saveButton.OnTouchEvent += new OnTouchEventHandler(OnTouchSaveTourButton);
			butttonList.Add(saveButton);

			Button exitButton = new Button("ButtonExit");
			exitButton.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
			butttonList.Add(exitButton);
		}

		private void OnTouchExitButton(object sender, TouchEventArgs e)
		{
			UIUtils.ActivateCameraLabels(true);
			MenuManager.GetInstance().RemoveCurrentMenu();
			State.ChangeState(eState.Navigation);
		}

		private void OnTouchSaveTourButton(object sender, TouchEventArgs e)
		{
			GUIMenuTourCreationForm form = new GUIMenuTourCreationForm("GUIMenuTourCreationForm", listModelNode);
			MenuManager.GetInstance().AddMenu(form);
			State.ChangeState(eState.TourCreationForm);
		}

		private void OnTouchClearButton(object sender, TouchEventArgs e)
		{
			listModelNode.Clear();
		}

		private void OnTouchRemoveButton(object sender, TouchEventArgs e)
		{
			var modelObject = e.Mensaje as ModelObject;
			modelObject.TourActive = false;
			modelObject.isSeleted = false;

			listModelNode.Remove(modelObject.ObjectTag as ModelNode);
		}

		private void OnTouchAddButton(object sender, TouchEventArgs e)
		{
			var modelObject = e.Mensaje as ModelObject;
			modelObject.TourActive = true;
			modelObject.isSeleted = false;

			listModelNode.Add(modelObject.ObjectTag as ModelNode);
		}

		public List<ModelNode> GetModelNodeList()
		{
			return this.listModelNode;
		}

		#region Implemented Methods

		public string GetMenuName()
		{
			return name;
		}

		public List<Button> GetButtonList()
		{
			return butttonList;
		}
		 
		#endregion

		#endregion

		#region Destructor

		~ GUIMenuTourCreation()
		{
			this.butttonList = null;
			this.name = null;
			this.listModelNode = null;
		}

		#endregion
	}
}
