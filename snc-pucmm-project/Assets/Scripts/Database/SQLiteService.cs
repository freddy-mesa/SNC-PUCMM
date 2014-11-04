using System;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SncPucmm.Model.Domain;

namespace SncPucmm.Database
{
	public class SQLiteService
	{
		#region Atributos
		private static SQLiteService _dataBaseService;
		private IDbConnection _databaseConnection;

		private const string  DATABASE_NAME = "sncpucmm.sqlite";

		#endregion

		#region Constructor
		public SQLiteService ()
		{
			OpenDateBase();
		}

		#endregion

		#region Metodos

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <returns>The instance.</returns>
		public static SQLiteService GetInstance()
		{
			if(_dataBaseService == null)
				_dataBaseService = new SQLiteService();
			
			return _dataBaseService;
		}

		/// <summary>
		/// Opens the date base.
		/// </summary>
		private void OpenDateBase() 
		{
			string connectionString = String.Empty;
			bool createDataBase = false;

			if(Application.platform == RuntimePlatform.WindowsEditor)
			{
				connectionString = "URI=file:Assets/StreamingAssets/" + DATABASE_NAME;
			}
			else if(Application.platform == RuntimePlatform.Android)
			{
				Debug.Log("Llamando to OpenDataBase: " + DATABASE_NAME);
				Debug.Log("persistentDataPath: " + Application.persistentDataPath);
				Debug.Log("dataPath: " + Application.dataPath);

				var filePath = Application.persistentDataPath + "/" + DATABASE_NAME;

				Debug.Log("Existe ? : " + File.Exists(filePath));

				if(!File.Exists(filePath))
				{
					Debug.LogWarning("File \"" + filePath + "\" does not exist. Attempting to create from \"" + Application.dataPath + "!/assets/" + DATABASE_NAME);

					// this is the path to your StreamingAssets in android
					WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DATABASE_NAME); 

					//Esperando que termine la carga
					while(!loadDB.isDone); 
					
					// then save to Application.persistentDataPath
					File.WriteAllBytes(filePath, loadDB.bytes);
					createDataBase = true;
				} 

				connectionString = "URI=file:" + filePath;
			}

			try 
			{ 
				Debug.Log("Estableciendo conexion con : " + connectionString);
				_databaseConnection = new SqliteConnection(connectionString);
				_databaseConnection.Open();
				Debug.Log("Conexion establecida con : " + connectionString);
			}
			catch (SqliteException e)
			{
				Debug.LogException(e);
			}

			if (createDataBase)
			{
				if (Application.platform == RuntimePlatform.WindowsEditor)
				{
					DropAllModelTables();
				}

				InitializeDataBase();
			}
		}

		private void InitializeDataBase() 
		{
			InitCreateModelTables();
			InitCreateOtherTables();
			InitInsertInTables();
		}

		private void InitCreateModelTables()
		{

			//Ubicacion
			CreateTableQuery(
				"Ubicacion",
				new Dictionary<string, string>
				{
					{"idUbicacion","integer"},
					{"nombre","text"},
					{"abreviacion","text"},
				},
				new Dictionary<string, string[]>
				{
					{"PK_Ubicacion", new string[] { "idUbicacion" }}
				},
				new Dictionary<string, string[]>{ }
			);
			
			//Nodo
			CreateTableQuery(
				"Nodo",
				new Dictionary<string, string>
				{
					{"idNodo","integer"},
					{"idUbicacion","integer"},
					{"edificio","integer"},
					{"nombre","text"},
					{"activo","integer"}
				},
				new Dictionary<string, string[]>
				{
					{"PK_Nodo", new string[] { "idNodo" }}
				},
				new Dictionary<string, string[]>
				{
					{"FK_Nodo_Ubicacion", new string[] { "idUbicacion","Ubicacion","idUbicacion" }}
				}
			);

			//Coordena
			CreateTableQuery(
				"CoordenadaNodo",
				new Dictionary<string, string>
				{
					{"idCoordenadaNodo", "integer"},
					{"idNodo","integer"},
					{"latitud","real"},
					{"longitud","real"},
				},
				new Dictionary<string, string[]>
				{
					{"PK_CoordenadaNodo", new string[] { "idCoordenadaNodo" }}
				},
				new Dictionary<string, string[]>
				{
					{"FK_CoordenadaNodo_Ubicacion", new string[] { "idNodo","Nodo","idNodo" }}
				}
			);

			//Neighbor
			CreateTableQuery(
				"Neighbor",
				new Dictionary<string, string>
				{
					{"idNeighbor","integer"},
					{"idNodo","integer"},
					{"NodoName","text"},
					{"idNodoNeighbor", "integer"},
					{"NodoNeighborName","text"},
				},
				new Dictionary<string, string[]>
				{
					{"PK_Neighbor", new string[] { "idNeighbor"} }
				},
				new Dictionary<string, string[]> { } 
			);
		}

		private void InitCreateOtherTables()
		{
			CreateTableQuery(
				"Usuario",
				new Dictionary<string, string>
				{
					{"id","integer"},
					{"nombre","text"},
					{"apellido", "text"},
					{"contrasena","text"},
					{"usuario","text"},
					{"idTipoUsuario", "integer"},
					{"idCuentaUsuario","integer"}
				},
				new Dictionary<string, string[]> { },
				new Dictionary<string, string[]> { }
			);

			//Guardar los ultimos tours
			CreateTableQuery(
				"Tour",
				new Dictionary<string, string>
				{
					{"id","integer"},
					{"nombreTour","text"},
					{"fechaCreacion", "text"},
					{"fechaInicio","text"},
					{"fechaFin","text"},
					{"idUsuario","integer"}
				},
				new Dictionary<string, string[]> { },
				new Dictionary<string, string[]> { }
			);

			CreateTableQuery(
				"PuntoReunionTour",
				new Dictionary<string, string>
				{
					{"id","integer"},
					{"secuencia","integer"},
					{"idNodo","integer"},
					{"idTour","integer"}
				},
				new Dictionary<string, string[]> { },
				new Dictionary<string, string[]> { }
			);

			//Datos de Sincronizacion
			CreateTableQuery(
				"UsuarioTour",
				new Dictionary<string, string>
				{
					{"id","integer"},
					{"estado","text"},
					{"fechaInicio","text"},
					{"fechaFin","text"},
					{"idTour","integer"},
					{"idUsuario","integer"},
					{"request","text"}
				},
				new Dictionary<string, string[]> { },
				new Dictionary<string, string[]> { }
			);

			CreateTableQuery(
				"DetalleUsuarioTour",
				new Dictionary<string, string>
				{
					{"id","integer"},
					{"fechaInicio","text"},
					{"fechaLlegada","text"},
					{"fechaFin", "text"},
					{"idPuntoReunionTour","integer"},
					{"idUsuarioTour","integer"}
				},
				new Dictionary<string, string[]> { },
				new Dictionary<string, string[]> { }
			);

			CreateTableQuery(
				"UsuarioLocalizacion",
				new Dictionary<string, string>
				{
					{"id","integer"},
					{"idNodo","integer"},
					{"fechaLocalizacion","text"},
					{"idUsuario","integer"},
					{"request","text"}
				},
				new Dictionary<string, string[]> { },
				new Dictionary<string, string[]> { }
			);

		}

