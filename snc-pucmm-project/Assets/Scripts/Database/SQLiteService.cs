using System;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using SncPucmm.Model.Domain;
using System.Globalization;
using System.Text;

namespace SncPucmm.Database
{
	public class SQLiteService : IDisposable
	{
		#region Atributos

		private SqliteConnection _databaseConnection;

		private const string  DATABASE_NAME = "sncpucmm.sqlite";

		public static bool IsCreatingDatabase;

		#endregion

		#region Constructor
		
		public SQLiteService()
		{
			OpenDateBaseConnection();
		}

		static SQLiteService()
		{
			IsCreatingDatabase = true;
		}

		#endregion

		#region Metodos

		private void OpenDateBaseConnection() 
		{
			string connectionString = String.Empty;

			if(Application.platform == RuntimePlatform.WindowsEditor)
			{
				connectionString = "URI=file:Assets/StreamingAssets/" + DATABASE_NAME + ";version=3";
			}
			else if(Application.platform == RuntimePlatform.Android)
			{
				//Debug.Log("Llamando to OpenDataBase: " + DATABASE_NAME);
				//Debug.Log("persistentDataPath: " + Application.persistentDataPath);
				//Debug.Log("dataPath: " + Application.dataPath);

				var filePath = Application.persistentDataPath + "/" + DATABASE_NAME;

				//Debug.Log("Existe ? : " + File.Exists(filePath));

				if(!File.Exists(filePath))
				{
					Debug.LogWarning("File \"" + filePath + "\" does not exist. Attempting to create from \"" + Application.dataPath + "!/assets/" + DATABASE_NAME);

					// this is the path to your StreamingAssets in android
					WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DATABASE_NAME); 

					//Esperando que termine la carga
					while(!loadDB.isDone); 
					
					// then save to Application.persistentDataPath
					File.WriteAllBytes(filePath, loadDB.bytes);
					IsCreatingDatabase = true;
				}

				connectionString = "URI=file:" + filePath + ";version=3";
			}

			try 
			{ 
				//Debug.Log("Estableciendo conexion con : " + connectionString);
				_databaseConnection = new SqliteConnection(connectionString);
				_databaseConnection.Open();
				//Debug.Log("Conexion establecida con : " + connectionString);
			}
			catch (SqliteException e)
			{
				Debug.LogException(e);
			}

			if (IsCreatingDatabase)
			{
				if (Application.platform == RuntimePlatform.WindowsEditor)
				{
					DropAllModelTables();
				}

				InitializeDataBase();

				IsCreatingDatabase = false;
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
			StringBuilder sqlBuilder = new StringBuilder();
			//Ubicacion
			sqlBuilder.Append(CreateTableQuery(
				"Ubicacion",
				new Dictionary<string, string>
				{
					{"idUbicacion","integer"},
					{"nombre","text"},
					{"abreviacion","text"},
					{"cantidadPlantas","integer"},
				},
				new Dictionary<string, string[]>
				{
					{"PK_Ubicacion", new string[] { "idUbicacion" }}
				},
				new Dictionary<string, string[]>{ }
			));
			
			//Nodo
			sqlBuilder.Append(CreateTableQuery(
				"Nodo",
				new Dictionary<string, string>
				{
					{"idNodo","integer"},
					{"idUbicacion","integer"},
					{"edificio","integer"},
					{"nombre","text"},
					{"activo","integer"},
					{"planta", "integer"}
				},
				new Dictionary<string, string[]>
				{
					{"PK_Nodo", new string[] { "idNodo" }}
				},
				new Dictionary<string, string[]>
				{
					{"FK_Nodo_Ubicacion", new string[] { "idUbicacion","Ubicacion","idUbicacion" }}
				}
			));

			//Coordena
			sqlBuilder.Append(CreateTableQuery(
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
			));

			//Neighbor
			sqlBuilder.Append(CreateTableQuery(
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
			));

			TransactionalQuery(sqlBuilder.ToString());
		}

		private void InitCreateOtherTables()
		{
			StringBuilder sqlBuilder = new StringBuilder();

			sqlBuilder.Append(CreateTableQuery(
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
			));

			sqlBuilder.Append(CreateTableQuery(
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
			));

			sqlBuilder.Append(CreateTableQuery(
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
			));

			//Datos de Sincronizacion
			sqlBuilder.Append(CreateTableQuery(
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
			));

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

			sqlBuilder.Append(CreateTableQuery(
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
			));

			TransactionalQuery(sqlBuilder.ToString());
		}

