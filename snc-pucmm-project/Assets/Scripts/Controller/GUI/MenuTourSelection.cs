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
using System.Globalization;

namespace SncPucmm.Controller.GUI
{
	class MenuTourSelection : IMenu, IButton
	{
		#region Atributos

		string name;
		List<Button> buttonList;
		List<Tour> tourList;
		Transform scrollViewItemTemplate;

		#endregion

		#region Constructor

		public MenuTourSelection(string name)
		{
			this.name = name;
			this.tourList = GetTourList();

			Initializer();
		}
		
		#endregion

		#region Metodos

		public void Initializer()
		{
			scrollViewItemTemplate = (Resources.Load("GUI/TourScrollViewItem") as GameObject).transform;

			buttonList = new List<Button>();

			Button btnExit = new Button("ButtonExit");
			btnExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
			buttonList.Add(btnExit);

			CreateScrollView();

		}

		private void OnTouchExitButton(object sender, TouchEventArgs e)
		{
			//Delete ScrollView Childrens
			UIUtils.DestroyChilds("MenuTourSelection/ScrollView", true);

			//Remove Menu
			MenuManager.GetInstance().RemoveCurrentMenu();

			UIUtils.ActivateCameraLabels(true);
			State.ChangeState(eState.Exploring);

		}

