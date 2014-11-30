using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using SncPucmm.Controller.Control;
using SncPucmm.Controller;
using SncPucmm.Database;
using SncPucmm.View;
using SncPucmm.Model.Domain;
using SncPucmm.Model;
using SncPucmm.Controller.Facebook;

namespace SncPucmm.Controller.GUI
{
	class MenuMain : IMenu, ITextBox, IButton, IScrollView
	{
		#region Atributos

		private string name;

		List<Button> buttonList;
		List<TextBox> textBoxList;
		ScrollView treeView;

		#endregion

		#region Constructores

		public MenuMain(string name)
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

			var ButtonUsuario = new Button("ButtonUsuario");
			ButtonUsuario.OnTouchEvent += new OnTouchEventHandler(OnTouchButtonUsuario);
			buttonList.Add(ButtonUsuario);

			var ButtonCambioVista = new Button("ButtonCambioVista");
			ButtonCambioVista.OnTouchEvent += new OnTouchEventHandler(OnTouchButtonCambioVista);
			buttonList.Add(ButtonCambioVista);

			var ButtonSeguridad = new Button("ButtonBuscarAmigos");
			ButtonSeguridad.OnTouchEvent += new OnTouchEventHandler(OnTouchButtonBuscarAmigos);
			buttonList.Add(ButtonSeguridad);

			var ButtonTours = new Button("ButtonTours");
			ButtonTours.OnTouchEvent += new OnTouchEventHandler(OnTouchButtonTours);
			buttonList.Add(ButtonTours);

			var ButtonLogout = new Button("ButtonSignOut");
			ButtonLogout.OnTouchEvent += new OnTouchEventHandler(OnTouchButtonSignOut);
			buttonList.Add(ButtonLogout);

			textBoxList = new List<TextBox>();

			var searchTextBox = new TextBox("SearchBox");
			searchTextBox.OnChangeEvent += new OnChangeEventHandler(OnChangeSearchTextBox);
			textBoxList.Add(searchTextBox);

			treeView = new ScrollView("TreeView");
			treeView.OnChangeEvent += new OnChangeEventHandler(OnChangeScrollTreeView);
			treeView.OnCloseEvent += new OnCloseEventHandler(OnCloseScrollTreeView);

			this.Update();
		}

		private void OnTouchButtonMain(object sender, TouchEventArgs e)
		{
			OpenCloseMainMenu();
		}

		private void OpenCloseMainMenu()
		{
			var sidebar = UIUtils.FindGUI("MenuMain/Sidebar");
			sidebar.SetActive(true);
			float position;

			if (sidebar.transform.localPosition.x > -130)
			{
				position = -0.60f;
				State.ChangeState(eState.Exploring);
			}
			else
			{
				position = 0.60f;
				State.ChangeState(eState.MenuMain);
			}

			UIAnimation.MoveBy(sidebar, new Dictionary<string, object> {
				{"x", position},{"easeType", iTween.EaseType.easeInOutExpo},{"time", 0.5}
			});
		}

		#region Sidebar

		private void OnTouchButtonSignOut(object sender, TouchEventArgs e)
		{
			OpenCloseMainMenu();
			FacebookController.Logout();
			ModelPoolManager.GetInstance().Remove("Usuario");
			this.Update();
		}

		private void OnTouchButtonTours(object sender, TouchEventArgs e)
		{
			OpenCloseMainMenu();
			UIUtils.ActivateCameraLabels(false);
			MenuManager.GetInstance().AddMenu(new MenuTourSelection("MenuTourSelection"));
			State.ChangeState(eState.Tour);
		}

		private void OnTouchButtonBuscarAmigos(object sender, TouchEventArgs e)
		{
			OpenCloseMainMenu();
			//MenuManager.GetInstance().AddMenu(new MenuUsuarioFollowing("MenuUsuarioFollowing"));
		}

