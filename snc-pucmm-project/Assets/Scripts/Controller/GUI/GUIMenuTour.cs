using SncPucmm.Controller.Control;
using SncPucmm.Controller.Tours;
using SncPucmm.Model;
using SncPucmm.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Controller.GUI
{
	class GUIMenuTour : IMenu, IButton
	{
		#region Atributos

		List<Button> butttonList;
		List<ModelLocalizacion> listModelLocalizacion;

		#endregion

		#region Constructor

		public GUIMenuTour()
		{
			Initializer();
		}

		#endregion

		#region Metodos

		private void Initializer()
		{
			listModelLocalizacion = new List<ModelLocalizacion>();
			butttonList = new List<Button>();

			Button addButton = new Button("ButtonAddBuilding");
			addButton.OnTouchEvent += new OnTouchEventHandler(OnTouchAddButton);
			butttonList.Add(addButton);

			Button removeButton = new Button("ButtonRemoveBuilding");
			removeButton.OnTouchEvent += new OnTouchEventHandler(OnTouchRemoveButton);
			butttonList.Add(addButton);

			Button clearButton = new Button("ButtonClearAllBuilding");
			clearButton.OnTouchEvent += new OnTouchEventHandler(OnTouchClearButton);
			butttonList.Add(clearButton);

			Button createButton = new Button("ButtonCreateTour");
			createButton.OnTouchEvent += new OnTouchEventHandler(OnTouchCreateTourButton);
			butttonList.Add(createButton);
		}

		private void OnTouchCreateTourButton(object sender, TouchEventArgs e)
		{
			var controller = (TourController) ModelPoolManager.GetInstance().GetValue("tourCrl");
		}

		private void OnTouchClearButton(object sender, TouchEventArgs e)
		{
			listModelLocalizacion.Clear();
		}

		private void OnTouchRemoveButton(object sender, TouchEventArgs e)
		{
			var modelLocalizacion = (ModelLocalizacion)e.Mensaje;
			listModelLocalizacion.Remove(modelLocalizacion);
		}

		private void OnTouchAddButton(object sender, TouchEventArgs e)
		{
			var modelLocalizacion = (ModelLocalizacion) e.Mensaje;
			listModelLocalizacion.Add(modelLocalizacion);
		}

		#region Implemented Methods

		public string GetMenuName()
		{
			throw new NotImplementedException();
		}

		public List<Button> GetButtonList()
		{
			throw new NotImplementedException();
		}
		 
		#endregion

		#endregion
	}
}
