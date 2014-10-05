using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using SncPucmm.Controller.Control;
using SncPucmm.Controller;
using SncPucmm.Database;
using SncPucmm.Model;
using SncPucmm.View;

namespace SncPucmm.Controller.GUI
{
	class GUIMenuMain : IMenu, ITextBox, IButton, IScrollTreeView
	{
		#region Atributos

		private string name;

		List<Button> buttonList;
		List<TextBox> textBoxList;
		ScrollTreeView treeView;

		#endregion

		#region Constructores

		public GUIMenuMain(string name)
		{
			this.name = name;
			Initializer();
		}

		#endregion

		#region Metodos

		private void Initializer()
		{
			buttonList = new List<Button>();

			var ModelController = new Button("ModelController");
			ModelController.OnTouchEvent += new OnTouchEventHandler(OnTouchModelController);
			buttonList.Add(ModelController);

			var ButtonMain = new Button("ButtonMain");
			ButtonMain.OnTouchEvent += new OnTouchEventHandler(OnTouchButtonMain);
			buttonList.Add(ButtonMain);

			var ButtonRegistrar = new Button("ButtonRegistrar");
			ButtonRegistrar.OnTouchEvent += new OnTouchEventHandler(OnTouchButtonRegistrar);
			buttonList.Add(ButtonRegistrar);

			var ButtonCambioVista = new Button("ButtonCambioVista");
			ButtonCambioVista.OnTouchEvent += new OnTouchEventHandler(OnTouchButtonCambioVista);
			buttonList.Add(ButtonCambioVista);

			var ButtonSeguridad = new Button("ButtonSeguridad");
			ButtonSeguridad.OnTouchEvent += new OnTouchEventHandler(OnTouchButtonSeguridad);
			buttonList.Add(ButtonSeguridad);

			var ButtonTours = new Button("ButtonTours");
			ButtonTours.OnTouchEvent += new OnTouchEventHandler(OnTouchButtonTours);
			buttonList.Add(ButtonTours);

			textBoxList = new List<TextBox>();

			var searchTextBox = new TextBox("SearchBox", "SearchText");
			searchTextBox.OnChangeEvent += new OnChangeEventHandler(OnChangeSearchTextBox);
			textBoxList.Add(searchTextBox);

			treeView = new ScrollTreeView("TreeViewList");
			treeView.OnChangeEvent += new OnChangeEventHandler(OnChangeScrollTreeView);
			treeView.OnCloseEvent += new OnCloseEventHandler(OnCloseScrollTreeView);
		}

		#region Sidebar

		private void OnTouchButtonTours(object sender, TouchEventArgs e)
		{

		}

		private void OnTouchButtonSeguridad(object sender, TouchEventArgs e)
		{

		}

		private void OnTouchButtonRegistrar(object sender, TouchEventArgs e)
		{

		}

		private void OnTouchButtonCambioVista(object sender, TouchEventArgs e)
		{
			UICamara.Cambiar();

			if (UICamara.Vista_1era_Persona)
			{
				UICamara.CambiarCamaraPrimeraPersona();
			}

			if (UICamara.Vista_3era_Persona)
			{
				UICamara.CambiarCamaraTerceraPersona();
			}

			OpenCloseMainMenu();
		}

		private void OnTouchButtonMain(object sender, TouchEventArgs e)
		{
			OpenCloseMainMenu();
		}

		private void OpenCloseMainMenu()
		{
			var sidebarGameObject = UIUtils.FindGUI("GUIMenuMain/Sidebar");
			float position;

			sidebarGameObject.SetActive(true);

			if (sidebarGameObject.transform.localPosition.x > 17.50)
			{
				position = -0.75f;
				State.ChangeState(eState.Navigation);
			}
			else
			{
				position = 0.75f;
				State.ChangeState(eState.GUIMenuMain);
			}

			UIAnimation.MoveBy(sidebarGameObject, new Dictionary<string, object> {
				{"x", position},{"easeType", iTween.EaseType.easeInOutExpo},{"time", 1}
			});
		}

		#endregion

		private void OnTouchModelController(object sender, TouchEventArgs e)
		{
			var location = (Localizacion)e.Mensaje;

			OpenGUIMenuBuildingDescriptor(location);
		}

		private void OnChangeSearchTextBox(object sender, ChangeEventArgs e)
		{
			var text = e.Mensaje as String;
			var textBox = sender as TextBox;

			textBox.label.Text = text;
		}

		private void OpenGUIMenuBuildingDescriptor(Localizacion location)
		{
			UIUtils.ActivateCameraLabels(false);

			var menuManager = MenuManager.GetInstance();
			menuManager.AddMenu(new GUIMenuBuildingDescriptor("GUIMenuDescriptor", location));

			var lblBuildingName = UIUtils.FindGUI(menuManager.GetCurrentMenu().GetMenuName() + "/LabelBuildingName");

			lblBuildingName.guiText.text = UIUtils.FormatStringLabel(location.Nombre, ' ', 20);

			State.ChangeState(eState.GUIMenuBuildingDescriptor);
		}

		#region ScrollTreeView

		private void OnTouchScrollTreeViewItem(object sender, TouchEventArgs e)
		{
			if (!UIScrollTreeView.isScrolling)
			{
				var button = sender as Button;
				var localizacion = (Localizacion)button.ObjectTag;

				treeView.OnClose(null);

				var textBox = UIUtils.FindGUI("GUIMenuMain/HorizontalBar/SearchBox").GetComponent<UITextBox>();

				if (textBox.GetUIKeyBoard() != null)
				{
					textBox.GetUIKeyBoard().Close();
				}

				OpenGUIMenuBuildingDescriptor(localizacion);
			}
		}