		private void InitInsertInTables()
		{
			StringBuilder sqlBuilder;

			#region Ubicacion

			sqlBuilder = new StringBuilder();

			sqlBuilder.Append("Insert Into Ubicacion values (1,'Laboratorios de Ingeniería Eléctronica','MATADERO', 1);");
			sqlBuilder.Append("Insert Into Ubicacion values (2,'Talleres de Ingeniería Eléctronica y Electromecánica','TALLERIEE', 1);");
			sqlBuilder.Append("Insert Into Ubicacion values (3,'Suministro y Talleres','SUMINISTRO', 1);");
			sqlBuilder.Append("Insert Into Ubicacion values (4,'Laboratorios Generales de Ingeniería','LABING', 2);");
			sqlBuilder.Append("Insert Into Ubicacion values (5,'Departamentos de Ingeniería','DEPING', 2);");
			sqlBuilder.Append("Insert Into Ubicacion values (6,'Ciencias Básicas I','CIENBASI', 2);");
			sqlBuilder.Append("Insert Into Ubicacion values (7,'Ciencias Básicas II','CIENBASII', 2);");
			sqlBuilder.Append("Insert Into Ubicacion values (8,'Aulas 1','AULAS1', 3);");
			sqlBuilder.Append("Insert Into Ubicacion values (9,'Aulas 2','AULAS2', 3);");
			sqlBuilder.Append("Insert Into Ubicacion values (10,'Aulas 3','AULAS3', 3);");
			sqlBuilder.Append("Insert Into Ubicacion values (11,'Aulas 4','AULAS4', 3);");
			sqlBuilder.Append("Insert Into Ubicacion values (12,'Centro de Estudiantes','CENTROEST', 3);");
			sqlBuilder.Append("Insert Into Ubicacion values (13,'Biblioteca','BIBLIOTECA', 3);");
			sqlBuilder.Append("Insert Into Ubicacion values (14,'Padre Arroyo','PADARROYO', 2);");
			sqlBuilder.Append("Insert Into Ubicacion values (15,'Edificio Administrativo','ADMINISTRACION', 7);");
			sqlBuilder.Append("Insert Into Ubicacion values (16,'Ciencias de la Salud','CIENSALUD', 3);");
			sqlBuilder.Append("Insert Into Ubicacion values (17,'Salón Multiusos','MULTIUSOS', 1);");
			sqlBuilder.Append("Insert Into Ubicacion values (18,'Centro de Tecnología y Educación Permanente','TEP', 3);");
			sqlBuilder.Append("Insert Into Ubicacion values (19,'Kiosko Universitario','KIOSKO', 1);");
			sqlBuilder.Append("Insert Into Ubicacion values (20,'Arquitectura','ARQUITECTURA', 2);");
			sqlBuilder.Append("Insert Into Ubicacion values (21,'Departamento de Tecnología de la Información','DEPIT', 1);");
			sqlBuilder.Append("Insert Into Ubicacion values (22,'Capilla','CAPILLA', 1);");
			sqlBuilder.Append("Insert Into Ubicacion values (23,'Colegio','COLEGIO', 2);");
			sqlBuilder.Append("Insert Into Ubicacion values (24,'Profesores I','PROFEI', 3);");
			sqlBuilder.Append("Insert Into Ubicacion values (25,'Profesores II','PROFEII', 2);");
			sqlBuilder.Append("Insert Into Ubicacion values (26,'Teatro','TEATRO', 2);");
			sqlBuilder.Append("Insert Into Ubicacion values (27,'Postgrado','POSTGRADO', 2);");
			sqlBuilder.Append("Insert Into Ubicacion values (28,'Piscina','PISCINA', 1);");
			sqlBuilder.Append("Insert Into Ubicacion values (29,'Gimnasio','GIMNASIO', 3);");

			TransactionalQuery(sqlBuilder.ToString());

			#endregion

			#region Nodos

			sqlBuilder = new StringBuilder();

			#region Edificaciones

			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (1,1,1,'Laboratorios de Ingeniería Electrónica',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (2,2,2,'Talleres de Ingeniería Electrónica y Electromecánica',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (3,3,3,'Suministro y Talleres',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (4,4,4,'Laboratorios Generales de Ingeniería',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (5,5,5,'Departamentos de Ingeniería',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (6,6,6,'Ciencias Básicas I',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (7,7,7,'Ciencias Básicas II',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (8,8,8,'Aulas 1',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (9,9,9,'Aulas 2',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (10,10,10,'Aulas 3',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (11,11,11,'Aulas 4',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (12,12,12,'Centro de Estudiantes',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (13,13,13,'Biblioteca',1);");
			//sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (14,14,14,'Padre Arroyo',0);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (15,15,15,'Edificio Administrativo',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (16,16,16,'Ciencias de la Salud',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (17,17,17,'Salón Multiusos',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (18,18,18,'Centro de Tecnología y Educación Permanente',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (19,19,19,'Kiosko',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (20,20,20,'Arquitectura',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (21,21,21,'Tecnología de la Información',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (22,22,22,'Capilla',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (23,23,23,'Colegio Juan XVIII',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (24,24,24,'Profesores I',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (25,25,25,'Profesores II',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (26,26,26,'Teatro Universitario',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (27,27,27,'Postgrado',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (28,28,28,'Piscina Universitaria',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, edificio, nombre, activo) values (29,29,29,'Gimnasio Universitario',1);");

			#endregion

			#region Caminos en el Campus

			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (30,'Node 1',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (31,'Node 2',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (32,'Node 3',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (33,'Node 3.5',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (34,'Node 4',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (35,'Node 4.5',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (36,'Node 5',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (37,'Node 6',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (38,'Node 6.5',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (39,'Node 7',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (40,'Node 8',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (41,'Node 8.5',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (42,'Node 9',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (43,'Node 9.5',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (44,'Node 10',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (45,'Node 11',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (46,'Node 12',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (47,'Node 13',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (48,'Node 14',1);");
			//sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (49,'Node 15',0);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (50,'Node 16',1);");
			//sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (51,'Node 17',0);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (52,'Node 18',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (53,'Node 19',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (54,'Node 20',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (55,'Node 20.5',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (56,'Node 21',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (57,'Node 21.5',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (58,'Node 22',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (59,'Node 23',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (60,'Node 24',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (61,'Node 24.5',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (62,'Node 25',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (63,'Node 25.5',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (64,'Node 26',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (65,'Node 27',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (66,'Node 28',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (67,'Node 29',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (68,'Node 30',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (69,'Node 31',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (70,'Node 32',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (71,'Node 33',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (72,'Node 34',1);");
			//sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (73,'Node 35',0);");
			//sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (74,'Node 36',0);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (75,'Node 37',1);");
			//sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (76,'Node 38',0);");
			//sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (77,'Node 39',0);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (78,'Node 40',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (79,'Node 41',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (80,'Node 42',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (81,'Node 43',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (82,'Node 44',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (83,'Node 45',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (84,'Node 46',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (85,'Node 47',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (86,'Node 48',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (87,'Node 48.5',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (88,'Node 49',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (89,'Node 50',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (90,'Node 51',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (91,'Node 52',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (92,'Node 53',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (93,'Node 54',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (94,'Node 55',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (95,'Node 56',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (96,'Node 57',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (97,'Node 58',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (98,'Node 59',1);");

			//Nuevos Nodos para Aulas 3, conexion desde los nodo 13 al 21
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (99, 'Node 60',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (100,'Node 61',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (101,'Node 62',1);");

		   //Nuevos Nodos para Aulas 4:
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (102, 'Node 63',1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (103, 'Node 64',1);");

			#endregion

			TransactionalQuery(sqlBuilder.ToString());

			#region Aulas 3 -> 300

			sqlBuilder = new StringBuilder();

			#region Nodos A3
			
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (300, 10,'A3-11',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (301, 10,'A3-12',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (302, 10,'A3-14',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (303, 10,'A3-15',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (304, 10,'A3-16',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (305, 10,'A3-17',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (306, 10,'A3-21',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (307, 10,'A3-22',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (308, 10,'A3-23',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (309, 10,'A3-24',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (310, 10,'A3-25',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (311, 10,'Salon de Dibujo I',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (312, 10,'A3-31',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (313, 10,'A3-32',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (314, 10,'A3-33',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (315, 10,'A3-34',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (316, 10,'A3-35',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (317, 10,'Salon de Dibujo II',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (318, 10,'A3-Bedel',1,1);");

			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (319, 10, 'AULAS3_Nodo1', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (320, 10, 'AULAS3_Nodo2', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (321, 10, 'AULAS3_Nodo3', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (322, 10, 'AULAS3_Nodo4', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (323, 10, 'AULAS3_Nodo5', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (324, 10, 'AULAS3_Nodo6', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (325, 10, 'AULAS3_Nodo7', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (326, 10, 'AULAS3_Nodo8', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (327, 10, 'AULAS3_Nodo9', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (328, 10, 'AULAS3_Nodo10', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (329, 10, 'AULAS3_Nodo11', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (330, 10, 'AULAS3_Nodo12', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (331, 10, 'AULAS3_Nodo13', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (332, 10, 'AULAS3_Nodo14', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (333, 10, 'AULAS3_Nodo15', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (334, 10, 'AULAS3_Nodo16', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (335, 10, 'AULAS3_Nodo17', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (336, 10, 'AULAS3_Nodo18', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (337, 10, 'AULAS3_Nodo19', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (338, 10, 'AULAS3_Nodo20', 1, 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (339, 10, 'AULAS3_Nodo21', 1, 1);");

			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (340, 10, 'AULAS3_Nodo22', 1, 2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (341, 10, 'AULAS3_Nodo23', 1, 2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (342, 10, 'AULAS3_Nodo24', 1, 2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (343, 10, 'AULAS3_Nodo25', 1, 2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (344, 10, 'AULAS3_Nodo26', 1, 2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (345, 10, 'AULAS3_Nodo27', 1, 2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (346, 10, 'AULAS3_Nodo28', 1, 2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (347, 10, 'AULAS3_Nodo29', 1, 2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (348, 10, 'AULAS3_Nodo30', 1, 2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (349, 10, 'AULAS3_Nodo31', 1, 2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (350, 10, 'AULAS3_Nodo32', 1, 2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (351, 10, 'AULAS3_Nodo33', 1, 2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (352, 10, 'AULAS3_Nodo34', 1, 2);");

			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (353, 10, 'AULAS3_Nodo35', 1, 3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (354, 10, 'AULAS3_Nodo36', 1, 3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (355, 10, 'AULAS3_Nodo37', 1, 3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (356, 10, 'AULAS3_Nodo38', 1, 3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (357, 10, 'AULAS3_Nodo39', 1, 3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (358, 10, 'AULAS3_Nodo40', 1, 3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (359, 10, 'AULAS3_Nodo41', 1, 3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (360, 10, 'AULAS3_Nodo42', 1, 3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (361, 10, 'AULAS3_Nodo43', 1, 3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (362, 10, 'AULAS3_Nodo44', 1, 3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (363, 10, 'AULAS3_Nodo45', 1, 3);");

			//Aulas
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 300, 300, -246.5, -91.3);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 301, 301, -245.8, -80.4);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 302, 302, -246.1, -66.3);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 303, 303, -247.3, -61.9);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 304, 304, -257.1, -78.3);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 305, 305, -268.8, -80.4);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 306, 306, -246.3, -91.6);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 307, 307, -245.4, -80.6);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 308, 308, -245.4, -73.7);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 309, 309, -245.8, -66.21);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 310, 310, -247, -61.9);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 311, 311, -256.64, -78.48);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 312, 312, -246.5, -91.3);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 313, 313, -245.6, -80.22);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 314, 314, -245.6, -73.78);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 315, 315, -245.6, -72.9);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 316, 316, -247, -61.94);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 317, 317, -256.96, -78.56);");

			//Nodos

			//Planta 1
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 318, 318, -246.4, -70.5);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 319, 319, -250.7, -89.8);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 320, 320, -248.1, -89.8);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 321, 321, -248.4, -88.1);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 322, 322, -247.45, -84);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 323, 323, -247.3, -79.2);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 324, 324, -247.23, -78);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 325, 325, -247.23, -77);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 326, 326, -247.23, -75.6);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 327, 327, -247.66, -73.1);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 328, 328, -247.87, -68.75);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 329, 329, -247.87, -64.97);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 330, 330, -242.16, -76.31);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 331, 331, -248.61, -77.75);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 332, 332, -252.71, -77.75);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 333, 333, -248.84, -75.46);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 334, 334, -252.84, -75.46);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 335, 335, -236.06, -77.0);");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 336, 336, , );");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 337, 337, -268.95, -81.74);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 338, 338, -249.01, -79.18);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 339, 339, -247.1, -89);");

			//Planta 2
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 340, 340, -252.3, -79.15);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 341, 341, -255.3, -79.3);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 342, 342, -255.3, -77.8);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 343, 343, -252, -77.76);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 344, 344, -249.2, -77.5);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 345, 345, -249.2, -79.4);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 346, 346, -247, -78.95);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 347, 347, -247, -80.14);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 348, 348, -246.75, -85.32);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 349, 349, -247, -77.61);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 350, 350, -247, -75.6);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 351, 351, -247.1, -70.23);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 352, 352, -247.35, -64.3);");

			//Planta 3
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 353, 353, -252, -79.25);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 354, 354, -255, -79.25);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 355, 355, -255, -77.87);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 356, 356, -252.2, -77.87);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 357, 357, -249, -77.75);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 358, 358, -247.23, -77.75);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 359, 359, -247.23, -79.84);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 360, 360, -246.87, -85.57);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 361, 361, -247.3, -75.27);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 362, 362, -247.3, -73.33);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 363, 363, -247.3, -68);");

			#endregion

			#region Planta1

			sqlBuilder.Append("Insert Into Neighbor values (300, 319,'AULAS3_Nodo1', 56,'Node 21');");
			sqlBuilder.Append("Insert Into Neighbor values (301, 319,'AULAS3_Nodo1', 320,'AULAS3_Nodo2');");
			sqlBuilder.Append("Insert Into Neighbor values (302, 320,'AULAS3_Nodo2', 300,'A3-11');");
			sqlBuilder.Append("Insert Into Neighbor values (303, 319,'AULAS3_Nodo1', 321,'AULAS3_Nodo3');");
			sqlBuilder.Append("Insert Into Neighbor values (304, 321,'AULAS3_Nodo3', 322,'AULAS3_Nodo4');");
			sqlBuilder.Append("Insert Into Neighbor values (305, 322,'AULAS3_Nodo4', 323,'AULAS3_Nodo5');");
			sqlBuilder.Append("Insert Into Neighbor values (306, 322,'AULAS3_Nodo4', 301,'A3-12');");
			sqlBuilder.Append("Insert Into Neighbor values (307, 323,'AULAS3_Nodo5', 301,'A3-12');");
			sqlBuilder.Append("Insert Into Neighbor values (308, 323,'AULAS3_Nodo5', 324,'AULAS3_Nodo6');");
			sqlBuilder.Append("Insert Into Neighbor values (309, 324,'AULAS3_Nodo6', 325,'AULAS3_Nodo7');");
			sqlBuilder.Append("Insert Into Neighbor values (310, 325,'AULAS3_Nodo7', 326,'AULAS3_Nodo8');");
			sqlBuilder.Append("Insert Into Neighbor values (311, 326,'AULAS3_Nodo8', 327,'AULAS3_Nodo9');");
			sqlBuilder.Append("Insert Into Neighbor values (312, 327,'AULAS3_Nodo9', 328,'AULAS3_Nodo10');");
			sqlBuilder.Append("Insert Into Neighbor values (313, 327,'AULAS3_Nodo9', 318,'A3-Bedel');");
			sqlBuilder.Append("Insert Into Neighbor values (314, 328,'AULAS3_Nodo10', 329,'AULAS3_Nodo11');");
			sqlBuilder.Append("Insert Into Neighbor values (315, 328,'AULAS3_Nodo10', 302,'A3-14');");
			sqlBuilder.Append("Insert Into Neighbor values (316, 329,'AULAS3_Nodo11', 303,'A3-15');");
			
			//Nodo Aulas 3
			sqlBuilder.Append("Insert Into Neighbor values (317, 10,'Aulas 3', 324,'AULAS3_Nodo6');");
			sqlBuilder.Append("Insert Into Neighbor values (318, 10,'Aulas 3', 326,'AULAS3_Nodo8');");
			sqlBuilder.Append("Insert Into Neighbor values (319, 10,'Aulas 3', 330,'AULAS3_Nodo12');");

			sqlBuilder.Append("Insert Into Neighbor values (320, 331,'AULAS3_Nodo13', 324,'AULAS3_Nodo6');");
			sqlBuilder.Append("Insert Into Neighbor values (321, 331,'AULAS3_Nodo13', 332,'AULAS3_Nodo14');");
			sqlBuilder.Append("Insert Into Neighbor values (322, 332,'AULAS3_Nodo14', 100,'Node 61');");

			sqlBuilder.Append("Insert Into Neighbor values (323, 333,'AULAS3_Nodo15', 326,'AULAS3_Nodo8');");
			sqlBuilder.Append("Insert Into Neighbor values (324, 333,'AULAS3_Nodo15', 334,'AULAS3_Nodo16');");
			sqlBuilder.Append("Insert Into Neighbor values (325, 334,'AULAS3_Nodo16', 99,'Node 99');");

			sqlBuilder.Append("Insert Into Neighbor values (326, 335,'AULAS3_Nodo17', 330,'AULAS3_Nodo12');");
			sqlBuilder.Append("Insert Into Neighbor values (327, 335,'AULAS3_Nodo17', 55,'Node 20.5');");

			sqlBuilder.Append("Insert Into Neighbor values (328, 100,'Node 61', 304,'A3-16');");

			sqlBuilder.Append("Insert Into Neighbor values (329, 337,'AULAS3_Nodo19', 101,'Node 62');");
			sqlBuilder.Append("Insert Into Neighbor values (330, 337,'AULAS3_Nodo19', 305,'A3-17');");


			sqlBuilder.Append("Insert Into Neighbor values (331, 338,'AULAS3_Nodo20', 331,'AULAS3_Nodo13');");
			sqlBuilder.Append("Insert Into Neighbor values (332, 338,'AULAS3_Nodo20', 323,'AULAS3_Nodo5');");

			sqlBuilder.Append("Insert Into Neighbor values (333, 322,'AULAS3_Nodo4', 339,'AULAS3_Nodo21');");
			sqlBuilder.Append("Insert Into Neighbor values (334, 339,'AULAS3_Nodo21', 300,'A3-11');");

			sqlBuilder.Append("Insert Into Neighbor values (335, 328,'AULAS3_Nodo10', 318,'A3-Bedel');");
			sqlBuilder.Append("Insert Into Neighbor values (336, 329,'AULAS3_Nodo11', 303,'A3-14');");

			#endregion

			#region Planta2

			sqlBuilder.Append("Insert Into Neighbor values (338, 338,'AULAS3_Nodo20', 340,'AULAS3_Nodo22');");
			sqlBuilder.Append("Insert Into Neighbor values (339, 340,'AULAS3_Nodo22', 341,'AULAS3_Nodo23');");
			sqlBuilder.Append("Insert Into Neighbor values (340, 341,'AULAS3_Nodo23', 311,'Salon de Dibujo I');");
			sqlBuilder.Append("Insert Into Neighbor values (341, 341,'AULAS3_Nodo23', 342,'AULAS3_Nodo24');");
			sqlBuilder.Append("Insert Into Neighbor values (342, 342,'AULAS3_Nodo24', 311,'Salon de Dibujo I');");
			sqlBuilder.Append("Insert Into Neighbor values (343, 342,'AULAS3_Nodo24', 343,'AULAS3_Nodo25');");
			sqlBuilder.Append("Insert Into Neighbor values (344, 343,'AULAS3_Nodo25', 344,'AULAS3_Nodo26');");
			sqlBuilder.Append("Insert Into Neighbor values (345, 344,'AULAS3_Nodo26', 345,'AULAS3_Nodo27');");

			sqlBuilder.Append("Insert Into Neighbor values (346, 345,'AULAS3_Nodo27', 346,'AULAS3_Nodo28');");
			sqlBuilder.Append("Insert Into Neighbor values (347, 346,'AULAS3_Nodo28', 347,'AULAS3_Nodo29');");
			sqlBuilder.Append("Insert Into Neighbor values (348, 347,'AULAS3_Nodo29', 307,'A3-22');");
			sqlBuilder.Append("Insert Into Neighbor values (349, 347,'AULAS3_Nodo29', 348,'AULAS3_Nodo30');");
			sqlBuilder.Append("Insert Into Neighbor values (350, 348,'AULAS3_Nodo30', 306,'A3-21');");
			sqlBuilder.Append("Insert Into Neighbor values (351, 348,'AULAS3_Nodo30', 307,'A3-22');");

			sqlBuilder.Append("Insert Into Neighbor values (352, 344,'AULAS3_Nodo26', 349,'AULAS3_Nodo31');");
			sqlBuilder.Append("Insert Into Neighbor values (353, 349,'AULAS3_Nodo31', 350,'AULAS3_Nodo32');");
			sqlBuilder.Append("Insert Into Neighbor values (354, 350,'AULAS3_Nodo32', 308,'A3-23');");
			sqlBuilder.Append("Insert Into Neighbor values (355, 350,'AULAS3_Nodo32', 351,'AULAS3_Nodo33');");
			sqlBuilder.Append("Insert Into Neighbor values (356, 351,'AULAS3_Nodo33', 308,'A3-23');");
			sqlBuilder.Append("Insert Into Neighbor values (357, 351,'AULAS3_Nodo33', 309,'A3-24');");
			sqlBuilder.Append("Insert Into Neighbor values (358, 351,'AULAS3_Nodo33', 352,'AULAS3_Nodo34');");
			sqlBuilder.Append("Insert Into Neighbor values (359, 352,'AULAS3_Nodo34', 309,'A3-24');");
			sqlBuilder.Append("Insert Into Neighbor values (360, 352,'AULAS3_Nodo34', 310,'A3-25');");

			#endregion

			#region Planta3

			sqlBuilder.Append("Insert Into Neighbor values (361, 345,'AULAS3_Nodo27', 353,'AULAS3_Nodo35');");
			sqlBuilder.Append("Insert Into Neighbor values (362, 353,'AULAS3_Nodo35', 354,'AULAS3_Nodo36');");
			sqlBuilder.Append("Insert Into Neighbor values (363, 354,'AULAS3_Nodo36', 317,'Salon de Dibujo II');");
			sqlBuilder.Append("Insert Into Neighbor values (364, 354,'AULAS3_Nodo36', 355,'AULAS3_Nodo37');");
			sqlBuilder.Append("Insert Into Neighbor values (365, 355,'AULAS3_Nodo37', 317,'Salon de Dibujo II');");
			sqlBuilder.Append("Insert Into Neighbor values (366, 355,'AULAS3_Nodo37', 356,'AULAS3_Nodo38');");
			sqlBuilder.Append("Insert Into Neighbor values (367, 356,'AULAS3_Nodo38', 357,'AULAS3_Nodo39');");
			sqlBuilder.Append("Insert Into Neighbor values (368, 357,'AULAS3_Nodo39', 358,'AULAS3_Nodo40');");

			sqlBuilder.Append("Insert Into Neighbor values (369, 358,'AULAS3_Nodo40', 359,'AULAS3_Nodo41');");
			sqlBuilder.Append("Insert Into Neighbor values (370, 359,'AULAS3_Nodo41', 313,'A3-32');");
			sqlBuilder.Append("Insert Into Neighbor values (371, 359,'AULAS3_Nodo41', 360,'AULAS3_Nodo42');");
			sqlBuilder.Append("Insert Into Neighbor values (372, 360,'AULAS3_Nodo42', 312,'A3-31');");
			sqlBuilder.Append("Insert Into Neighbor values (373, 360,'AULAS3_Nodo42', 313,'A3-32');");

			sqlBuilder.Append("Insert Into Neighbor values (374, 358,'AULAS3_Nodo40', 361,'AULAS3_Nodo43');");
			sqlBuilder.Append("Insert Into Neighbor values (375, 361,'AULAS3_Nodo43', 314,'A3-33');");
			sqlBuilder.Append("Insert Into Neighbor values (376, 361,'AULAS3_Nodo43', 362,'AULAS3_Nodo44');");
			sqlBuilder.Append("Insert Into Neighbor values (377, 362,'AULAS3_Nodo44', 314,'A3-33');");
			sqlBuilder.Append("Insert Into Neighbor values (378, 362,'AULAS3_Nodo44', 315,'A3-34');");
			sqlBuilder.Append("Insert Into Neighbor values (379, 362,'AULAS3_Nodo44', 363,'AULAS3_Nodo45');");
			sqlBuilder.Append("Insert Into Neighbor values (380, 363,'AULAS3_Nodo45', 315,'A3-34');");
			sqlBuilder.Append("Insert Into Neighbor values (381, 363,'AULAS3_Nodo45', 316,'A3-35');");

			#endregion

			TransactionalQuery(sqlBuilder.ToString());

			#endregion

			#region Aulas 4 -> 400

			sqlBuilder = new StringBuilder();

			#region Nodos A4

			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (400, 11,'A4-11',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (401, 11,'A4-12',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (402, 11,'A4-13',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (403, 11,'A4-14',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (404, 11,'A4-15',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (405, 11,'A4-16',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (406, 11,'A4-21',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (407, 11,'A4-22',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (408, 11,'A4-23',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (409, 11,'A4-24',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (410, 11,'A4-25',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (411, 11,'A4-31',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (412, 11,'A4-32',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (413, 11,'A4-33',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (414, 11,'A4-34',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (415, 11,'A4-35',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (416, 11,'A4-Bedel',1,3);");

			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (417, 11,'AULAS4_Nodo1',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (418, 11,'AULAS4_Nodo2',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (419, 11,'AULAS4_Nodo3',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (420, 11,'AULAS4_Nodo4',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (421, 11,'AULAS4_Nodo5',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (422, 11,'AULAS4_Nodo5.5',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (423, 11,'AULAS4_Nodo6',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (424, 11,'AULAS4_Nodo7',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (425, 11,'AULAS4_Nodo8',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (426, 11,'AULAS4_Nodo9',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (427, 11,'AULAS4_Nodo10',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (428, 11,'AULAS4_Nodo11',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (429, 11,'AULAS4_Nodo12',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (430, 11,'AULAS4_Nodo13',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (431, 11,'AULAS4_Nodo14',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (432, 11,'AULAS4_Nodo15',1,1);");

			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (433, 11,'AULAS4_Nodo16',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (434, 11,'AULAS4_Nodo17',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (435, 11,'AULAS4_Nodo18',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (436, 11,'AULAS4_Nodo19',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (437, 11,'AULAS4_Nodo20',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (438, 11,'AULAS4_Nodo21',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (439, 11,'AULAS4_Nodo22',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (440, 11,'AULAS4_Nodo23',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (441, 11,'AULAS4_Nodo24',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (442, 11,'AULAS4_Nodo25',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (443, 11,'AULAS4_Nodo26',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (444, 11,'AULAS4_Nodo27',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (445, 11,'AULAS4_Nodo28',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (446, 11,'AULAS4_Nodo29',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (447, 11,'AULAS4_Nodo30',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (448, 11,'AULAS4_Nodo31',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (449, 11,'AULAS4_Nodo32',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (450, 11,'AULAS4_Nodo33',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (451, 11,'AULAS4_Nodo34',1,2);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (452, 11,'AULAS4_Nodo35',1,2);");

			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (453, 11,'AULAS4_Nodo36',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (454, 11,'AULAS4_Nodo37',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (455, 11,'AULAS4_Nodo38',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (456, 11,'AULAS4_Nodo39',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (457, 11,'AULAS4_Nodo40',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (458, 11,'AULAS4_Nodo41',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (459, 11,'AULAS4_Nodo42',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (460, 11,'AULAS4_Nodo43',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (461, 11,'AULAS4_Nodo44',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (462, 11,'AULAS4_Nodo45',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (463, 11,'AULAS4_Nodo46',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (464, 11,'AULAS4_Nodo47',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (465, 11,'AULAS4_Nodo48',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (466, 11,'AULAS4_Nodo49',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (467, 11,'AULAS4_Nodo50',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (468, 11,'AULAS4_Nodo51',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (469, 11,'AULAS4_Nodo52',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (470, 11,'AULAS4_Nodo53',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (471, 11,'AULAS4_Nodo54',1,3);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (472, 11,'AULAS4_Nodo55',1,3);");

			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (473, 11,'AULAS4_Nodo1.5',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (474, 11,'AULAS4_Nodo2.5',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (475, 11,'AULAS4_Nodo3.5',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (476, 11,'AULAS4_Nodo56',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (477, 11,'AULAS4_Nodo57',1,1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, idUbicacion, nombre, activo, planta) values (478, 11,'AULAS4_Nodo58',1,1);");

			#region Coordenada
			
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 400, 400, -71.38872, -63.09964);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 401, 401, -72.53468, -67.89835);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 402, 402, -72.53468, -75.32272);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 403, 403, -72.53468, -83.17952);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 404, 404, -72.53468, -90.43492);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 405, 405, -71.10265, -95.26787);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 406, 406, -71.38872, -63.09964);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 407, 407, -72.15552, -73.71671);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 408, 408, -72.15552, -74.53363);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 409, 409, -72.15552, -84.70815);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 410, 410, -71.10265, -95.26787);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 411, 411, -71.38872, -63.09964);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 412, 412, -72.15552, -73.71671);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 413, 413, -72.15552, -74.53363);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 414, 414, -72.15552, -84.70815);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 415, 415, -71.10265, -95.26787);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 416, 416, -71.10265, -95.26787);");

			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 417, 417, -69.42599, -80.86333);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 418, 418, -79.44044, -80.81641);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 419, 419, -82.00778, -80.81931);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 420, 420, -74.56327, -81.86002);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 421, 421, -73.08286, -76.77988);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 422, 422, -76.67506, -76.77988);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 423, 423, -69.58312, -76.09524);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 424, 424, -69.58312, -74.67059);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 425, 425, -69.58312, -71.85518);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 426, 426, -69.58312, -69.08182);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 427, 427, -69.58312, -66.33599);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 428, 428, -69.58312, -82.55781);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 429, 429, -69.58312, -84.44877);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 430, 430, -69.58312, -87.77254);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 431, 431, -69.58312, -89.59022);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 432, 432, -69.58312, -92.32164);");
			
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 433, 433, -66.17279, -66.2272);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 434, 434, -63.48983, -65.99696);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 435, 435, -63.48983, -64.40867);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 436, 436, -65.67355, -64.21241);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 437, 437, -66.56064, -93.14214);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 438, 438, -63.77089, -93.43439);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 439, 439, -63.77089, -95.47803);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 440, 440, -65.68049, -95.70292);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 441, 441, -68.6246, -64.04218);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 442, 442, -68.6246, -66.08994);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 443, 443, -70.67625, -64.44982);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 444, 444, -70.67625, -69.17361);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 445, 445, -70.67625, -73.78217);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 446, 446, -70.67625, -74.82053);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 447, 447, -70.67625, -79.82765);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 448, 448, -70.67625, -84.78305);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 449, 449, -70.67625, -89.37828);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 450, 450, -70.67625, -94.0473);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 451, 451, -68.81034, -93.18438);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 452, 452, -68.81034, -94.98606);");

			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 453, 453, -66.17279, -66.2272);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 454, 454, -63.48983, -65.99696);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 455, 455, -63.48983, -64.40867);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 456, 456, -65.67355, -64.21241);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 457, 457, -66.56064, -93.14214);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 458, 458, -63.77089, -93.43439);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 459, 459, -63.77089, -95.47803);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 460, 460, -65.68049, -95.70292);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 461, 461, -68.6246, -64.04218);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 462, 462, -68.6246, -66.08994);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 463, 463, -70.67625, -64.44982);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 464, 464, -70.67625, -69.17361);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 465, 465, -70.67625, -73.78217);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 466, 466, -70.67625, -74.82053);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 467, 467, -70.67625, -79.82765);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 468, 468, -70.67625, -84.78305);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 469, 469, -70.67625, -89.37828);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 470, 470, -70.67625, -94.0473);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 471, 471, -68.81034, -93.18438);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 472, 472, -68.81034, -94.98606);");

			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 473, 473, -69.42599, -77.83113);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 474, 474, -79.44044, -77.80381);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 475, 475, -81.87326, -77.80651);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 476, 476, -64.97144, -78.88818);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 477, 477, -64.86373, -87.89888);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 478, 478, -64.86373, -71.28806);");

			#endregion

			#endregion

			#region Planta1

			sqlBuilder.Append("Insert Into Neighbor values (400, 417,'AULAS4_Nodo1', 476,'AULAS4_Nodo56');");
			sqlBuilder.Append("Insert Into Neighbor values (401, 417,'AULAS4_Nodo1', 11,'Aulas 4');");
			sqlBuilder.Append("Insert Into Neighbor values (402, 417,'AULAS4_Nodo1', 473,'AULAS4_Nodo1.5');");
			sqlBuilder.Append("Insert Into Neighbor values (403, 473,'AULAS4_Nodo1.5', 476,'AULAS4_Nodo56');");
			sqlBuilder.Append("Insert Into Neighbor values (404, 473,'AULAS4_Nodo1.5', 11,'Aulas 4');");

			sqlBuilder.Append("Insert Into Neighbor values (405, 425,'AULAS4_Nodo8', 478,'AULAS4_Nodo58');");
			sqlBuilder.Append("Insert Into Neighbor values (406, 430,'AULAS4_Nodo13', 477,'AULAS4_Nodo57');");

			sqlBuilder.Append("Insert Into Neighbor values (407, 418,'AULAS4_Nodo2', 474,'AULAS4_Nodo2.5');");
			sqlBuilder.Append("Insert Into Neighbor values (408, 419,'AULAS4_Nodo3', 475,'AULAS4_Nodo3.5');");

			sqlBuilder.Append("Insert Into Neighbor values (410, 417,'AULAS4_Nodo1', 423,'AULAS4_Nodo6');");
			sqlBuilder.Append("Insert Into Neighbor values (411, 423,'AULAS4_Nodo6', 402,'A4-13');");
			sqlBuilder.Append("Insert Into Neighbor values (412, 423,'AULAS4_Nodo6', 424,'AULAS4_Nodo7');");
			sqlBuilder.Append("Insert Into Neighbor values (413, 424,'AULAS4_Nodo7', 402,'A4-13');");
			sqlBuilder.Append("Insert Into Neighbor values (414, 424,'AULAS4_Nodo7', 425,'AULAS4_Nodo8');");
			sqlBuilder.Append("Insert Into Neighbor values (415, 425,'AULAS4_Nodo8', 426,'AULAS4_Nodo9');");
			sqlBuilder.Append("Insert Into Neighbor values (416, 426,'AULAS4_Nodo9', 401,'A4-12');");
			sqlBuilder.Append("Insert Into Neighbor values (417, 426,'AULAS4_Nodo9', 427,'AULAS4_Nodo10');");
			sqlBuilder.Append("Insert Into Neighbor values (418, 427,'AULAS4_Nodo10', 401,'A4-12');");
			sqlBuilder.Append("Insert Into Neighbor values (419, 427,'AULAS4_Nodo10', 400,'A4-11');");

			sqlBuilder.Append("Insert Into Neighbor values (420, 473,'AULAS4_Nodo1.5', 428,'AULAS4_Nodo11');");
			sqlBuilder.Append("Insert Into Neighbor values (421, 428,'AULAS4_Nodo11', 403,'A4-14');");
			sqlBuilder.Append("Insert Into Neighbor values (422, 428,'AULAS4_Nodo11', 429,'AULAS4_Nodo12');");
			sqlBuilder.Append("Insert Into Neighbor values (423, 429,'AULAS4_Nodo12', 403,'A4-14');");
			sqlBuilder.Append("Insert Into Neighbor values (424, 429,'AULAS4_Nodo12', 430,'AULAS4_Nodo13');");
			sqlBuilder.Append("Insert Into Neighbor values (425, 429,'AULAS4_Nodo12', 430,'AULAS4_Nodo13');");
			sqlBuilder.Append("Insert Into Neighbor values (426, 430,'AULAS4_Nodo13', 431,'AULAS4_Nodo14');");
			sqlBuilder.Append("Insert Into Neighbor values (427, 431,'AULAS4_Nodo14', 404,'A4-15');");
			sqlBuilder.Append("Insert Into Neighbor values (428, 431,'AULAS4_Nodo14', 432,'AULAS4_Nodo15');");
			sqlBuilder.Append("Insert Into Neighbor values (429, 432,'AULAS4_Nodo15', 405,'A4-11');");

			sqlBuilder.Append("Insert Into Neighbor values (430, 421,'AULAS4_Nodo5', 417,'AULAS4_Nodo1');");
			sqlBuilder.Append("Insert Into Neighbor values (431, 421,'AULAS4_Nodo5', 423,'AULAS4_Nodo6');");
			sqlBuilder.Append("Insert Into Neighbor values (432, 421,'AULAS4_Nodo5', 416,'A4-Bedel');");
			sqlBuilder.Append("Insert Into Neighbor values (433, 421,'AULAS4_Nodo5', 422,'AULAS4_Nodo5.5');");
			sqlBuilder.Append("Insert Into Neighbor values (434, 422,'AULAS4_Nodo5.5', 416,'A4-Bedel');");
			sqlBuilder.Append("Insert Into Neighbor values (435, 422,'AULAS4_Nodo5.5', 474,'AULAS4_Nodo2.5');");

			sqlBuilder.Append("Insert Into Neighbor values (436, 420,'AULAS4_Nodo4', 473,'AULAS4_Nodo1.5');");
			sqlBuilder.Append("Insert Into Neighbor values (437, 420,'AULAS4_Nodo4', 428,'AULAS4_Nodo11');");
			sqlBuilder.Append("Insert Into Neighbor values (438, 420,'AULAS4_Nodo4', 418,'AULAS4_Nodo2.5');");

			sqlBuilder.Append("Insert Into Neighbor values (439, 418,'AULAS4_Nodo2', 419,'AULAS4_Nodo3');");
			sqlBuilder.Append("Insert Into Neighbor values (440, 474,'AULAS4_Nodo2.5', 475,'AULAS4_Nodo3.5');");

			sqlBuilder.Append("Insert Into Neighbor values (501, 419,'AULAS4_Nodo3', 65,'Node 27');");
			sqlBuilder.Append("Insert Into Neighbor values (502, 475,'AULAS4_Nodo3.5', 65,'Node 27');");
			sqlBuilder.Append("Insert Into Neighbor values (503, 476,'AULAS4_Nodo56', 90,'Node 51');");
			sqlBuilder.Append("Insert Into Neighbor values (504, 477,'AULAS4_Nodo57', 89,'Node 50');");
			sqlBuilder.Append("Insert Into Neighbor values (505, 478,'AULAS4_Nodo58', 102,'Node 63');");

			#endregion

			#region Planta 2

			sqlBuilder.Append("Insert Into Neighbor values (441, 433,'AULAS4_Nodo16', 427,'AULAS4_Nodo10');");
			sqlBuilder.Append("Insert Into Neighbor values (442, 433,'AULAS4_Nodo16', 434,'AULAS4_Nodo17');");
			sqlBuilder.Append("Insert Into Neighbor values (443, 434,'AULAS4_Nodo17', 435,'AULAS4_Nodo18');");
			sqlBuilder.Append("Insert Into Neighbor values (444, 435,'AULAS4_Nodo18', 436,'AULAS4_Nodo19');");
			sqlBuilder.Append("Insert Into Neighbor values (445, 436,'AULAS4_Nodo19', 441,'AULAS4_Nodo24');");
			sqlBuilder.Append("Insert Into Neighbor values (446, 441,'AULAS4_Nodo24', 443,'AULAS4_Nodo26');");
			sqlBuilder.Append("Insert Into Neighbor values (447, 443,'AULAS4_Nodo26', 406,'A4-21');");
			sqlBuilder.Append("Insert Into Neighbor values (448, 441,'AULAS4_Nodo24', 442,'AULAS4_Nodo25');");
			sqlBuilder.Append("Insert Into Neighbor values (449, 442,'AULAS4_Nodo25', 444,'AULAS4_Nodo27');");
			sqlBuilder.Append("Insert Into Neighbor values (450, 444,'AULAS4_Nodo27', 443,'AULAS4_Nodo26');");
			sqlBuilder.Append("Insert Into Neighbor values (451, 444,'AULAS4_Nodo27', 445,'AULAS4_Nodo28');");
			sqlBuilder.Append("Insert Into Neighbor values (452, 445,'AULAS4_Nodo28', 407,'A4-22');");
			sqlBuilder.Append("Insert Into Neighbor values (453, 445,'AULAS4_Nodo28', 446,'AULAS4_Nodo29');");
			sqlBuilder.Append("Insert Into Neighbor values (454, 446,'AULAS4_Nodo29', 408,'A4-23');");
			sqlBuilder.Append("Insert Into Neighbor values (455, 446,'AULAS4_Nodo29', 447,'AULAS4_Nodo30');");
			sqlBuilder.Append("Insert Into Neighbor values (456, 447,'AULAS4_Nodo30', 448,'AULAS4_Nodo31');");
			sqlBuilder.Append("Insert Into Neighbor values (457, 448,'AULAS4_Nodo31', 409,'A4-24');");
			sqlBuilder.Append("Insert Into Neighbor values (458, 448,'AULAS4_Nodo31', 449,'AULAS4_Nodo32');");
			sqlBuilder.Append("Insert Into Neighbor values (459, 449,'AULAS4_Nodo32', 450,'AULAS4_Nodo33');");
			sqlBuilder.Append("Insert Into Neighbor values (460, 450,'AULAS4_Nodo33', 410,'A4-25');");
			sqlBuilder.Append("Insert Into Neighbor values (461, 450,'AULAS4_Nodo33', 452,'AULAS4_Nodo35');");
			sqlBuilder.Append("Insert Into Neighbor values (462, 452,'AULAS4_Nodo35', 451,'AULAS4_Nodo34');");
			sqlBuilder.Append("Insert Into Neighbor values (463, 451,'AULAS4_Nodo34', 449,'AULAS4_Nodo32');");
			sqlBuilder.Append("Insert Into Neighbor values (466, 452,'AULAS4_Nodo35', 440,'AULAS4_Nodo23');");
			sqlBuilder.Append("Insert Into Neighbor values (467, 440,'AULAS4_Nodo23', 439,'AULAS4_Nodo22');");
			sqlBuilder.Append("Insert Into Neighbor values (468, 439,'AULAS4_Nodo22', 438,'AULAS4_Nodo21');");
			sqlBuilder.Append("Insert Into Neighbor values (469, 438,'AULAS4_Nodo23', 437,'AULAS4_Nodo22');");
			sqlBuilder.Append("Insert Into Neighbor values (470, 437,'AULAS4_Nodo22', 432,'AULAS4_Nodo15');");

			#endregion

			#region Planta 3

			sqlBuilder.Append("Insert Into Neighbor values (471, 453,'AULAS4_Nodo36', 442,'AULAS4_Nodo25');");
			sqlBuilder.Append("Insert Into Neighbor values (472, 453,'AULAS4_Nodo36', 454,'AULAS4_Nodo37');");
			sqlBuilder.Append("Insert Into Neighbor values (473, 454,'AULAS4_Nodo37', 455,'AULAS4_Nodo38');");
			sqlBuilder.Append("Insert Into Neighbor values (474, 455,'AULAS4_Nodo38', 456,'AULAS4_Nodo39');");
			sqlBuilder.Append("Insert Into Neighbor values (475, 456,'AULAS4_Nodo39', 461,'AULAS4_Nodo44');");
			sqlBuilder.Append("Insert Into Neighbor values (476, 461,'AULAS4_Nodo44', 463,'AULAS4_Nodo46');");
			sqlBuilder.Append("Insert Into Neighbor values (477, 463,'AULAS4_Nodo46', 411,'A4-31');");
			sqlBuilder.Append("Insert Into Neighbor values (478, 461,'AULAS4_Nodo44', 462,'AULAS4_Nodo45');");
			sqlBuilder.Append("Insert Into Neighbor values (479, 462,'AULAS4_Nodo45', 464,'AULAS4_Nodo47');");
			sqlBuilder.Append("Insert Into Neighbor values (480, 464,'AULAS4_Nodo47', 463,'AULAS4_Nodo46');");
			sqlBuilder.Append("Insert Into Neighbor values (481, 464,'AULAS4_Nodo47', 465,'AULAS4_Nodo48');");
			sqlBuilder.Append("Insert Into Neighbor values (482, 465,'AULAS4_Nodo48', 412,'A4-32');");
			sqlBuilder.Append("Insert Into Neighbor values (483, 465,'AULAS4_Nodo48', 466,'AULAS4_Nodo49');");
			sqlBuilder.Append("Insert Into Neighbor values (484, 466,'AULAS4_Nodo49', 413,'A4-33');");
			sqlBuilder.Append("Insert Into Neighbor values (485, 466,'AULAS4_Nodo49', 467,'AULAS4_Nodo50');");
			sqlBuilder.Append("Insert Into Neighbor values (486, 467,'AULAS4_Nodo50', 468,'AULAS4_Nodo51');");
			sqlBuilder.Append("Insert Into Neighbor values (487, 468,'AULAS4_Nodo51', 414,'A4-34');");
			sqlBuilder.Append("Insert Into Neighbor values (488, 468,'AULAS4_Nodo51', 469,'AULAS4_Nodo52');");
			sqlBuilder.Append("Insert Into Neighbor values (489, 469,'AULAS4_Nodo52', 470,'AULAS4_Nodo53');");
			sqlBuilder.Append("Insert Into Neighbor values (490, 470,'AULAS4_Nodo53', 415,'A4-35');");
			sqlBuilder.Append("Insert Into Neighbor values (491, 470,'AULAS4_Nodo53', 472,'AULAS4_Nodo55');");
			sqlBuilder.Append("Insert Into Neighbor values (492, 472,'AULAS4_Nodo55', 471,'AULAS4_Nodo54');");
			sqlBuilder.Append("Insert Into Neighbor values (493, 471,'AULAS4_Nodo54', 469,'AULAS4_Nodo52');");
			sqlBuilder.Append("Insert Into Neighbor values (496, 472,'AULAS4_Nodo55', 460,'AULAS4_Nodo43');");
			sqlBuilder.Append("Insert Into Neighbor values (497, 460,'AULAS4_Nodo43', 459,'AULAS4_Nodo42');");
			sqlBuilder.Append("Insert Into Neighbor values (498, 459,'AULAS4_Nodo42', 458,'AULAS4_Nodo41');");
			sqlBuilder.Append("Insert Into Neighbor values (499, 458,'AULAS4_Nodo43', 457,'AULAS4_Nodo42');");
			sqlBuilder.Append("Insert Into Neighbor values (500, 457,'AULAS4_Nodo42', 451,'AULAS4_Nodo34');");

			#endregion

			TransactionalQuery(sqlBuilder.ToString());

			#endregion

			#endregion

			#region CoordenadaNodo

			sqlBuilder = new StringBuilder();

			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 1, 8, -41.74656,-113.5702);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 2, 9, 95.9976,-82.7831);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 3, 10, -78.58511,-248.1843);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 4, 11, -72.05814,-79.38595);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 5, 12, 14.03714,-12.91638);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 6, 16, -5.921415,104.006);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 7, 6, -168.5624,-77.78427);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 8, 7, -187.247,-40.94572);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 9, 14, -161.3434,-221.9352);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 10, 5, -222.2807,-40.94572);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 11, 4, -298.0807,-7.310517);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 12, 1, -395.5379,-49.75494);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 13, 3, -345.6415,-72.97925);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 14, 13, -2.736539,-160.2706);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 15, 2, -383.2231,-5.708841);");

			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 30, 30, -382.7984,-49.75494);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 31, 31, -373.2438,-49.75494);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 32, 32, -353.9222,-49.75494);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 33, 33, -357.1071,-56.96248);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 34, 34, -340.1211,-49.75494);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 35, 35, -345.0046,-57.76332);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 36, 36, -329.9295,-7.310517);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 37, 37, -382.5861,-20.12393);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 38, 38, -372.1822,-10.51387);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 39, 39, -385.134,-59.365);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 40, 40, -387.6819,-85.79266);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 41, 41, -384.0724,-105.0128);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 42, 42, -338.6348,-97.00439);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 43, 43, -343.7306,-88.19517);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 44, 44, -298.0807,-49.75494);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 45, 45, -291.2863,-49.75494);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 46, 46, -254.3418,-49.75494);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 47, 47, -255.1911,-72.17841);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 48, 48, -290.8617,-7.310517);");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 49, 49, );");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 50, 50, -252.8555,-7.310517);");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 51, 51, );");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 52, 52, -230.7737,-7.310517);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 53, 53, -230.1367,-40.94572);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 54, 54, -230.349,-77.78427);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 55, 55, -235.3625,-77.23409);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 56, 56, -251.1569,-89.79684);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 57, 57, -287.8891,-106.6144);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 58, 58, -202.7468,-77.78427);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 59, 59, -202.7468,-51.35662);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 60, 60, -197.4386,-44.94991);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 61, 61, -193.6168,-44.94991);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 62, 62, -211.6644,-42.5474);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 63, 63, -211.4521,-43.34823);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 64, 64, -129.4946,-41.74656);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 65, 65, -131.6179,-78.58511);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 66, 66, -161.131,-78.58511);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 67, 67, -112.2963,49.54899);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 68, 68, -104.8649,96.79845);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 69, 69, -75.56404,93.59509);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 70, 70, -92.9747,95.19677);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 71, 71, 4.270189,36.73558);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 72, 72, -38.8318,-17.72141);");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 73, 73, );");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 74, 74, );");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 75, 75, -37.1332,-64.97086);");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 76, 76, );");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 77, 77, );");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 78, 78, 9.365991,-52.15746);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 79, 79, 2.571588,-117.8262);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 80, 80, -2.311889,-130.6396);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 81, 81, -9.106291,-145.0547);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 82, 82, -11.22954,-154.6647);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 83, 83, -9.530941,-160.2706);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 84, 84, -41.59203,-160.2706);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 85, 85, -42.01668,-195.5075);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 86, 86, -53.48223,-157.8681);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 87, 87, -87.8789,-137.8471);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 88, 88, -109.0065,-60.78814);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 89, 89, -60.27663,-87.96867);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 90, 90, -60.60952,-78.98951);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 91, 91, -60.68656,-49.14934);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 92, 92, -40.31808,-78.58511);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 93, 93, -113.5702,66.36659);");

			//New Nodes
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 94, 94, -340.3334,-58.56416);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 95, 95, -352.0113,-59.365);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 96, 96, -337.9978,-88.99601);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 97, 97, -353.0729,-88.99601);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 98, 98, -352.8606,-97.00439);");

			//Aulas 3
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 99, 99, -255.1911,-75.38176);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 100, 100, -255.1911,-77.78427);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 101, 101, 19.441455, -70.683427);");

			//Aulas 4
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 102, 102, -60.60952, -71.37204);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 103, 103, -42.28512, -82.34691);");

			TransactionalQuery(sqlBuilder.ToString());

			#endregion

			#region Neighbor

			sqlBuilder = new StringBuilder();

			sqlBuilder.Append("Insert Into Neighbor values (1, 30,'Node 1',      1,'Departamento de Ingenierías Electrónica y Electromecánica');");
			sqlBuilder.Append("Insert Into Neighbor values (2, 30,'Node 1',     31,'Node 2');");
			sqlBuilder.Append("Insert Into Neighbor values (3, 30,'Node 1',     39,'Node 7');");
			sqlBuilder.Append("Insert Into Neighbor values (4, 30,'Node 1',     37,'Node 6');");
			sqlBuilder.Append("Insert Into Neighbor values (5, 31,'Node 2',     32,'Node 3');");
			sqlBuilder.Append("Insert Into Neighbor values (6, 31,'Node 2',     39,'Node 7');");
			sqlBuilder.Append("Insert Into Neighbor values (7, 32,'Node 3',     34,'Node 4');");
			sqlBuilder.Append("Insert Into Neighbor values (8, 32,'Node 3',     33,'Node 3.5');");
			sqlBuilder.Append("Insert Into Neighbor values (9, 32,'Node 3',     37,'Node 6');");
			sqlBuilder.Append("Insert Into Neighbor values (10, 33,'Node 3.5',   3,'Suministro y Talleres');");
			sqlBuilder.Append("Insert Into Neighbor values (11, 34,'Node 4',    35,'Node 4.5');");
			sqlBuilder.Append("Insert Into Neighbor values (12, 34,'Node 4',    36,'Node 5');");
			sqlBuilder.Append("Insert Into Neighbor values (13, 34,'Node 4',    44,'Node 10');");
			sqlBuilder.Append("Insert Into Neighbor values (14, 35,'Node 4.5',   3,'Suministro y Talleres');");
			sqlBuilder.Append("Insert Into Neighbor values (15, 36,'Node 5',    38,'Node 6.5');");
			sqlBuilder.Append("Insert Into Neighbor values (16, 36,'Node 5',     4,'Laboratorios Generales de Ingeniería');");
			sqlBuilder.Append("Insert Into Neighbor values (17, 37,'Node 6',    38,'Node 6.5');");
			sqlBuilder.Append("Insert Into Neighbor values (18, 37,'Node 6',     2,'Talleres de Ingeniería Electrónica y Electromecánica');");
			sqlBuilder.Append("Insert Into Neighbor values (19, 38,'Node 6.5',   2,'Talleres de Ingeniería Electrónica y Electromecánica');");
			sqlBuilder.Append("Insert Into Neighbor values (20, 39,'Node 7',    40,'Node 8');");
			sqlBuilder.Append("Insert Into Neighbor values (21, 40,'Node 8',    41,'Node 8.5');");
			sqlBuilder.Append("Insert Into Neighbor values (22, 42,'Node 9',    57,'Node 21.5');");
			sqlBuilder.Append("Insert Into Neighbor values (23, 42,'Node 9',    43,'Node 9.5');");
			sqlBuilder.Append("Insert Into Neighbor values (24, 43,'Node 9.5',   3,'Suministro y Talleres');");
			sqlBuilder.Append("Insert Into Neighbor values (25, 44,'Node 10',   45,'Node 11');");
			sqlBuilder.Append("Insert Into Neighbor values (26, 44,'Node 10',   47,'Node 13');");
			sqlBuilder.Append("Insert Into Neighbor values (27, 45,'Node 11',   48,'Node 14');");
			sqlBuilder.Append("Insert Into Neighbor values (28, 45,'Node 11',   46,'Node 12');");
			sqlBuilder.Append("Insert Into Neighbor values (29, 46,'Node 12',   47,'Node 13');");
			sqlBuilder.Append("Insert Into Neighbor values (30, 46,'Node 12',   50,'Node 16');");
			sqlBuilder.Append("Insert Into Neighbor values (31, 46,'Node 12',   52,'Node 18');");
			sqlBuilder.Append("Insert Into Neighbor values (32, 46,'Node 12',   53,'Node 19');");
			////sqlBuilder.Append("Insert Into Neighbor values (33, 47,'Node 13',   10,'Aulas 3');");
			sqlBuilder.Append("Insert Into Neighbor values (34, 48,'Node 14',    4,'Laboratorios Generales de Ingeniería');");
			sqlBuilder.Append("Insert Into Neighbor values (35, 48,'Node 14',   50,'Node 16');");
			sqlBuilder.Append("Insert Into Neighbor values (36, 50,'Node 16',   51,'Node 17');");
			//Revisar estos nodos. Como que hay uno de más.
			sqlBuilder.Append("Insert Into Neighbor values (37, 51,'Node 17',   52,'Node 18');");
			sqlBuilder.Append("Insert Into Neighbor values (38, 52,'Node 18',   93,'Node 54');");
			sqlBuilder.Append("Insert Into Neighbor values (39, 53,'Node 19',   54,'Node 20');");
			sqlBuilder.Append("Insert Into Neighbor values (40, 53,'Node 19',    5,'Departamentos de Ingeniería');");
			sqlBuilder.Append("Insert Into Neighbor values (41, 54,'Node 20',   58,'Node 22');");
			sqlBuilder.Append("Insert Into Neighbor values (42, 54,'Node 20',   55,'Node 20.5');");
			//Conexion con A3 -> caminos internos (300)
			////sqlBuilder.Append("Insert Into Neighbor values (43, 55,'Node 20.5', 10,'Aulas 3');");
			sqlBuilder.Append("Insert Into Neighbor values (44, 56,'Node 21',   57,'Node 21.5');");
			////sqlBuilder.Append("Insert Into Neighbor values (45, 56,'Node 21',   10,'Aulas 3');");
			sqlBuilder.Append("Insert Into Neighbor values (46, 58,'Node 22',   59,'Node 23');");
			sqlBuilder.Append("Insert Into Neighbor values (47, 58,'Node 22',    6,'Ciencias Básicas I');");
			sqlBuilder.Append("Insert Into Neighbor values (48, 59,'Node 23',   60,'Node 24');");
			sqlBuilder.Append("Insert Into Neighbor values (49, 59,'Node 23',   62,'Node 25');");
			sqlBuilder.Append("Insert Into Neighbor values (50, 60,'Node 24',   62,'Node 25');");
			sqlBuilder.Append("Insert Into Neighbor values (51, 60,'Node 24',   61,'Node 24.5');");
			sqlBuilder.Append("Insert Into Neighbor values (52, 61,'Node 24.5',  7,'Ciencias Básicas II');");
			sqlBuilder.Append("Insert Into Neighbor values (53, 62,'Node 25',   63,'Node 25.5');");
			sqlBuilder.Append("Insert Into Neighbor values (54, 63,'Node 25.5',  5,'Departamentos de Ingeniería');");
			sqlBuilder.Append("Insert Into Neighbor values (55, 64,'Node 26',    7,'Ciencias Básicas II');");
			sqlBuilder.Append("Insert Into Neighbor values (56, 64,'Node 26',    8,'Aulas 1');");
			sqlBuilder.Append("Insert Into Neighbor values (57, 64,'Node 26',   65,'Node 27');");
			sqlBuilder.Append("Insert Into Neighbor values (58, 64,'Node 26',   66,'Node 28');");
			sqlBuilder.Append("Insert Into Neighbor values (59, 64,'Node 26',   67,'Node 29');");
			sqlBuilder.Append("Insert Into Neighbor values (60, 65,'Node 27',   66,'Node 28');");
			//sqlBuilder.Append("Insert Into Neighbor values (61, 65,'Node 27',   11,'Aulas 4');");
			sqlBuilder.Append("Insert Into Neighbor values (62, 66,'Node 28',    6,'Ciencias Básicas I');");
			sqlBuilder.Append("Insert Into Neighbor values (63, 67,'Node 29',   93,'Node 54');");
			sqlBuilder.Append("Insert Into Neighbor values (64, 67,'Node 29',   72,'Node 34');");
			sqlBuilder.Append("Insert Into Neighbor values (65, 68,'Node 30',   93,'Node 54');");
			sqlBuilder.Append("Insert Into Neighbor values (66, 68,'Node 30',    9,'Aulas 2');");
			sqlBuilder.Append("Insert Into Neighbor values (67, 69,'Node 31',    9,'Aulas 2');");
			sqlBuilder.Append("Insert Into Neighbor values (68, 69,'Node 31',   16,'Ciencias de la Salud');");
			sqlBuilder.Append("Insert Into Neighbor values (69, 69,'Node 31',   70,'Node 32');");
			sqlBuilder.Append("Insert Into Neighbor values (70, 70,'Node 32',   71,'Node 33');");
			sqlBuilder.Append("Insert Into Neighbor values (71, 70,'Node 32',   72,'Node 34');");
			sqlBuilder.Append("Insert Into Neighbor values (72, 71,'Node 33',   12,'Centro de Estudiantes');");
			sqlBuilder.Append("Insert Into Neighbor values (73, 71,'Node 33',   16,'Ciencias de la Salud');");
			sqlBuilder.Append("Insert Into Neighbor values (74, 72,'Node 34',   73,'Node 35');");
			sqlBuilder.Append("Insert Into Neighbor values (75, 73,'Node 35',   74,'Node 36');");
			sqlBuilder.Append("Insert Into Neighbor values (76, 73,'Node 35',   77,'Node 39');");
			sqlBuilder.Append("Insert Into Neighbor values (77, 74,'Node 36',   75,'Node 37');");
			sqlBuilder.Append("Insert Into Neighbor values (78, 74,'Node 36',   76,'Node 38');");
			sqlBuilder.Append("Insert Into Neighbor values (79, 74,'Node 36',   91,'Node 52');");
			sqlBuilder.Append("Insert Into Neighbor values (80, 75,'Node 37',   76,'Node 38');");
			sqlBuilder.Append("Insert Into Neighbor values (81, 75,'Node 37',   90,'Node 51');");
			sqlBuilder.Append("Insert Into Neighbor values (82, 76,'Node 38',   77,'Node 39');");
			sqlBuilder.Append("Insert Into Neighbor values (83, 77,'Node 39',   78,'Node 40');");
			sqlBuilder.Append("Insert Into Neighbor values (84, 78,'Node 40',   12,'Centro de Estudiantes');");
			sqlBuilder.Append("Insert Into Neighbor values (85, 78,'Node 40',   79,'Node 41');");
			sqlBuilder.Append("Insert Into Neighbor values (86, 79,'Node 41',   80,'Node 42');");
			sqlBuilder.Append("Insert Into Neighbor values (87, 80,'Node 42',   81,'Node 43');");
			sqlBuilder.Append("Insert Into Neighbor values (88, 80,'Node 42',   90,'Node 51');");
			sqlBuilder.Append("Insert Into Neighbor values (89, 81,'Node 43',   82,'Node 44');");
			sqlBuilder.Append("Insert Into Neighbor values (90, 81,'Node 43',   13,'Biblioteca');");
			sqlBuilder.Append("Insert Into Neighbor values (91, 82,'Node 44',   83,'Node 45');");
			sqlBuilder.Append("Insert Into Neighbor values (92, 82,'Node 44',   86,'Node 48');");
			sqlBuilder.Append("Insert Into Neighbor values (93, 83,'Node 45',   84,'Node 46');");
			sqlBuilder.Append("Insert Into Neighbor values (94, 84,'Node 46',   85,'Node 47');");
			sqlBuilder.Append("Insert Into Neighbor values (95, 86,'Node 48',   87,'Node 48.5');");
			sqlBuilder.Append("Insert Into Neighbor values (96, 87,'Node 48.5', 88,'Node 49');");

			//Aulas 4
			sqlBuilder.Append("Insert Into Neighbor values (97, 88,'Node 49',   89,'Node 50');");
			sqlBuilder.Append("Insert Into Neighbor values (98, 88,'Node 49',  103,'Node 64');");
			sqlBuilder.Append("Insert Into Neighbor values (99,103,'Node 64',   92,'Node 53');");
			sqlBuilder.Append("Insert Into Neighbor values (100, 89,'Node 50',  90,'Node 51');");
			sqlBuilder.Append("Insert Into Neighbor values (101, 90,'Node 51',  92,'Node 53');");
			sqlBuilder.Append("Insert Into Neighbor values (102, 90,'Node 51', 102,'Node 63');");
			sqlBuilder.Append("Insert Into Neighbor values (103,102,'Node 63',  91,'Node 52');");

			sqlBuilder.Append("Insert Into Neighbor values (104, 94,'Node 55',  34,'Node 4');");
			sqlBuilder.Append("Insert Into Neighbor values (105, 94,'Node 55',   3,'Suministro y Talleres');");
			sqlBuilder.Append("Insert Into Neighbor values (106, 95,'Node 56',  32,'Node 3');");
			sqlBuilder.Append("Insert Into Neighbor values (107, 95,'Node 56',   3,'Suministro y Talleres');");
			sqlBuilder.Append("Insert Into Neighbor values (108, 96,'Node 57',  42,'Node 9');");
			sqlBuilder.Append("Insert Into Neighbor values (109, 96,'Node 57',   3,'Suministro y Talleres');");
			sqlBuilder.Append("Insert Into Neighbor values (110, 97,'Node 58',  98,'Node 59');");
			sqlBuilder.Append("Insert Into Neighbor values (111, 97,'Node 58',   3,'Suministro y Talleres');");
			sqlBuilder.Append("Insert Into Neighbor values (112, 98,'Node 59',  41,'Node 8.5');");
			sqlBuilder.Append("Insert Into Neighbor values (113, 98,'Node 59',  42,'Node 9');");

			//Nodos Aulas 3 de Nodo 13 al Nodo 21
			sqlBuilder.Append("Insert Into Neighbor values (114, 99,'Node 60',  47,'Node 13');");
			sqlBuilder.Append("Insert Into Neighbor values (115, 99,'Node 60', 100,'Node 61');");
			sqlBuilder.Append("Insert Into Neighbor values (116,100,'Node 61',101,'Node 62');");
			sqlBuilder.Append("Insert Into Neighbor values (117,101,'Node 62', 56,'Node 21');");

			TransactionalQuery(sqlBuilder.ToString());

			#endregion
		}

		public IDataReader SelectQuery(string sqlQuery) 
		{
			IDataReader reader = null;
			try
			{
				using (var _databaseCommand = _databaseConnection.CreateCommand())
				{
					_databaseCommand.CommandText = sqlQuery; // fill the command

					reader = _databaseCommand.ExecuteReader(); // execute command which returns a reader
					Debug.Log("Query Executed: " + sqlQuery);
				}
			}
			catch (SqliteException e)
			{
				Debug.LogException(e);
			}

			return reader;
		}

		public void TransactionalQuery(string sqlQuery)
		{
			try
			{
				using (var transaction = _databaseConnection.BeginTransaction())
				{
					using (var command = new SqliteCommand(sqlQuery, _databaseConnection, transaction))
					{
						command.CommandText = sqlQuery; // fill the command
						command.ExecuteNonQuery();

						transaction.Commit();

						Debug.Log("Query Executed: " + sqlQuery);
					}
				}
				
			}
			catch (SqliteException e)
			{
				Debug.LogException(e);
			}
		}

		public string CreateTableQuery(string tableName,
									 Dictionary<string, string> columns,
									 Dictionary<string, string[]> primaryColumnsConstraint,
									 Dictionary<string, string[]> foreignColumnsConstraint)
		{
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
			
			query = query.Substring(0, query.Length - 1) + ");";

			return query;
		}

		private void DropAllModelTables()
		{
			StringBuilder sqlBuilder = new StringBuilder();

			sqlBuilder.Append("DROP TABLE Neighbor;");
			sqlBuilder.Append("DROP TABLE CoordenadaNodo;");
			sqlBuilder.Append("DROP TABLE Nodo;");
			sqlBuilder.Append("DROP TABLE Ubicacion;");

			TransactionalQuery(sqlBuilder.ToString());
		}

		public void UpdateModel(JSONObject json)
		{
			//Drop to Tables
			DropAllModelTables();

			StringBuilder sqlBuilder;

			//Create Tables
			InitCreateModelTables();

			sqlBuilder = new StringBuilder();
			if (json.HasField("Ubicacion"))
			{
				JSONObject ubicacionJsonList = json.GetField("Ubicacion");

				foreach (var ubicacionJson in ubicacionJsonList.list)
				{
					var ubicacion = new Ubicacion(ubicacionJson);
					sqlBuilder.Append("INSERT INTO Ubicacion VALUES (" + ubicacion.idUbicacion + ",'" + ubicacion.nombre + "','" + ubicacion.abreviacion + "'," + ubicacion.cantidadPlantas + ");");
				}

				TransactionalQuery(sqlBuilder.ToString());
			}

			sqlBuilder = new StringBuilder();
			if (json.HasField("Nodo"))
			{
				JSONObject nodoJsonList = json.GetField("Nodo");

				foreach (var nodoJson in nodoJsonList.list)
				{
					var nodo = new Nodo(nodoJson);
					sqlBuilder.Append("INSERT INTO Nodo VALUES (" + nodo.idNodo + "," + nodo.idUbicacion + "," + nodo.edificio + ",'" + nodo.nombre + "'," + (nodo.activo.Value ? 1 : 0).ToString() + ");");
				}

				TransactionalQuery(sqlBuilder.ToString());
			}

			sqlBuilder = new StringBuilder();
			if (json.HasField("CoordenadaNodo"))
			{
				JSONObject coordenadaNodoJsonList = json.GetField("CoordenadaNodo");

				foreach (var coordenadaNodoJson in coordenadaNodoJsonList.list)
				{
					var coordenadaNodo = new CoordenadaNodo(coordenadaNodoJson);
					sqlBuilder.Append("INSERT INTO CoordenadaNodo VALUES (" + coordenadaNodo.idCoordenadaNodo + "," + coordenadaNodo.nodo + "," + coordenadaNodo.latitud + "," + coordenadaNodo.longitud + ");");
				}

				TransactionalQuery(sqlBuilder.ToString());
			}

			sqlBuilder = new StringBuilder();
			if (json.HasField("Neighbor"))
			{
				JSONObject neighborJsonList = json.GetField("Neighbor");

				foreach (var neighborJson in neighborJsonList.list)
				{
					var neighbor = new Neighbor(neighborJson);
					sqlBuilder.Append("INSERT INTO Neighbor VALUES (" + neighbor.idNeighbor + "," + neighbor.nodo.idNodo + ",'" + neighbor.nodo.nombre + "'," + neighbor.nodoNeighbor.idNodo + ",'" + neighbor.nodoNeighbor.nombre + "');");
				}

				TransactionalQuery(sqlBuilder.ToString());
			}
		}

		public void UpdateTours(JSONObject json)
		{
			//Delete data from Tables

			StringBuilder sqlBuilder = new StringBuilder();

			sqlBuilder.Append("DELETE FROM Tour;");
			sqlBuilder.Append("DELETE FROM PuntoReunionTour;");
			sqlBuilder.Append("DELETE FROM UsuarioTour;");
			sqlBuilder.Append("DELETE FROM DetalleUsuarioTour;");
			sqlBuilder.Append("DELETE FROM UsuarioLocalizacion;");

			TransactionalQuery(sqlBuilder.ToString());

			if (json.HasField("Tours"))
			{
				sqlBuilder = new StringBuilder();

				JSONObject tourJsonList = json.GetField("Tours");

				foreach (var tourJson in tourJsonList.list)
				{
					var tour = new Tour(tourJson);

					sqlBuilder.Append("INSERT INTO Tour VALUES (" + 
						tour.idTour.Value +",'" + 
						tour.nombreTour +"','" + 
						(tour.fechaCreacion.HasValue ? tour.fechaCreacion.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) +"','"+
						(tour.fechaInicio.HasValue ? tour.fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) +"','"+
						(tour.fechaFin.HasValue ? tour.fechaFin.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) +"',"+
						tour.idUsuario.Value
					+");");


					if (tourJson.HasField("PuntosReunion"))
					{
						JSONObject PuntoReunionTourJsonList = tourJson.GetField("PuntosReunion");

						foreach (var PuntoReunionTourJson in PuntoReunionTourJsonList.list)
						{
							var puntoReunionTour = new PuntoReunionTour(PuntoReunionTourJson);
							sqlBuilder.Append("INSERT INTO PuntoReunionTour VALUES (" +
								puntoReunionTour.idPuntoReunionTour.Value + "," +
								puntoReunionTour.secuencia + "," +
								puntoReunionTour.idNodo.Value + "," +
								puntoReunionTour.idTour.Value
							+ ");");
						}
					}
				}

				TransactionalQuery(sqlBuilder.ToString());
			}

			if (json.HasField("UsuariosTours"))
			{
				sqlBuilder = new StringBuilder();

				JSONObject usuarioTourJsonList = json.GetField("UsuariosTours");

				foreach (var usuarioTourJson in usuarioTourJsonList.list)
				{
					var usuarioTour = new UsuarioTour(usuarioTourJson);
					sqlBuilder.Append("INSERT INTO UsuarioTour VALUES (" +
						usuarioTour.idUsuarioTour +",'"+ 
						usuarioTour.estado +"','"+ 
						(usuarioTour.fechaInicio.HasValue ? usuarioTour.fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) +"','"+ 
						(usuarioTour.fechaFin.HasValue ? usuarioTour.fechaFin.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) +"',"+
						usuarioTour.idTour.Value +","+
						usuarioTour.idUsuario.Value +",'"+
						usuarioTour.request
					+"');");

					if (usuarioTourJson.HasField("DetalleUsuarioTourList"))
					{
						JSONObject detalleUsuarioTourJsonList = usuarioTourJson.GetField("DetalleUsuarioTourList");

						foreach (var detalleUsuarioTourJson in detalleUsuarioTourJsonList.list)
						{
							var detalleUsuarioTour = new DetalleUsuarioTour(detalleUsuarioTourJson);
							detalleUsuarioTour.idUsuarioTour = usuarioTour.idUsuarioTour;
							sqlBuilder.Append("INSERT INTO DetalleUsuarioTour VALUES (" +
								detalleUsuarioTour.idDetalleUsuarioTour.Value + ",'" +
								(detalleUsuarioTour.fechaInicio.HasValue ? detalleUsuarioTour.fechaInicio.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) + "','" +
								(detalleUsuarioTour.fechaLlegada.HasValue ? detalleUsuarioTour.fechaLlegada.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) + "','" +
								(detalleUsuarioTour.fechaFin.HasValue ? detalleUsuarioTour.fechaFin.Value.ToString("dd/MM/yyyy HH:mm:ss") : null) + "'," +
								detalleUsuarioTour.idPuntoReunionTour.Value + "," +
								detalleUsuarioTour.idUsuarioTour.Value
							+ ");");
						}
					}
				}

				TransactionalQuery(sqlBuilder.ToString());
			}
		}

		public JSONObject DataSynchronization()
		{
			JSONObject json = new JSONObject(JSONObject.Type.OBJECT);

			#region UsuarioTourList

			JSONObject jsonUsuarioTourArray = new JSONObject(JSONObject.Type.ARRAY);

			#region UsuarioTour
			using (var resultUsuarioTour = SelectQuery("SELECT * FROM UsuarioTour"))
			{
				while (resultUsuarioTour.Read())
				{
					DateTime? startDate = null, endDate = null;
					DateTime temp;
					object obj;

					obj = resultUsuarioTour["fechaInicio"];
					if (obj != null)
					{
						var fechaInicio = Convert.ToString(obj);
						if (DateTime.TryParseExact(fechaInicio, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
						{
							startDate = temp;
						}
					}

					obj = resultUsuarioTour["fechaFin"];
					if (obj != null)
					{
						var fechaFin = Convert.ToString(obj);
						if (DateTime.TryParseExact(fechaFin, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
						{
							endDate = temp;
						}
					}

					var usuarioTour = new UsuarioTour()
					{
						estado = Convert.ToString(resultUsuarioTour["estado"]),
						fechaInicio = startDate,
						fechaFin = endDate,
						idTour = Convert.ToInt32(resultUsuarioTour["idTour"]),
						idUsuario = Convert.ToInt32(resultUsuarioTour["idUsuario"]),
						request = Convert.ToString(resultUsuarioTour["request"])
					};

					obj = null;

					#region DetalleUsuarioTour

					var jsonDetalleUsuarioList = new JSONObject(JSONObject.Type.ARRAY);

					int idUsuarioTour = Convert.ToInt32(resultUsuarioTour["id"]);
					using (var resultDetalleUsuarioTour = SelectQuery("SELECT * FROM DetalleUsuarioTour WHERE idUsuarioTour = " + idUsuarioTour))
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
								var fechaLlegada = Convert.ToString(obj);
								if (DateTime.TryParseExact(fechaLlegada, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
								{
									startDate = temp;
								}
							}

							obj = resultDetalleUsuarioTour["fechaFin"];
							if (obj != null)
							{
								var fechaFin = Convert.ToString(obj);
								if (DateTime.TryParseExact(fechaFin, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
								{
									startDate = temp;
								}
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
					}

					#endregion

					var jsonUsuarioTour = new JSONObject(JSONObject.Type.OBJECT);

					jsonUsuarioTour.AddField("UsuarioTour", usuarioTour.ToJson());
					jsonUsuarioTour.AddField("DetalleUsuarioTourList", jsonDetalleUsuarioList);

					jsonUsuarioTourArray.Add(jsonUsuarioTour);
				}
			}

			#endregion

			#endregion

			#region UsuarioLocalizacionList

			JSONObject jsonUsuarioLocalizacionArray = new JSONObject(JSONObject.Type.ARRAY);

			using (var resultUsuarioLocalizacion = SelectQuery("SELECT * FROM UsuarioLocalizacion"))
			{
				while (resultUsuarioLocalizacion.Read())
				{
					DateTime? localizationDate = null;
					object obj;
					DateTime temp;

					obj = resultUsuarioLocalizacion["fechaLocalizacion"];
					if (obj != null)
					{
						string fechaLocalizacion = Convert.ToString(obj);
						if (DateTime.TryParseExact(fechaLocalizacion, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
						{
							localizationDate = temp;
						}
					}
					
					var usuarioLocalizacion = new LocalizacionUsuario()
					{
						idNodo = Convert.ToInt32(resultUsuarioLocalizacion["idNodo"]),
						idUsuario = Convert.ToInt32(resultUsuarioLocalizacion["idUsuario"]),
						fechaLocalizacion = localizationDate
					};

					jsonUsuarioLocalizacionArray.Add(usuarioLocalizacion.ToJson());
				}
			}

			#endregion

			json.AddField("UsuarioTourList", jsonUsuarioTourArray);
			json.AddField("UsuarioLocalizacionList", jsonUsuarioLocalizacionArray);

			return json;
		}

		public void Dispose()
		{
			_databaseConnection.Close();
			_databaseConnection = null;
		}

		#endregion
	}
}