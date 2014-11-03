using SncPucmm.Controller.Control;
using SncPucmm.Model.Domain;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.Controller.GUI;
using SncPucmm.Database;

namespace SncPucmm.Controller.GUI
{
	class MenuTourSelection : IMenu, IButton
	{
		#region Atributos

		string name;
		List<Button> buttonList;
		List<Tour> tourList;
		Transform scrollViewItemTemplate;
		Transform guiScrollView;

		#endregion

		#region Constructor

		public MenuTourSelection(string name, List<Tour> tourList)
		{
			this.name = name;
			this.tourList = tourList;

			Initializer();
		}
		
		#endregion

		#region Metodos

		public void Initializer()
		{
			scrollViewItemTemplate = Resources.Load("GUI/TourScrollViewItem") as Transform;
			guiScrollView = UIUtils.FindGUI("MenuTourSelection/Scroll View").transform as Transform;

			buttonList = new List<Button>();

			Button btnExit = new Button("ButtonExit");
			btnExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
			buttonList.Add(btnExit);

			CreateScrollView();

		}

		private void OnTouchExitButton(object sender, TouchEventArgs e)
		{
			MenuManager.GetInstance().RemoveCurrentMenu();

			UIUtils.ActivateCameraLabels(true);
			State.ChangeState(eState.Navigation);

		}

		private void CreateScrollView()
		{
			for(int i = 0; i < tourList.Count; i++)
			{
				//Creando el item del Tree View con world coordinates
				var item = GameObject.Instantiate(scrollViewItemTemplate) as Transform;

				item.name = "TourSelectionItem" + i;

				//Agregando relacion de padre (Tree View List) - hijo (item del Tree View List)
				item.parent = guiScrollView;

				//Agregando la posicion relativa del hijo con relacion al padre
				item.transform.localPosition = new Vector3(
					scrollViewItemTemplate.localPosition.x,
					scrollViewItemTemplate.localPosition.y - 65f * i,
					scrollViewItemTemplate.localPosition.z
				);

				//Agregando la escala relativa del hijo con relacion al padre
				item.localScale = scrollViewItemTemplate.localScale;

				//Encontrando texto del un item (su hijo)
				var itemText = item.FindChild("Label").GetComponent<UILabel>();

				itemText.text = tourList[i].nombreTour;

				var button = new Button(item.name);
				button.OnTouchEvent += new OnTouchEventHandler(OnTouchScrollViewItem);
				button.ObjectTag = tourList[i];

				buttonList.Add(button);
			}
		}

		private void OnTouchScrollViewItem(object sender, TouchEventArgs e)
		{
			Tour selectedTour = (sender as Button).ObjectTag as Tour;
			
			UsuarioTour usuarioTour;
			List<DetalleUsuarioTour> detalleUsuarioTourList;
			Process(selectedTour, out usuarioTour, out detalleUsuarioTourList);

			MenuManager.GetInstance().AddMenu(
				new MenuUsuarioTourSelection("MenuUsuarioDetalleTour", usuarioTour, detalleUsuarioTourList)
			);

		}