		private void OnChangeScrollTreeView(object sender, ChangeEventArgs e)
		{
			var type = e.Mensaje.GetType();

			var text = Convert.ToString(type.GetProperty("text").GetValue(e.Mensaje, null));
			var parent = (Transform)type.GetProperty("parent").GetValue(e.Mensaje, null);
			var template = (GameObject)type.GetProperty("template").GetValue(e.Mensaje, null);

			this.OpenGUIScrollTreeView(text, parent, template);
		}

		private void OnCloseScrollTreeView(object sender, CloseEventArgs e)
		{
			UIUtils.DestroyChilds("GUIMenuMain/TreeView/ScrollTreeView", true);

			DeleteScrollTreeViewItem();

			//Activando el ScrollTreeView
			UIUtils.FindGUI("GUIMenuMain/TreeView/ScrollTreeView").SetActive(false);

			UIUtils.ActivateCameraLabels(false);
		}

		private void OpenGUIScrollTreeView(string searchText, Transform Parent, GameObject Template)
		{
			//Activando el ScrollTreeView
			UIUtils.FindGUI("GUIMenuMain/TreeView/ScrollTreeView").SetActive(true);
			
			UIUtils.ActivateCameraLabels(false);

			//Obteniendo de la Base de datos
			var sqliteService = SQLiteService.GetInstance();
			var reader = sqliteService.Query(
				true,
				"SELECT nombre,idUbicacion,idLocalizacion " +
					"FROM Localizacion " +
					"WHERE nombre LIKE '%" + searchText + "%' " +
					"ORDER BY idUbicacion, idLocalizacion"
			);

			//Guardando los datos en memoria
			var textList = new List<object>();
			while (reader.Read())
			{
				var lugar = new
				{
					nombre = reader["nombre"],
					ubicacion = reader["idUbicacion"],
					localizacion = reader["idLocalizacion"],
				};

				textList.Add(lugar);
			}
			reader = null;

			//Eliminando los hijos del Tree View List
			UIUtils.DestroyChilds("GUIMenuMain/TreeView/ScrollTreeView", true);

			//Eliminando los item del tree view de la lista de botones de MenuMain
			DeleteScrollTreeViewItem();

			//Agregando los hijos al Tree View List
			for (int i = 0; i < textList.Count; i++)
			{
				GameObject item;

				//Creando el item del Tree View con world coordinates
				item = GameObject.Instantiate(Template) as GameObject;

				item.name = "ScrollTreeViewItem" + i;

				//Agregando relacion de padre (Tree View List) - hijo (item del Tree View List)
				item.transform.parent = Parent;

				//Agregando la posicion relativa del hijo con relacion al padre
				item.transform.localPosition = new Vector3(
					Template.transform.localPosition.x,
					Template.transform.localPosition.y - 0.275f * i,
					Template.transform.localPosition.z
				);

				//Agregando la escala relativa del hijo con relacion al padre
				item.transform.localScale = Template.transform.localScale;

				//Encontrando texto del un item (su hijo)
				var itemText = item.transform.FindChild("text");

				var nombre = Convert.ToString(textList[i].GetType().GetProperty("nombre").GetValue(textList[i], null));
				var ubicacion = Convert.ToInt32(textList[i].GetType().GetProperty("ubicacion").GetValue(textList[i], null));
				var localizacion = Convert.ToInt32(textList[i].GetType().GetProperty("localizacion").GetValue(textList[i], null));

				if (nombre.Length < 30)
				{
					itemText.guiText.lineSpacing = 1;
				}
				else
				{
					itemText.guiText.lineSpacing = 0;
				}

				//Si son iguales la localizacion es un nombre de un edificio
				if (ubicacion == localizacion)
				{
					itemText.guiText.text = UIUtils.FormatStringLabel(nombre, ' ', 29);
				}
				//De lo contrario esta dentro del edificio
				else
				{
					//Se agrega un padding de 5 espacios
					itemText.guiText.text = UIUtils.FormatStringLabel(nombre.PadLeft(5, ' '), ' ', 29);
				}

				//Si contiene un '\n'
				if (itemText.guiText.text.Contains('\n'))
				{
					itemText.guiText.lineSpacing = 1;
					itemText.guiText.pixelOffset = new Vector2(0, -7);
				}

				var button = new Button(item.name);
				button.OnTouchEvent += new OnTouchEventHandler(OnTouchScrollTreeViewItem);
				button.ObjectTag = new Localizacion(localizacion, ubicacion, nombre);

				buttonList.Add(button);
				treeView.ButtonCount++;
			}
		}

		private void DeleteScrollTreeViewItem()
		{
			List<Button> buttonForClear = new List<Button>();

			//Buscando las buttones
			for (int i = 0, k = 0; i < buttonList.Count; ++i)
			{
				if (k == treeView.ButtonCount)
				{
					break;
				}

				if (buttonList[i].Name.Equals("ScrollTreeViewItem" + k))
				{
					buttonForClear.Add(buttonList[i]);
					k++;
				}
			}

			//Eliminando los botones del Menu Main
			foreach (Button button in buttonForClear)
			{
				buttonList.Remove(button);
			}

			//Borrando la cantidad botones del tree view
			treeView.ButtonCount = 0;
		}

		#endregion

		#region Implement Methods

		public string GetMenuName()
		{
			return name;
		}

		public List<Button> GetButtonList()
		{
			return buttonList;
		}

		public List<TextBox> GetTextBoxList()
		{
			return textBoxList;
		}

		public ScrollTreeView GetScrollTreeView()
		{
			return treeView;
		}

		#endregion

		#endregion
	}
}