		private void CreateScrollView()
		{
			Transform guiScrollView = UIUtils.FindGUI("MenuTourSelection/ScrollView").transform as Transform;
			for(int i = 0; i < tourList.Count; i++)
			{
				//Creando el item del Tree View con world coordinates
				var item = (GameObject.Instantiate(scrollViewItemTemplate.gameObject) as GameObject).transform;

				item.name = "TourSelectionItem" + i;

				//Agregando relacion de padre (Tree View List) - hijo (item del Tree View List)
				item.parent = guiScrollView;

				//Agregando la posicion relativa del hijo con relacion al padre
				item.transform.localPosition = new Vector3(
					scrollViewItemTemplate.localPosition.x,
					scrollViewItemTemplate.localPosition.y - 60f * i,
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
				new MenuUsuarioTourSelection("MenuUsuarioTourSelection", selectedTour.nombreTour, usuarioTour, detalleUsuarioTourList)
			);

		}

		private void Process(Tour tour, out UsuarioTour usuarioTour, out List<DetalleUsuarioTour> detalleUsuarioTourList)
		{
			UsuarioTour userTour = null;
			List<DetalleUsuarioTour> detailsList = new List<DetalleUsuarioTour>();

			using (var sqlService = new SQLiteService())
			{
				var user = ModelPoolManager.GetInstance().GetValue("Usuario") as Usuario;

				//Del tour seleccionado verificar si el usuario esta suscrito
				var sqlQuery = "SELECT * FROM UsuarioTour WHERE idUsuarioFacebook = '"+ user.idUsuarioFacebook +"' AND idTour = " + tour.idTour;
				using (var resultUsuarioTour = sqlService.SelectQuery(sqlQuery))
				{
					//El UsuarioTour ya ha sido creado
					if (resultUsuarioTour.HasRows == true)
					{
						#region Recuperacion UsuarioTour

						DateTime? startDate = null, endDate = null;
						DateTime temp;

						var obj = resultUsuarioTour["fechaInicio"];
						if (obj != null)
						{
							string fechaInicio = Convert.ToString(obj);
							if (DateTime.TryParseExact(fechaInicio, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
							{
								startDate = temp;
							}
						}

						obj = resultUsuarioTour["fechaFin"];
						if (obj != null)
						{
							string fechaFin = Convert.ToString(obj);
							if (DateTime.TryParseExact(fechaFin, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
							{
								endDate = temp;
							}
						}

						userTour = new UsuarioTour()
						{
							idUsuarioFacebook = user.idUsuarioFacebook,
							idUsuarioTour = Convert.ToInt32(resultUsuarioTour["id"]),
							idTour = Convert.ToInt32(resultUsuarioTour["idTour"]),
							estado = Convert.ToString(resultUsuarioTour["estado"]),
							fechaInicio = startDate,
							fechaFin = endDate
						};

						#endregion

						#region Recuperacion DetalleUsuarioTour

						var sql = "SELECT * FROM DetalleUsuarioTour WHERE idUsuarioTour = " + userTour.idUsuarioTour;

						using (var resultDetalleUsuarioTour = sqlService.SelectQuery(sql))
						{
							while (resultDetalleUsuarioTour.Read())
							{
								DateTime? updatedDate = null;
								startDate = endDate = null;

								obj = resultDetalleUsuarioTour["fechaInicio"];
								if (obj != null)
								{
									var fechaInicio = Convert.ToString(obj);
									if (DateTime.TryParseExact(fechaInicio, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
									{
										startDate = temp;
									}
								}

								obj = resultDetalleUsuarioTour["fechaLlegada"];
								if (obj != null)
								{
									var fechaFin = Convert.ToString(obj);
									if (DateTime.TryParseExact(fechaFin, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
									{
										endDate = temp;
									}
								}

								obj = resultDetalleUsuarioTour["fechaFin"];
								if (obj != null)
								{
									var fechaActualizacion = Convert.ToString(obj);
									if (DateTime.TryParseExact(fechaActualizacion, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
									{
										updatedDate = temp;
									}
								}

								detailsList.Add(
									new DetalleUsuarioTour()
									{
										idDetalleUsuarioTour = Convert.ToInt32(resultDetalleUsuarioTour["id"]),
										idPuntoReunionTour = Convert.ToInt32(resultDetalleUsuarioTour["idPuntoReunionTour"]),
										idUsuarioTour = userTour.idUsuarioTour,
										fechaInicio = startDate,
										fechaLlegada = endDate,
										fechaFin = updatedDate
									}
								);
							}
						}

						#endregion
					}
					//Se creará un nuevo registro de UsuarioTour y sus repectivos DetalleUsuarioTour
					else
					{
						#region Creacion UsuarioTour

						//Buscando el ultimo id
						int idUsuarioTour = 0;

						using (var result = sqlService.SelectQuery("SELECT MAX(id) as id FROM UsuarioTour"))
						{
							while (result.Read())
							{
								int.TryParse(Convert.ToString(result["id"]), out idUsuarioTour);
							}
						}

						idUsuarioTour++;
						var fechaInicio = DateTime.Now;

						//Creacion del UsuarioTour

						//Insertando en la base de datos
						sqlService.TransactionalQuery(
							"INSERT INTO UsuarioTour (id, idTour, idUsuarioFacebook, fechaInicio, request) " +
							"VALUES (" + idUsuarioTour + "," + tour.idTour + ",'" + user.idUsuarioFacebook + "','" + fechaInicio.ToString("dd/MM/yyyy HH:mm:ss") + "','create')"
						);

						//sqlService.TransactionalQuery(
						//    "INSERT INTO UsuarioTour (id, idTour, fechaInicio, request) " +
						//    "VALUES (" + idUsuarioTour + "," + tour.idTour + ",'" + fechaInicio.ToString("dd/MM/yyyy HH:mm:ss") + "','create')"
						//);

						//Creacion del objeto de UsuarioTour
						userTour = new UsuarioTour()
						{
							idUsuarioTour = idUsuarioTour,
							fechaInicio = fechaInicio,
							idTour = tour.idTour,
							estado = "activo",
							idUsuarioFacebook = user.idUsuarioFacebook
						};

						//userTour = new UsuarioTour()
						//{
						//    idUsuarioTour = idUsuarioTour,
						//    fechaInicio = fechaInicio,
						//    idTour = tour.idTour,
						//    estado = "activo",
						//};

						#endregion

						#region Creacion DetalleUsuarioTourList

						//Obtener los puntos de reunion del tour selecionado
						var puntoReuionList = new List<PuntoReunionTour>();

						using (var result = sqlService.SelectQuery("SELECT * FROM PuntoReunionTour WHERE idTour = " + tour.idTour))
						{
							while (result.Read())
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
						}

						//Creacion del DetalleUsuarioTour

						//Buscando el ultimo id
						int idDetalleUsuarioTour = 0;

						using (var result = sqlService.SelectQuery("SELECT MAX(id) as id FROM DetalleUsuarioTour"))
						{
							if (result.Read())
							{
								int.TryParse(Convert.ToString(result["id"]), out idDetalleUsuarioTour);
							}
						}

						StringBuilder sqlBuilder = new StringBuilder();

						foreach (var puntoReunion in puntoReuionList)
						{
							idDetalleUsuarioTour++;

							//Insertando en la base de datos
							sqlBuilder.Append(
								"INSERT INTO DetalleUsuarioTour (id, idPuntoReunionTour, idUsuarioTour) " +
								"VALUES (" + idDetalleUsuarioTour + "," + puntoReunion.idPuntoReunionTour + "," + idUsuarioTour + ");"
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

						sqlService.TransactionalQuery(sqlBuilder.ToString());

						#endregion
					}
				}
			}

			usuarioTour = userTour;
			detalleUsuarioTourList = detailsList;
		}

		private List<Tour> GetTourList()
		{
			List<Tour> tourList = new List<Tour>();

			using (var sqlService = new SQLiteService())
			{
				using (var result = sqlService.SelectQuery("SELECT id, nombreTour FROM Tour"))
				{
					while (result.Read())
					{
						tourList.Add(new Tour()
						{
							idTour = Convert.ToInt32(result["id"]),
							nombreTour = Convert.ToString(result["nombreTour"])
						});
					}
				}
			}

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

		public void Update()
		{

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
		}

		#endregion        
	}
}