		private void OnTouchButtonUsuario(object sender, TouchEventArgs e)
		{
			OpenCloseMainMenu();

			if (FB.IsLoggedIn && ModelPoolManager.GetInstance().Contains("Usuario"))
			{
				MenuManager.GetInstance().AddMenu(new MenuUsuarioSettings("MenuUsuarioSettings"));
			}
			else
			{
				MenuManager.GetInstance().AddMenu(new MenuSignIn("MenuSignIn"));
			}
		}

		private void OnTouchButtonCambioVista(object sender, TouchEventArgs e)
		{
			UICamaraControl.CambiarVistas();

			if (UICamaraControl.Vista_1era_Persona)
			{
				UICamaraControl.CambiarCamaraPrimeraPersona();
			}

			if (UICamaraControl.Vista_3era_Persona)
			{
				UICamaraControl.CambiarCamaraTerceraPersona();
			}

			OpenCloseMainMenu();
		}

		#endregion

		private void OnTouchModelController(object sender, TouchEventArgs e)
		{
			var node = e.Mensaje as ModelNode;

			OpenMenuBuilding(node);
		}

		private void OnChangeSearchTextBox(object sender, ChangeEventArgs e)
		{
			var text = e.Mensaje as String;
			var textBox = sender as TextBox;

			textBox.Text = text;
		}

		private void OpenMenuBuilding(ModelNode node)
		{
			UIUtils.ActivateCameraLabels(false);

			var menuManager = MenuManager.GetInstance();
			menuManager.AddMenu(new MenuBuilding("MenuBuilding", node));

			State.ChangeState(eState.MenuBuilding);
		}

		#region ScrollTreeView

		private void OnTouchScrollTreeViewItem(object sender, TouchEventArgs e)
		{
			var button = sender as Button;
			var localizacion = (ModelNode) button.ObjectTag;

			treeView.OnClose(null);

			var textBox = UIUtils.FindGUI("MenuMain/Bar/SearchBox").GetComponent<UITextBoxControl>();

			if (textBox.GetUIKeyBoard() != null)
			{
				textBox.GetUIKeyBoard().Close();
			}

			OpenMenuBuilding(localizacion);
		}

		private void OnChangeScrollTreeView(object sender, ChangeEventArgs e)
		{
			var type = e.Mensaje.GetType();

			var text = Convert.ToString(type.GetProperty("text").GetValue(e.Mensaje, null));
			var parent = (Transform)type.GetProperty("parent").GetValue(e.Mensaje, null);
			var template = (GameObject)type.GetProperty("template").GetValue(e.Mensaje, null);

			this.OpenScrollTreeView(text, parent, template);
		}

		private void OnCloseScrollTreeView(object sender, CloseEventArgs e)
		{
			UIUtils.DestroyChilds("MenuMain/TreeView/ScrollView", true);

			DeleteScrollTreeViewItem();

			//Activando el ScrollTreeView
			UIUtils.FindGUI("MenuMain/TreeView/ScrollView").SetActive(false);

			UIUtils.ActivateCameraLabels(false);

			State.ChangeState(eState.Exploring);

		}