		private void Process(Tour tour, out UsuarioTour usuarioTour, out List<DetalleUsuarioTour> detalleUsuarioTourList)
		{
			var user =  ModelPoolManager.GetInstance().GetValue("Usuario") as Usuario;
			//Del tour seleccionado verificar si el usuario esta suscrito
			var result = SQLiteService.GetInstance().Query(true,
				"SELECT * FROM UsuarioTour "+
				"WHERE idUsuario = "+ user.idUsuario +" AND idTour = " + tour.idTour
			);

			UsuarioTour userTour = null;
			List<DetalleUsuarioTour> detailsList = new List<DetalleUsuarioTour>();

			//El UsuarioTour ya ha sido creado
			if (result.Read())
			{
				#region Recuperacion UsuarioTour
				
				DateTime? startDate = null, endDate = null;

				string fechaInicio = Convert.ToString(result["fechaInicio"]);
				if(!fechaInicio.Equals(string.Empty))
				{
					startDate = Convert.ToDateTime(fechaInicio);
				}

				string fechaFin = Convert.ToString(result["fechaFin"]);
				if(!fechaFin.Equals(string.Empty))
				{
					startDate = Convert.ToDateTime(fechaInicio);
				}

				userTour = new UsuarioTour()
				{
					idUsuarioTour = Convert.ToInt32(result["id"]),
					idTour = Convert.ToInt32(result["idTour"]),
					estado = Convert.ToString(result["estado"]),
					fechaInicio = startDate,
					fechaFin = endDate
				};

				#endregion

				#region Recuperacion DetalleUsuarioTour

				result = SQLiteService.GetInstance().Query(true,
					"SELECT * FROM DetalleUsuarioTour " +
					"WHERE idUsuarioTour = " + userTour.idUsuarioTour
				);

				while (result.Read())
				{
					DateTime? updatedDate = null;
					startDate = endDate = null;

					fechaInicio = Convert.ToString(result["fechaInicio"]);
					if (!fechaInicio.Equals(string.Empty))
					{
						startDate = Convert.ToDateTime(fechaInicio);
					}

					fechaFin = Convert.ToString(result["fechaLlegada"]);
					if (!fechaFin.Equals(string.Empty))
					{
						startDate = Convert.ToDateTime(fechaInicio);
					}

					string fechaActualizacion = Convert.ToString(result["fechaActualizacion"]);
					if (!fechaActualizacion.Equals(string.Empty))
					{
						updatedDate = Convert.ToDateTime(fechaInicio);
					}

					detailsList.Add(
						new DetalleUsuarioTour()
						{
							idDetalleUsuarioTour = Convert.ToInt32(result["id"]),
							idPuntoReunionTour = Convert.ToInt32(result["idPuntoReunionTour"]),
							fechaInicio = startDate,
							fechaLlegada = endDate,
							fechaFin = updatedDate
						}
					);
				}

				#endregion
			}
			//Se creará un nuevo registro de UsuarioTour y sus repectivos DetalleUsuarioTour
			else
			{
				#region Creacion UsuarioTour
				
				//BUscando el ultimo id
				result = SQLiteService.GetInstance().Query(true, 
					"SELECT MAX(id) as id FROM UsuarioTour"
				);

				int idUsuarioTour = 0;
				if(result.Read())
				{
					idUsuarioTour = Convert.ToInt32(result["id"]);
				}

				idUsuarioTour++;
				var fechaInicio = DateTime.Now;

				//Creacion del UsuarioTour

				//Insertando en la base de datos
				SQLiteService.GetInstance().Query(false, 
					"INSERT INTO UsuarioTour (id, fechaInicio, idTour, idUsuario, request) "+
					"VALUES ("+ idUsuarioTour +",'"+ fechaInicio.ToString("dd/MM/yyyy HH:mm:ss") +"',"+ tour.idTour +","+ user.idUsuario +",'create')"
				);

				//Creacion del objeto de UsuarioTour
				userTour = new UsuarioTour()
				{
					idUsuarioTour = idUsuarioTour,
					fechaInicio = fechaInicio,
					idTour = tour.idTour,
					estado = "activo",
					idUsuario = user.idUsuario,
				};

				#endregion

				#region Creacion DetalleUsuarioTourList
				
				//Obtener los puntos de reunion del tour selecionado
				result = SQLiteService.GetInstance().Query(true,
					"SELECT * FROM PuntoReunionTour " +
					"WHERE idTour = " + tour.idTour
				);

				var puntoReuionList = new List<PuntoReunionTour>();
				if (result.Read())
				{

					var puntoReunion = new PuntoReunionTour()
					{
						idPuntoReunionTour = Convert.ToInt32(result["id"]),
						secuencia = Convert.ToInt32(result["secuencia"]),
						idNodo = Convert.ToInt32(result["idNodo"]),
						idTour = Convert.ToInt32(result["idTour"])
					};

					puntoReuionList.Add(puntoReunion);
				}

				//Creacion del DetalleUsuarioTour
				
				//Buscando el ultimo id
				result = SQLiteService.GetInstance().Query(true, 
					"SELECT MAX(id) as id FROM DetalleUsuarioTour"
				);

				int idDetalleUsuarioTour = 0;
				if(result.Read())
				{
					idDetalleUsuarioTour = Convert.ToInt32(result["id"]);
				}

				foreach(var puntoReunion in puntoReuionList)
				{
					idDetalleUsuarioTour++;

					//Insertando en la base de datos
					SQLiteService.GetInstance().Query(false, 
						"INSERT INTO DetalleUsuarioTour (id, idPuntoReunionTour, idUsuarioTour) "+
						"VALUES ("+ idDetalleUsuarioTour +","+ puntoReunion.idPuntoReunionTour +","+ idUsuarioTour +")"
					);

					//Creacion del objeto de DetalleUsuarioTour
					detailsList.Add(
						new DetalleUsuarioTour() 
						{
							idDetalleUsuarioTour = idDetalleUsuarioTour,
							idPuntoReunionTour = puntoReunion.idPuntoReunionTour,
							idUsuarioTour = idUsuarioTour
						}
					);
				}

				#endregion
			}

			usuarioTour = userTour;
			detalleUsuarioTourList = detailsList;
		}

		public List<Tour> GetTourList()
		{
			return tourList;
		}

		#region Metodos Implementados

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

		~MenuTourSelection()
		{
			name = null;
			buttonList = null;
			tourList = null;
			scrollViewItemTemplate = null;
			guiScrollView = null;
		}

		#endregion        
	}
}