		private void InitInsertInTables()
		{
			#region Ubicacion
			Query(false, "Insert Into Ubicacion values (1,'Laboratorios de Ingeniería Eléctronica','MATADERO')");
			Query(false, "Insert Into Ubicacion values (2,'Talleres de Ingeniería Eléctronica y Electromecánica','TALLERIEE')");
			Query(false, "Insert Into Ubicacion values (3,'Suministro y Talleres','SUMINISTRO')");
			Query(false, "Insert Into Ubicacion values (4,'Laboratorios Generales de Ingeniería','LABING')");
			Query(false, "Insert Into Ubicacion values (5,'Departamentos de Ingeniería','DEPING')");
			Query(false, "Insert Into Ubicacion values (6,'Ciencias Básicas I','CIENBASI')");
			Query(false, "Insert Into Ubicacion values (7,'Ciencias Básicas II','CIENBASII')");
			Query(false, "Insert Into Ubicacion values (8,'Aulas 1','AULAS1')");
			Query(false, "Insert Into Ubicacion values (9,'Aulas 2','AULAS2')");
			Query(false, "Insert Into Ubicacion values (10,'Aulas 3','AULAS3')");
			Query(false, "Insert Into Ubicacion values (11,'Aulas 4','AULAS4')");
			Query(false, "Insert Into Ubicacion values (12,'Centro de Estudiantes','CENTROEST')");
			Query(false, "Insert Into Ubicacion values (13,'Biblioteca','BIBLIOTECA')");
			Query(false, "Insert Into Ubicacion values (14,'Padre Arroyo','PADARROYO')");
			Query(false, "Insert Into Ubicacion values (15,'Edificio Administrativo','ADMINISTRACION')");
			Query(false, "Insert Into Ubicacion values (16,'Ciencias de la Salud','CIENSALUD')");
			Query(false, "Insert Into Ubicacion values (17,'Salón Multiusos','MULTIUSOS')");
			Query(false, "Insert Into Ubicacion values (18,'Centro de Tecnología y Educación Permanente','TEP')");
			Query(false, "Insert Into Ubicacion values (19,'Kiosko Universitario','KIOSKO')");
			Query(false, "Insert Into Ubicacion values (20,'Arquitectura','ARQUITECTURA')");
			Query(false, "Insert Into Ubicacion values (21,'Departamento de Tecnología de la Información','DEPIT')");
			Query(false, "Insert Into Ubicacion values (22,'Capilla','CAPILLA')");
			Query(false, "Insert Into Ubicacion values (23,'Colegio','COLEGIO')");
			Query(false, "Insert Into Ubicacion values (24,'Profesores I','PROFEI')");
			Query(false, "Insert Into Ubicacion values (25,'Profesores II','PROFEII')");
			Query(false, "Insert Into Ubicacion values (26,'Teatro','TEATRO')");
			Query(false, "Insert Into Ubicacion values (27,'Postgrado','POSTGRADO')");
			Query(false, "Insert Into Ubicacion values (28,'Piscina','PISCINA')");
			Query(false, "Insert Into Ubicacion values (29,'Gimnasio','GIMNASIO')");
			#endregion

			#region Nodos
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (1,1,1,'Laboratorios de Ingeniería Electrónica',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (2,2,2,'Talleres de Ingeniería Electrónica y Electromecánica',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (3,3,3,'Suministro y Talleres',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (4,4,4,'Laboratorios Generales de Ingeniería',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (5,5,5,'Departamentos de Ingeniería',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (6,6,6,'Ciencias Básicas I',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (7,7,7,'Ciencias Básicas II',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (8,8,8,'Aulas 1',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (9,9,9,'Aulas 2',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (10,10,10,'Aulas 3',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (11,11,11,'Aulas 4',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (12,12,12,'Centro de Estudiantes',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (13,13,13,'Biblioteca',1)");
			//Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (14,14,14,'Padre Arroyo',0)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (15,15,15,'Edificio Administrativo',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (16,16,16,'Ciencias de la Salud',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (17,17,17,'Salón Multiusos',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (18,18,18,'Centro de Tecnología y Educación Permanente',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (19,19,19,'Kiosko',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (20,20,20,'Arquitectura',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (21,21,21,'Tecnología de la Información',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (22,22,22,'Capilla',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (23,23,23,'Colegio Juan XVIII',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (24,24,24,'Profesores I',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (25,25,25,'Profesores II',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (26,26,26,'Teatro Universitario',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (27,27,27,'Postgrado',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (28,28,28,'Piscina Universitaria',1)");
			Query(false, "Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (29,29,29,'Gimnasio Universitario',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (30,'Node 1',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (31,'Node 2',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (32,'Node 3',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (33,'Node 3.5',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (34,'Node 4',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (35,'Node 4.5',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (36,'Node 5',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (37,'Node 6',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (38,'Node 6.5',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (39,'Node 7',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (40,'Node 8',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (41,'Node 8.5',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (42,'Node 9',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (43,'Node 9.5',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (44,'Node 10',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (45,'Node 11',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (46,'Node 12',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (47,'Node 13',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (48,'Node 14',1)");
			//Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (49,'Node 15',0)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (50,'Node 16',1)");
			//Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (51,'Node 17',0)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (52,'Node 18',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (53,'Node 19',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (54,'Node 20',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (55,'Node 20.5',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (56,'Node 21',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (57,'Node 21.5',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (58,'Node 22',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (59,'Node 23',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (60,'Node 24',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (61,'Node 24.5',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (62,'Node 25',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (63,'Node 25.5',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (64,'Node 26',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (65,'Node 27',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (66,'Node 28',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (67,'Node 29',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (68,'Node 30',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (69,'Node 31',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (70,'Node 32',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (71,'Node 33',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (72,'Node 34',1)");
			//Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (73,'Node 35',0)");
			//Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (74,'Node 36',0)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (75,'Node 37',1)");
			//Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (76,'Node 38',0)");
			//Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (77,'Node 39',0)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (78,'Node 40',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (79,'Node 41',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (80,'Node 42',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (81,'Node 43',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (82,'Node 44',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (83,'Node 45',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (84,'Node 46',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (85,'Node 47',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (86,'Node 48',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (87,'Node 48.5',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (88,'Node 49',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (89,'Node 50',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (90,'Node 51',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (91,'Node 52',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (92,'Node 53',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (93,'Node 54',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (94,'Node 55',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (95,'Node 56',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (96,'Node 57',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (97,'Node 58',1)");
			Query(false, "Insert Into Nodo (idNodo, nombre, activo) values (98,'Node 59',1)");

			#endregion

			#region CoordenadaNodo
			Query(false,"Insert Into CoordenadaNodo values ( 1, 8, 19.442731,-70.683049)");
			Query(false,"Insert Into CoordenadaNodo values ( 2, 9, 19.443009,-70.681736)");
			Query(false,"Insert Into CoordenadaNodo values ( 3, 10, 19.441522,-70.683402)");
			Query(false,"Insert Into CoordenadaNodo values ( 4, 11, 19.443083, -70.683407)");
			Query(false,"Insert Into CoordenadaNodo values ( 5, 12, 19.443879, -70.682780)");
			Query(false,"Insert Into CoordenadaNodo values ( 6, 16, 19.443699, -70.681666)");
			Query(false,"Insert Into CoordenadaNodo values ( 7, 6, 19.442237, -70.683398)");
			Query(false,"Insert Into CoordenadaNodo values ( 8, 7, 19.442070, -70.683046)");
			Query(false,"Insert Into CoordenadaNodo values ( 9, 14, 19.442303, -70.684772)");
			Query(false,"Insert Into CoordenadaNodo values ( 10, 5, 19.441756, -70.683045)");
			Query(false,"Insert Into CoordenadaNodo values ( 11, 4, 19.441074, -70.682723)");
			Query(false,"Insert Into CoordenadaNodo values ( 12, 1, 19.440198, -70.683129)");
			Query(false,"Insert Into CoordenadaNodo values ( 13, 3, 19.440648, -70.683352)");
			Query(false,"Insert Into CoordenadaNodo values ( 14, 13, 19.443727, -70.684183)");
			Query(false,"Insert Into CoordenadaNodo values ( 15, 2, 19.440310, -70.682706)");

			Query(false,"Insert Into CoordenadaNodo values ( 30, 30, 19.440313, -70.683129)");
			Query(false,"Insert Into CoordenadaNodo values ( 31, 31, 19.440400, -70.683125)");
			Query(false,"Insert Into CoordenadaNodo values ( 32, 32, 19.440573, -70.683132)");
			Query(false,"Insert Into CoordenadaNodo values ( 33, 33, 19.440544, -70.683199)");
			Query(false,"Insert Into CoordenadaNodo values ( 34, 34, 19.440697, -70.683132)");
			Query(false,"Insert Into CoordenadaNodo values ( 35, 35, 19.440652, -70.683205)");
			Query(false,"Insert Into CoordenadaNodo values ( 36, 36, 19.440788, -70.682727)");
			Query(false,"Insert Into CoordenadaNodo values ( 37, 37, 19.440315, -70.682848)");
			Query(false,"Insert Into CoordenadaNodo values ( 38, 38, 19.440409, -70.682754)");
			Query(false,"Insert Into CoordenadaNodo values ( 39, 39, 19.440293, -70.683218)");
			Query(false,"Insert Into CoordenadaNodo values ( 40, 40, 19.440270, -70.683475)");
			Query(false,"Insert Into CoordenadaNodo values ( 41, 41, 19.440301, -70.683658)");
			Query(false,"Insert Into CoordenadaNodo values ( 42, 42, 19.440711, -70.683580)");
			Query(false,"Insert Into CoordenadaNodo values ( 43, 43, 19.440665, -70.683498)");
			Query(false,"Insert Into CoordenadaNodo values ( 44, 44, 19.441075, -70.683132)");
			Query(false,"Insert Into CoordenadaNodo values ( 45, 45, 19.441135, -70.683132)");
			Query(false,"Insert Into CoordenadaNodo values ( 46, 46, 19.441468, -70.683127)");
			Query(false,"Insert Into CoordenadaNodo values ( 47, 47, 19.441459, -70.683335)");
			Query(false,"Insert Into CoordenadaNodo values ( 48, 48, 19.441140, -70.682723)");
			//Query(false,"Insert Into CoordenadaNodo values ( 49, 49, )");
			Query(false,"Insert Into CoordenadaNodo values ( 50, 50, 19.441480, -70.682723)");
			//Query(false,"Insert Into CoordenadaNodo values ( 51, 51, )");
			Query(false,"Insert Into CoordenadaNodo values ( 52, 52, 19.441679, -70.682722)");
			Query(false,"Insert Into CoordenadaNodo values ( 53, 53, 19.441684, -70.683045)");
			Query(false,"Insert Into CoordenadaNodo values ( 54, 54, 19.441683, -70.683398)");
			Query(false,"Insert Into CoordenadaNodo values ( 55, 55, 19.441608, -70.683400)");
			Query(false,"Insert Into CoordenadaNodo values ( 56, 56, 19.441608, -70.683400)");
			Query(false,"Insert Into CoordenadaNodo values ( 57, 57, 19.441145, -70.683655)");
			Query(false,"Insert Into CoordenadaNodo values ( 58, 58, 19.441930, -70.683395)");
			Query(false,"Insert Into CoordenadaNodo values ( 59, 59, 19.441930, -70.683140)");
			Query(false,"Insert Into CoordenadaNodo values ( 60, 60, 19.441979, -70.683082)");
			Query(false,"Insert Into CoordenadaNodo values ( 61, 61, 19.442012, -70.683082)");
			Query(false,"Insert Into CoordenadaNodo values ( 62, 62, 19.441850, -70.683060)");
			Query(false,"Insert Into CoordenadaNodo values ( 63, 63, 19.441853, -70.683066)");
			Query(false,"Insert Into CoordenadaNodo values ( 64, 64, 19.442589, -70.683049)");
			Query(false,"Insert Into CoordenadaNodo values ( 65, 65, 19.442570, -70.683402)");
			Query(false,"Insert Into CoordenadaNodo values ( 66, 66, 19.442305, -70.683405)");
			Query(false,"Insert Into CoordenadaNodo values ( 67, 67, 19.442743, -70.682186)");
			Query(false,"Insert Into CoordenadaNodo values ( 68, 68, 19.442811, -70.681733)");
			Query(false,"Insert Into CoordenadaNodo values ( 69, 69, 19.443074, -70.681760)");
			Query(false,"Insert Into CoordenadaNodo values ( 70, 70, 19.442917, -70.681744)");
			Query(false,"Insert Into CoordenadaNodo values ( 71, 71, 19.443790, -70.682307)");
			Query(false,"Insert Into CoordenadaNodo values ( 72, 72, 19.443403, -70.682822)");
			//Query(false,"Insert Into CoordenadaNodo values ( 73, 73, )");
			//Query(false,"Insert Into CoordenadaNodo values ( 74, 74, )");
			Query(false,"Insert Into CoordenadaNodo values ( 75, 75, 19.443418, -70.683270)");
			//Query(false,"Insert Into CoordenadaNodo values ( 76, 76, )");
			//Query(false,"Insert Into CoordenadaNodo values ( 77, 77", )");
			Query(false,"Insert Into CoordenadaNodo values ( 78, 78, 19.443836, -70.683149)");
			Query(false,"Insert Into CoordenadaNodo values ( 79, 79, 19.443775, -70.683777)");
			Query(false,"Insert Into CoordenadaNodo values ( 80, 80, 19.443732, -70.683898)");
			Query(false,"Insert Into CoordenadaNodo values ( 81, 81, 19.443671, -70.684035)");
			Query(false,"Insert Into CoordenadaNodo values ( 82, 82, 19.443652, -70.684129)");
			Query(false,"Insert Into CoordenadaNodo values ( 83, 83, 19.443666, -70.684180)");
			Query(false,"Insert Into CoordenadaNodo values ( 84, 84, 19.443378, -70.684180)");
			Query(false,"Insert Into CoordenadaNodo values ( 85, 85, 19.443375, -70.684515)");
			Query(false,"Insert Into CoordenadaNodo values ( 86, 86, 19.443272, -70.684155)");
			Query(false,"Insert Into CoordenadaNodo values ( 87, 87, 19.442963, -70.683967)");
			Query(false,"Insert Into CoordenadaNodo values ( 88, 88, 19.443165, -70.683640)");
			Query(false,"Insert Into CoordenadaNodo values ( 89, 89, 19.443158, -70.683489)");
			Query(false,"Insert Into CoordenadaNodo values ( 90, 90, 19.443319, -70.683390)");
			Query(false,"Insert Into CoordenadaNodo values ( 91, 91, 19.443167, -70.683107)");
			Query(false,"Insert Into CoordenadaNodo values ( 92, 92, 19.443163, -70.683390)");
			Query(false,"Insert Into CoordenadaNodo values ( 93, 93, 19.442731, -70.682021)");

			//New Nodes
			Query(false,"Insert Into CoordenadaNodo values ( 94, 94, 19.440695, -70.683214)");
			Query(false,"Insert Into CoordenadaNodo values ( 95, 95, 19.440589, -70.683217)");
			Query(false,"Insert Into CoordenadaNodo values ( 96, 96, 19.440715, -70.683501)");
			Query(false,"Insert Into CoordenadaNodo values ( 97, 97, 19.440581, -70.683506)");
			Query(false,"Insert Into CoordenadaNodo values ( 98, 98, 19.440582, -70.683577)");
			#endregion

			#region Neighbor
			Query(false, "Insert Into Neighbor values (1, 30,'Node 1',      1,'Departamento de Ingenierías Electrónica y Electromecánica')");
			Query(false, "Insert Into Neighbor values (2, 30,'Node 1',     31,'Node 2')");
			Query(false, "Insert Into Neighbor values (3, 30,'Node 1',     39,'Node 7')");
			Query(false, "Insert Into Neighbor values (4, 30,'Node 1',     37,'Node 6')");
			Query(false, "Insert Into Neighbor values (5, 31,'Node 2',     32,'Node 3')");
			Query(false, "Insert Into Neighbor values (6, 31,'Node 2',     39,'Node 7')");
			Query(false, "Insert Into Neighbor values (7, 32,'Node 3',     34,'Node 4')");
			Query(false, "Insert Into Neighbor values (8, 32,'Node 3',     33,'Node 3.5')");
			Query(false, "Insert Into Neighbor values (9, 32,'Node 3',     37,'Node 6')");
			Query(false, "Insert Into Neighbor values (10, 33,'Node 3.5',   3,'Suministro y Talleres')");
			Query(false, "Insert Into Neighbor values (11, 34,'Node 4',    35,'Node 4.5')");
			Query(false, "Insert Into Neighbor values (12, 34,'Node 4',    36,'Node 5')");
			Query(false, "Insert Into Neighbor values (13, 34,'Node 4',    44,'Node 10')");
			Query(false, "Insert Into Neighbor values (14, 35,'Node 4.5',   3,'Suministro y Talleres')");
			Query(false, "Insert Into Neighbor values (15, 36,'Node 5',    38,'Node 6.5')");
			Query(false, "Insert Into Neighbor values (16, 36,'Node 5',     4,'Laboratorios Generales de Ingeniería')");
			Query(false, "Insert Into Neighbor values (17, 37,'Node 6',    38,'Node 6.5')");
			Query(false, "Insert Into Neighbor values (18, 37,'Node 6',     2,'Talleres de Ingeniería Electrónica y Electromecánica')");
			Query(false, "Insert Into Neighbor values (19, 38,'Node 6.5',   2,'Talleres de Ingeniería Electrónica y Electromecánica')");
			Query(false, "Insert Into Neighbor values (20, 39,'Node 7',    40,'Node 8')");
			Query(false, "Insert Into Neighbor values (21, 40,'Node 8',    41,'Node 8.5')");
			Query(false, "Insert Into Neighbor values (22, 42,'Node 9',    56,'Node 21')");
			Query(false, "Insert Into Neighbor values (23, 42,'Node 9',    43,'Node 9.5')");
			Query(false, "Insert Into Neighbor values (24, 43,'Node 9.5',   3,'Suministro y Talleres')");
			Query(false, "Insert Into Neighbor values (25, 44,'Node 10',   45,'Node 11')");
			Query(false, "Insert Into Neighbor values (26, 44,'Node 10',   47,'Node 13')");
			Query(false, "Insert Into Neighbor values (27, 45,'Node 11',   48,'Node 14')");
			Query(false, "Insert Into Neighbor values (28, 45,'Node 11',   46,'Node 12')");
			Query(false, "Insert Into Neighbor values (29, 46,'Node 12',   47,'Node 13')");
			Query(false, "Insert Into Neighbor values (30, 46,'Node 12',   50,'Node 16')");
			Query(false, "Insert Into Neighbor values (31, 46,'Node 12',   52,'Node 18')");
			Query(false, "Insert Into Neighbor values (32, 46,'Node 12',   53,'Node 19')");
			Query(false, "Insert Into Neighbor values (33, 47,'Node 13',   10,'Aulas 3')");
			Query(false, "Insert Into Neighbor values (34, 48,'Node 14',    4,'Laboratorios Generales de Ingeniería')");
			Query(false, "Insert Into Neighbor values (35, 48,'Node 14',   50,'Node 16')");
			Query(false, "Insert Into Neighbor values (36, 50,'Node 16',   51,'Node 17')");//Revisar estos nodos. Como que hay uno de m')ás.
			Query(false, "Insert Into Neighbor values (37, 51,'Node 17',   52,'Node 18')");
			Query(false, "Insert Into Neighbor values (38, 52,'Node 18',   93,'Node 54')");
			Query(false, "Insert Into Neighbor values (39, 53,'Node 19',   54,'Node 20')");
			Query(false, "Insert Into Neighbor values (40, 53,'Node 19',    5,'Departamentos de Ingeniería')");
			Query(false, "Insert Into Neighbor values (41, 54,'Node 20',   58,'Node 22')");
			Query(false, "Insert Into Neighbor values (42, 54,'Node 20',   55,'Node 20.5')");
			Query(false, "Insert Into Neighbor values (43, 55,'Node 20.5', 10,'Aulas 3')");
			Query(false, "Insert Into Neighbor values (44, 56,'Node 21',   57,'Node 21.5')");
			Query(false, "Insert Into Neighbor values (45, 56,'Node 21',   10,'Aulas 3')");
			Query(false, "Insert Into Neighbor values (46, 58,'Node 22',   59,'Node 23')");
			Query(false, "Insert Into Neighbor values (47, 58,'Node 22',    6,'iencias Básicas I')");
			Query(false, "Insert Into Neighbor values (48, 59,'Node 23',   60,'Node 24')");
			Query(false, "Insert Into Neighbor values (49, 59,'Node 23',   62,'Node 25')");
			Query(false, "Insert Into Neighbor values (50, 60,'Node 24',   62,'Node 25')");
			Query(false, "Insert Into Neighbor values (51, 60,'Node 24',   61,'Node 24.5')");
			Query(false, "Insert Into Neighbor values (52, 61,'Node 24.5',  7,'Ciencias Básicas II')");
			Query(false, "Insert Into Neighbor values (53, 62,'Node 25',   63,'Node 25.5')");
			Query(false, "Insert Into Neighbor values (54, 63,'Node 25.5',  5,'Departamentos de Ingeniería')");
			Query(false, "Insert Into Neighbor values (55, 64,'Node 26',    7,'iencias Básicas II')");
			Query(false, "Insert Into Neighbor values (56, 64,'Node 26',    8,'Aulas 1')");
			Query(false, "Insert Into Neighbor values (57, 64,'Node 26',   65,'Node 27')");
			Query(false, "Insert Into Neighbor values (58, 64,'Node 26',   66,'Node 28')");
			Query(false, "Insert Into Neighbor values (59, 64,'Node 26',   67,'Node 29')");
			Query(false, "Insert Into Neighbor values (60, 65,'Node 27',   66,'Node 28')");
			Query(false, "Insert Into Neighbor values (61, 65,'Node 27',   11,'Aulas 4')");
			Query(false, "Insert Into Neighbor values (62, 66,'Node 28',    6,'Ciencias Básicas I')");
			Query(false, "Insert Into Neighbor values (63, 67,'Node 29',   93,'Node 54')");
			Query(false, "Insert Into Neighbor values (64, 67,'Node 29',   72,'Node 34')");
			Query(false, "Insert Into Neighbor values (65, 68,'Node 30',   93,'Node 54')");
			Query(false, "Insert Into Neighbor values (66, 68,'Node 30',    9,'Aulas 2')");
			Query(false, "Insert Into Neighbor values (67, 69,'Node 31',    9,'Aulas 2')");
			Query(false, "Insert Into Neighbor values (68, 69,'Node 31',   16,'Ciencias de la Salud')");
			Query(false, "Insert Into Neighbor values (69, 69,'Node 31',   70,'Node 32')");
			Query(false, "Insert Into Neighbor values (70, 70,'Node 32',   71,'Node 33')");
			Query(false, "Insert Into Neighbor values (71, 70,'Node 32',   72,'Node 34')");
			Query(false, "Insert Into Neighbor values (72, 71,'Node 33',   12,'Centro de Estudiantes')");
			Query(false, "Insert Into Neighbor values (73, 71,'Node 33',   16,'Ciencias de la Salud')");
			Query(false, "Insert Into Neighbor values (74, 72,'Node 34',   73,'Node 35')");
			Query(false, "Insert Into Neighbor values (75, 73,'Node 35',   74,'Node 36')");
			Query(false, "Insert Into Neighbor values (76, 73,'Node 35',   77,'Node 39')");
			Query(false, "Insert Into Neighbor values (77, 74,'Node 36',   75,'Node 37')");
			Query(false, "Insert Into Neighbor values (78, 74,'Node 36',   76,'Node 38')");
			Query(false, "Insert Into Neighbor values (79, 74,'Node 36',   91,'Node 52')");
			Query(false, "Insert Into Neighbor values (80, 75,'Node 37',   76,'Node 38')");
			Query(false, "Insert Into Neighbor values (81, 75,'Node 37',   90,'Node 51')");
			Query(false, "Insert Into Neighbor values (82, 76,'Node 38',   77,'Node 39')");
			Query(false, "Insert Into Neighbor values (83, 77,'Node 39',   78,'Node 40')");
			Query(false, "Insert Into Neighbor values (84, 78,'Node 40',   12,'Centro de Estudiantes')");
			Query(false, "Insert Into Neighbor values (85, 78,'Node 40',   79,'Node 41')");
			Query(false, "Insert Into Neighbor values (86, 79,'Node 41',   80,'Node 42')");
			Query(false, "Insert Into Neighbor values (87, 80,'Node 42',   81,'Node 43')");
			Query(false, "Insert Into Neighbor values (88, 80,'Node 42',   90,'Node 51')");
			Query(false, "Insert Into Neighbor values (89, 81,'Node 43',   82,'Node 44')");
			Query(false, "Insert Into Neighbor values (90, 81,'Node 43',   13,'Biblioteca')");
			Query(false, "Insert Into Neighbor values (91, 82,'Node 44',   83,'Node 45')");
			Query(false, "Insert Into Neighbor values (92, 82,'Node 44',   86,'Node 48')");
			Query(false, "Insert Into Neighbor values (93, 83,'Node 45',   84,'Node 46')");
			Query(false, "Insert Into Neighbor values (94, 84,'Node 46',   85,'Node 47')");
			Query(false, "Insert Into Neighbor values (95, 86,'Node 48',   87,'Node 48.5')");
			Query(false, "Insert Into Neighbor values (96, 87,'Node 48.5', 88,'Node 49')");
			Query(false, "Insert Into Neighbor values (97, 88,'Node 49',   89,'Node 50')");
			Query(false, "Insert Into Neighbor values (98, 88,'Node 49',   90,'Node 51')");
			Query(false, "Insert Into Neighbor values (99, 89,'Node 50',   11,'Aulas 4')");
			Query(false, "Insert Into Neighbor values (100, 90,'Node 51',  92,'Node 53')");
			Query(false, "Insert Into Neighbor values (101, 91,'Node 52',  92,'Node 53')");
			Query(false, "Insert Into Neighbor values (102, 92,'Node 53',  11,'Aulas 4')");

			Query(false, "Insert Into Neighbor values (103, 94,'Node 55',    34,'Node 4')");
			Query(false, "Insert Into Neighbor values (104, 94,'Node 55',     3,'Suministro y Talleres')");
			Query(false, "Insert Into Neighbor values (105, 95,'Node 56',    32,'Node 3')");
			Query(false, "Insert Into Neighbor values (106, 95,'Node 56',     3,'Suministro y Talleres')");
			Query(false, "Insert Into Neighbor values (107, 96,'Node 57',    42,'Node 9')");
			Query(false, "Insert Into Neighbor values (108, 96,'Node 57',     3,'Suministro y Talleres')");
			Query(false, "Insert Into Neighbor values (109, 97,'Node 58',    98,'Node 59')");
			Query(false, "Insert Into Neighbor values (110, 97,'Node 58',     3,'Suministro y Talleres')");
			Query(false, "Insert Into Neighbor values (111, 98,'Node 59',    41,'Node 8.5')");
			Query(false, "Insert Into Neighbor values (112, 98,'Node 59',    42,'Node 9')");
			#endregion
		}

		/// <summary>
		/// Closes the data base.
		/// </summary>
		private void CloseDataBase() {
			_databaseConnection.Close(); 
			_databaseConnection = null; 
		}

		public IDataReader Query(bool isReturning, string sqlQuery) 
		{
			Debug.Log("Query :" + sqlQuery);

			try
			{
				IDbCommand _databaseCommand = _databaseConnection.CreateCommand();
				_databaseCommand.CommandText = sqlQuery; // fill the command
				if (isReturning) 
				{ 
					IDataReader dataReader = _databaseCommand.ExecuteReader(); // execute command which returns a reader
					return dataReader;
				}

				_databaseCommand.ExecuteNonQuery();
			}
			catch (SqliteException e)
			{
				Debug.LogException(e);
			}

			return null;
		}

		/// <summary>
		/// Creates the table query.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="columns">Columns.</param>
		/// <param name="primaryColumnsConstraint">Primary columns constraint.</param>
		/// <param name="foreignColumnsConstraint">Foreign columns constraint.</param>
		public void CreateTableQuery(string tableName,
									 Dictionary<string, string> columns,
									 Dictionary<string, string[]> primaryColumnsConstraint,
									 Dictionary<string, string[]> foreignColumnsConstraint)
		{
			Debug.Log("Creando la tabla: " + tableName);
			
			var query = "CREATE TABLE " + tableName + " (";
			foreach (var pair in columns)
			{
				query += String.Format("{0} {1},", pair.Key, pair.Value);
			}
			
			foreach (var pair in primaryColumnsConstraint)
			{
				query += String.Format("CONSTRAINT {0} PRIMARY KEY (", pair.Key);
				foreach (var column in pair.Value)
				{
					query += column + ",";
				}
				query = query.Substring(0, query.Length - 1) + "),";
			}
			
			foreach (var pair in foreignColumnsConstraint)
			{
				query += String.Format("CONSTRAINT {0} FOREIGN KEY ({1}) REFERENCES {2}({3}),", pair.Key, pair.Value[0], pair.Value[1], pair.Value[2]);
			}
			
			query = query.Substring(0, query.Length - 1) + ")";
			Debug.Log("Query to Insert: " + query);

			try
			{
				IDbCommand _databaseCommand = _databaseConnection.CreateCommand();
				Debug.Log("Command created");
				_databaseCommand.CommandText = query;
				Debug.Log("Query added to Command ");
				_databaseCommand.ExecuteNonQuery();
				Debug.Log("Executed Completed");
			}
			catch (SqliteException e)
			{
				Debug.LogException(e);
			}
		}



		private void DropAllModelTables()
		{
			Query(false, "DROP TABLE Neighbor");
			Query(false, "DROP TABLE CoordenadaNodo");
			Query(false, "DROP TABLE Nodo");
			Query(false, "DROP TABLE Ubicacion");
		}

		public void UpdateModel(JSONObject json)
		{

			//Drop to Tables
			DropAllModelTables();

			//Create Tables
			InitCreateModelTables();

			if (json.HasField("Ubicacion"))
			{
				JSONObject ubicacionJsonList = json.GetField("Ubicacion");

				foreach (var ubicacionJson in ubicacionJsonList.list)
				{
					var ubicacion = new Ubicacion(ubicacionJson);
					Query(false, "Insert Into Ubicacion values (" +ubicacion.idUbicacion + ",'" + ubicacion.nombre + "','" + ubicacion.abreviacion + "')");
				}
			}
				
			if (json.HasField("Nodo"))
			{
				JSONObject nodoJsonList = json.GetField("Nodo");

				foreach (var nodoJson in nodoJsonList.list)
				{
					var nodo = new Nodo(nodoJson);
					Query(false, "Insert Into Nodo values (" + nodo.idNodo + "," + nodo.idUbicacion + "," + nodo.edificio + ",'" + nodo.nombre + "'," + (nodo.activo.Value ? 1 : 0).ToString() + ")");
				}
			}
			
			if (json.HasField("CoordenadaNodo"))
			{
				JSONObject coordenadaNodoJsonList = json.GetField("CoordenadaNodo");

				foreach (var coordenadaNodoJson in coordenadaNodoJsonList.list)
				{
					var coordenadaNodo = new CoordenadaNodo(coordenadaNodoJson);
					Query(false, "Insert Into CoordenadaNodo values (" + coordenadaNodo.idCoordenadaNodo + "," + coordenadaNodo.nodo + "," + coordenadaNodo.latitud + "," + coordenadaNodo.longitud + ")");
				}
			}
			
			if (json.HasField("Neighbor"))
			{
				JSONObject neighborJsonList = json.GetField("Neighbor");

				foreach (var neighborJson in neighborJsonList.list)
				{
					var neighbor = new Neighbor(neighborJson);
					Query(false, "Insert Into Neighbor values (" + neighbor.idNeighbor + "," + neighbor.nodo.idNodo + ",'" + neighbor.nodo.nombre + "'," + neighbor.nodoNeighbor.idNodo + ",'" + neighbor.nodoNeighbor.nombre + "')");
				}
			}
		}

		public void UpdateTours(JSONObject json)
		{
			//Delete data from Tables
			Query(false, "DELETE FROM Tour");
			Query(false, "DELETE FROM PuntoReunionTour");
			Query(false, "DELETE FROM UsuarioTour");
			Query(false, "DELETE FROM DetalleUsuarioTour");
			Query(false, "DELETE FROM UsuarioLocalizacion");

			if (json.HasField("Tours"))
			{
				JSONObject tourJsonList = json.GetField("Tours");

				foreach (var tourJson in tourJsonList.list)
				{
					var tour = new Tour(tourJson);
					Query(false, "INSERT INTO Tour VALUES (" + 
						tour.idTour.Value +",'" + 
						tour.nombreTour +"','" + 
						(tour.fechaCreacion.HasValue ? tour.fechaCreacion.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) +"','"+
						(tour.fechaInicio.HasValue ? tour.fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) +"','"+
						(tour.fechaFin.HasValue ? tour.fechaFin.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) +"',"+
						tour.idUsuario.Value
					+")");


					if (tourJson.HasField("PuntosReunion"))
					{
						JSONObject PuntoReunionTourJsonList = json.GetField("PuntosReunion");

						foreach (var PuntoReunionTourJson in PuntoReunionTourJsonList.list)
						{
							var puntoReunionTour = new PuntoReunionTour(PuntoReunionTourJson);
							Query(false, "INSERT INTO PuntoReunionTour VALUES (" +
								puntoReunionTour.idPuntoReunionTour.Value + "," +
								puntoReunionTour.secuencia + "," +
								puntoReunionTour.idNodo.Value + "," +
								puntoReunionTour.idTour.Value
							+ ")");
						}
					}
				}
			}

			if (json.HasField("UsuariosTours"))
			{
				JSONObject usuarioTourJsonList = json.GetField("UsuariosTours");

				foreach (var usuarioTourJson in usuarioTourJsonList.list)
				{
					var usuarioTour = new UsuarioTour(usuarioTourJson);
					Query(false, "INSERT INTO UsuarioTour VALUES (" +
						usuarioTour.idUsuarioTour.Value +",'"+ 
						usuarioTour.estado +"','"+ 
						(usuarioTour.fechaInicio.HasValue ? usuarioTour.fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) +"','"+ 
						(usuarioTour.fechaFin.HasValue ? usuarioTour.fechaFin.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) +"',"+
						usuarioTour.idTour.Value +","+
						usuarioTour.idUsuario.Value +",'"+
						usuarioTour.request
					+"')");

					if (json.HasField("DetalleUsuarioTourList"))
					{
						JSONObject detalleUsuarioTourJsonList = json.GetField("DetalleUsuarioTourList");

						foreach (var detalleUsuarioTourJson in detalleUsuarioTourJsonList.list)
						{
							var detalleUsuarioTour = new DetalleUsuarioTour(detalleUsuarioTourJson);
							Query(false, "INSERT INTO DetalleUsuarioTour VALUES (" +
								detalleUsuarioTour.idDetalleUsuarioTour.Value + ",'" +
								(detalleUsuarioTour.fechaInicio.HasValue ? detalleUsuarioTour.fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) + "','" +
								(detalleUsuarioTour.fechaLlegada.HasValue ? detalleUsuarioTour.fechaLlegada.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) + "','" +
								(detalleUsuarioTour.fechaFin.HasValue ? detalleUsuarioTour.fechaFin.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) + "'," +
								detalleUsuarioTour.idPuntoReunionTour.Value + "," +
								detalleUsuarioTour.idUsuarioTour.Value
							+ "')");
						}
					}
				}
			}
		}

		public JSONObject DataSynchronization()
		{
			JSONObject json = new JSONObject(JSONObject.Type.OBJECT);

			#region UsuarioTourList

			JSONObject jsonUsuarioTourArray = new JSONObject(JSONObject.Type.ARRAY);
			
			var resultUsuarioTour = Query(true, "SELECT * FROM UsuarioTour");

			if (resultUsuarioTour.Read())
			{
				#region UsuarioTour
				
				DateTime? startDate = null, endDate = null;

				string fechaInicio = Convert.ToString(resultUsuarioTour["fechaInicio"]);
				if(!fechaInicio.Equals(string.Empty))
				{
					startDate = Convert.ToDateTime(fechaInicio);
				}

				string fechaFin = Convert.ToString(resultUsuarioTour["fechaFin"]);
				if(!fechaFin.Equals(string.Empty))
				{
					startDate = Convert.ToDateTime(fechaInicio);
				}

				var usuarioTour = new UsuarioTour()
				{
					estado = Convert.ToString(resultUsuarioTour["estadoUsuarioTour"]),
					fechaInicio = startDate,
					fechaFin = endDate,
					idTour = Convert.ToInt32(resultUsuarioTour["idTour"]),
					idUsuario = Convert.ToInt32(resultUsuarioTour["idUsuario"]),
					request = Convert.ToString(resultUsuarioTour["request"])
				};

				#region DetalleUsuarioTour
				
				int idUsuarioTour = Convert.ToInt32(resultUsuarioTour["id"]);
				var resultDetalleUsuarioTour = Query(true, "SELECT * FROM DetalleUsuarioTour WHERE idUsuarioTour = " + idUsuarioTour);

				var jsonDetalleUsuarioList = new JSONObject(JSONObject.Type.ARRAY);
				if (resultDetalleUsuarioTour.Read())
				{
					DateTime? updatedDate = null;
					startDate = endDate = null;

					fechaInicio = Convert.ToString(resultDetalleUsuarioTour["fechaInicio"]);
					if (!fechaInicio.Equals(string.Empty))
					{
						startDate = Convert.ToDateTime(fechaInicio);
					}

					fechaFin = Convert.ToString(resultDetalleUsuarioTour["fechaLlegada"]);
					if (!fechaFin.Equals(string.Empty))
					{
						startDate = Convert.ToDateTime(fechaInicio);
					}

					string fechaActualizacion = Convert.ToString(resultDetalleUsuarioTour["fechaActualizacion"]);
					if (!fechaActualizacion.Equals(string.Empty))
					{
						updatedDate = Convert.ToDateTime(fechaInicio);
					}

					var detalleUsuarioTour = new DetalleUsuarioTour()
					{
						idDetalleUsuarioTour = Convert.ToInt32(resultDetalleUsuarioTour["id"]),
						idPuntoReunionTour = Convert.ToInt32(resultDetalleUsuarioTour["idPuntoReunionTour"]),
						fechaInicio = startDate,
						fechaLlegada = endDate,
						fechaFin = updatedDate
					};

					jsonDetalleUsuarioList.Add(detalleUsuarioTour.ToJson());
				}

				#endregion

				#endregion

				var jsonUsuarioTour = new JSONObject(JSONObject.Type.OBJECT);

				jsonUsuarioTour.AddField("UsuarioTour", usuarioTour.ToJson());
				jsonUsuarioTour.AddField("DetalleUsuarioTourList", jsonDetalleUsuarioList);

				jsonUsuarioTourArray.Add(jsonUsuarioTour);
			}

			resultUsuarioTour = null;

			#endregion

			#region UsuarioLocalizacionList

			JSONObject jsonUsuarioLocalizacionArray = new JSONObject(JSONObject.Type.ARRAY);

			var resultUsuarioLocalizacion = SQLiteService.GetInstance().Query(true, "SELECT * FROM UsuarioLocalizacion");
			if (resultUsuarioLocalizacion.Read())
			{
				DateTime? localizationDate = null;

				string fechaLocalizacion = Convert.ToString(resultUsuarioTour["fechaLocalizacion"]);
				if (!fechaLocalizacion.Equals(string.Empty))
				{
					localizationDate = Convert.ToDateTime(fechaLocalizacion);
				}

				var usuarioLocalizacion = new LocalizacionUsuario() 
				{
					idNodo = Convert.ToInt32(resultUsuarioLocalizacion["idNodo"]),
					idUsuario = Convert.ToInt32(resultUsuarioLocalizacion["idUsuario"]),
					fechaLocalizacion = localizationDate
				};

				jsonUsuarioLocalizacionArray.Add(usuarioLocalizacion.ToJson());
			}

			#endregion

			json.AddField("UsuarioTourList", jsonUsuarioTourArray);
			json.AddField("UsuarioLocalizacionList", jsonUsuarioLocalizacionArray);

			return json;
		}

		#endregion

		#region Destructor
		
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="SncPucmm.Database.SQLiteService"/> is reclaimed by garbage collection.
		/// </summary>
		~SQLiteService()
		{
			CloseDataBase();
		}

		#endregion
	}
}