		private void OpenScrollTreeView(string searchText, Transform Parent, GameObject Template)
		{
			//Activando el ScrollTreeView
			UIUtils.FindGUI("MenuMain/TreeView/ScrollView").SetActive(true);
			
			UIUtils.ActivateCameraLabels(false);

			var textList = new List<object>();

			//Obteniendo de la Base de datos
			using (var sqlService = new SQLiteService())
			{
				var sql = "SELECT nombre, idUbicacion, idNodo, edificio " +
						"FROM Nodo " +
						"WHERE idUbicacion is not null and nombre LIKE '%" + searchText + "%' " +
						"ORDER BY idUbicacion, idNodo";

				using (var reader = sqlService.SelectQuery(sql))
				{
					while (reader.Read())
					{
						var lugar = new
						{
							nombre = reader["nombre"],
							ubicacion = reader["idUbicacion"],
							node = reader["idNodo"],
							edificio = reader["edificio"]
						};

						textList.Add(lugar);
					}
				}
			}

			//Eliminando los hijos del Tree View List
			UIUtils.DestroyChilds("MenuMain/TreeView/ScrollView", true);

			//Eliminando los item del tree view de la lista de botones de MenuMain
			DeleteScrollTreeViewItem();

			//Agregando los hijos al Tree View List
			for (int i = 0; i < textList.Count; i++)
			{
				//Creando el item del Tree View con world coordinates
				var item = GameObject.Instantiate(Template) as GameObject;

				item.transform.name = "ScrollTreeViewItem" + i;

				//Agregando relacion de padre (Tree View List) - hijo (item del Tree View List)
				item.transform.parent = Parent;

				//Agregando la posicion relativa del hijo con relacion al padre
				item.transform.localPosition = new Vector3(
					Template.transform.localPosition.x,
					Template.transform.localPosition.y - 65f * i,
					Template.transform.localPosition.z
				);

				//Agregando la escala relativa del hijo con relacion al padre
				item.transform.localScale = Template.transform.localScale;

				//Encontrando texto del un item (su hijo)
				var itemText = item.transform.FindChild("Label").GetComponent<UILabel>();

				var nombre = Convert.ToString(textList[i].GetType().GetProperty("nombre").GetValue(textList[i], null));
				var ubicacion = Convert.ToInt32(textList[i].GetType().GetProperty("ubicacion").GetValue(textList[i], null));
				var node = Convert.ToInt32(textList[i].GetType().GetProperty("node").GetValue(textList[i], null));
				var edificio = Convert.ToInt32(textList[i].GetType().GetProperty("edificio").GetValue(textList[i], null));

				//Si son iguales la localizacion es un nombre de un edificio
				if (ubicacion == edificio)
				{
					itemText.text = nombre;
				}
				//De lo contrario esta dentro del edificio
				else
				{
					//Se agrega un padding de 5 espacios
					itemText.text = nombre.PadLeft(5, ' ');
				}

				var button = new Button(item.name);
				button.OnTouchEvent += new OnTouchEventHandler(OnTouchScrollTreeViewItem);
				button.ObjectTag = new ModelNode() { idNodo = node, idUbicacion = ubicacion, name = nombre };

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

		public void Update()
		{
			var sidebar = UIUtils.FindGUI("MenuMain/Sidebar").transform;

			if (FB.IsLoggedIn)
			{
				var buttonUsuario = sidebar.FindChild("ButtonUsuario").transform;
				buttonUsuario.FindChild("NoLogeado").gameObject.SetActive(false);
				buttonUsuario.FindChild("Logeado").gameObject.SetActive(true);

				sidebar.FindChild("ButtonTours").gameObject.SetActive(true);
				sidebar.FindChild("ButtonBuscarAmigos").gameObject.SetActive(true);
				sidebar.FindChild("ButtonSignOut").gameObject.SetActive(true);
			}
			else
			{
				var buttonUsuario = sidebar.FindChild("ButtonUsuario").transform;
				buttonUsuario.FindChild("NoLogeado").gameObject.SetActive(true);
				buttonUsuario.FindChild("Logeado").gameObject.SetActive(false);

				sidebar.FindChild("ButtonTours").gameObject.SetActive(false);
				sidebar.FindChild("ButtonBuscarAmigos").gameObject.SetActive(false);
				sidebar.FindChild("ButtonSignOut").gameObject.SetActive(false);
			}

			State.ChangeState(eState.MenuMain);
		}

		public List<Button> GetButtonList()
		{
			return buttonList;
		}

		public List<TextBox> GetTextBoxList()
		{
			return textBoxList;
		}

		public ScrollView GetScrollView()
		{
			return treeView;
		}

		#endregion

		#endregion

		#region Destructor

		~MenuMain()
		{
			this.buttonList = null;
			this.name = null;
			this.textBoxList = null;
		}

		#endregion
	}
}
