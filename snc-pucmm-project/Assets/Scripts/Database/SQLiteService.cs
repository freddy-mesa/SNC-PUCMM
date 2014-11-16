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

		#endregion

		#region Constructor
		
		public SQLiteService()
		{
			OpenDateBaseConnection();
		}

		#endregion

		#region Metodos

		private void OpenDateBaseConnection() 
		{
			string connectionString = String.Empty;
			bool createDataBase = false;

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
					createDataBase = true;
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
			StringBuilder sqlBuilder = new StringBuilder();

			#region Ubicacion
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

			#region Edificaciones

			sqlBuilder = new StringBuilder();

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

			TransactionalQuery(sqlBuilder.ToString());

			#endregion

			#region Caminos en el Campus

			sqlBuilder = new StringBuilder();

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

			TransactionalQuery(sqlBuilder.ToString());

			#endregion

			#region Aulas 3 -> 300

			sqlBuilder = new StringBuilder();

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

			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (319, 'AULAS3_Nodo1', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (320, 'AULAS3_Nodo2', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (321, 'AULAS3_Nodo3', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (322, 'AULAS3_Nodo4', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (323, 'AULAS3_Nodo5', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (324, 'AULAS3_Nodo6', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (325, 'AULAS3_Nodo7', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (326, 'AULAS3_Nodo8', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (327, 'AULAS3_Nodo9', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (328, 'AULAS3_Nodo10', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (329, 'AULAS3_Nodo11', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (330, 'AULAS3_Nodo12', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (331, 'AULAS3_Nodo13', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (332, 'AULAS3_Nodo14', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (333, 'AULAS3_Nodo15', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (334, 'AULAS3_Nodo16', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (335, 'AULAS3_Nodo17', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (336, 'AULAS3_Nodo18', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (337, 'AULAS3_Nodo19', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (338, 'AULAS3_Nodo20', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (339, 'AULAS3_Nodo21', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (340, 'AULAS3_Nodo22', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (341, 'AULAS3_Nodo23', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (342, 'AULAS3_Nodo24', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (343, 'AULAS3_Nodo25', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (344, 'AULAS3_Nodo26', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (345, 'AULAS3_Nodo27', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (346, 'AULAS3_Nodo28', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (347, 'AULAS3_Nodo29', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (348, 'AULAS3_Nodo30', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (349, 'AULAS3_Nodo31', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (350, 'AULAS3_Nodo32', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (351, 'AULAS3_Nodo33', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (352, 'AULAS3_Nodo34', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (353, 'AULAS3_Nodo35', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (354, 'AULAS3_Nodo36', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (355, 'AULAS3_Nodo37', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (356, 'AULAS3_Nodo38', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (357, 'AULAS3_Nodo39', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (358, 'AULAS3_Nodo40', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (359, 'AULAS3_Nodo41', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (360, 'AULAS3_Nodo42', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (361, 'AULAS3_Nodo43', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (362, 'AULAS3_Nodo44', 1);");
			sqlBuilder.Append("Insert Into Nodo (idNodo, nombre, activo) values (363, 'AULAS3_Nodo45', 1);");

			TransactionalQuery(sqlBuilder.ToString());

			#region Planta1

			sqlBuilder = new StringBuilder();

			//sqlBuilder.Append("Insert Into Neighbor values (300, 318,'AULAS3_Nodo1', ,'');");
			//sqlBuilder.Append("Insert Into Neighbor values (301, 318,'AULAS3_Nodo1', ,'');");
			//sqlBuilder.Append("Insert Into Neighbor values (302, 318,'AULAS3_Nodo1', ,'');");
			sqlBuilder.Append("Insert Into Neighbor values (303, 319,'AULAS3_Nodo1', 320,'AULAS3_Nodo2');");
			sqlBuilder.Append("Insert Into Neighbor values (304, 320,'AULAS3_Nodo2', 300,'A3-11');");
			sqlBuilder.Append("Insert Into Neighbor values (305, 319,'AULAS3_Nodo1', 321,'AULAS3_Nodo3');");
			sqlBuilder.Append("Insert Into Neighbor values (306, 321,'AULAS3_Nodo3', 322,'AULAS3_Nodo4');");
			sqlBuilder.Append("Insert Into Neighbor values (307, 322,'AULAS3_Nodo4', 323,'AULAS3_Nodo5');");
			sqlBuilder.Append("Insert Into Neighbor values (308, 322,'AULAS3_Nodo4', 301,'A3-11');");
			sqlBuilder.Append("Insert Into Neighbor values (309, 323,'AULAS3_Nodo5', 324,'AULAS3_Nodo6');");
			sqlBuilder.Append("Insert Into Neighbor values (310, 324,'AULAS3_Nodo6', 325,'AULAS3_Nodo7');");
			sqlBuilder.Append("Insert Into Neighbor values (311, 325,'AULAS3_Nodo7', 326,'AULAS3_Nodo8');");
			sqlBuilder.Append("Insert Into Neighbor values (312, 326,'AULAS3_Nodo8', 327,'AULAS3_Nodo9');");
			sqlBuilder.Append("Insert Into Neighbor values (313, 327,'AULAS3_Nodo9', 328,'AULAS3_Nodo10');");
			sqlBuilder.Append("Insert Into Neighbor values (314, 327,'AULAS3_Nodo9', 318,'A3-Bedel');");
			sqlBuilder.Append("Insert Into Neighbor values (315, 328,'AULAS3_Nodo10', 329,'AULAS3_Nodo11');");
			sqlBuilder.Append("Insert Into Neighbor values (316, 328,'AULAS3_Nodo10', 302,'A3-14');");
			sqlBuilder.Append("Insert Into Neighbor values (317, 329,'AULAS3_Nodo11', 303,'A3-15');");

			sqlBuilder.Append("Insert Into Neighbor values (318, 330,'AULAS3_Nodo12', 324,'AULAS3_Nodo6');");
			sqlBuilder.Append("Insert Into Neighbor values (319, 330,'AULAS3_Nodo12', 325,'AULAS3_Nodo7');");
			sqlBuilder.Append("Insert Into Neighbor values (320, 330,'AULAS3_Nodo12', 326,'AULAS3_Nodo8');");

			sqlBuilder.Append("Insert Into Neighbor values (321, 331,'AULAS3_Nodo13', 324,'AULAS3_Nodo6');");
			sqlBuilder.Append("Insert Into Neighbor values (322, 331,'AULAS3_Nodo13', 332,'AULAS3_Nodo14');");
			//Insertar con la parte exterior
			//sqlBuilder.Append("Insert Into Neighbor values (323, 332,'AULAS3_Nodo14', ,'');");

			sqlBuilder.Append("Insert Into Neighbor values (324, 333,'AULAS3_Nodo15', 326,'AULAS3_Nodo8');");
			sqlBuilder.Append("Insert Into Neighbor values (325, 333,'AULAS3_Nodo15', 334,'AULAS3_Nodo16');");
			//Insertar con la parte exterior
			//sqlBuilder.Append("Insert Into Neighbor values (326, 334,'AULAS3_Nodo16', ,'');");

			sqlBuilder.Append("Insert Into Neighbor values (327, 335,'AULAS3_Nodo17', 330,'AULAS3_Nodo12');");
			//Insertar con la parte exterior
			//sqlBuilder.Append("Insert Into Neighbor values (328, 335,'AULAS3_Nodo17', ,'');");

			sqlBuilder.Append("Insert Into Neighbor values (329, 336,'AULAS3_Nodo18', 330,'AULAS3_Nodo12');");
			//Insertar con la parte exterior
			//sqlBuilder.Append("Insert Into Neighbor values (330, 336,'AULAS3_Nodo18', ,'');");

			//Insertar con la parte exterior
			//sqlBuilder.Append("Insert Into Neighbor values (331, ,'', 304,'A3-16');");

			//Insertar con la parte exterior
			//sqlBuilder.Append("Insert Into Neighbor values (332, 337,'AULAS3_Nodo19', ,'');");
			//sqlBuilder.Append("Insert Into Neighbor values (333, 337,'AULAS3_Nodo19', 305,'A3-17');");

			sqlBuilder.Append("Insert Into Neighbor values (334, 338,'AULAS3_Nodo20', 331,'AULAS3_Nodo13');");
			sqlBuilder.Append("Insert Into Neighbor values (335, 338,'AULAS3_Nodo20', 339,'AULAS3_Nodo21');");
			sqlBuilder.Append("Insert Into Neighbor values (336, 338,'AULAS3_Nodo21', 330,'AULAS3_Nodo12');");
			sqlBuilder.Append("Insert Into Neighbor values (337, 338,'AULAS3_Nodo20', 322,'AULAS3_Nodo4');");

			TransactionalQuery(sqlBuilder.ToString());

			#endregion

			#region Planta2

			sqlBuilder = new StringBuilder();

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

			sqlBuilder.Append("Insert Into Neighbor values (351, 344,'AULAS3_Nodo26', 349,'AULAS3_Nodo31');");
			sqlBuilder.Append("Insert Into Neighbor values (352, 349,'AULAS3_Nodo31', 350,'AULAS3_Nodo32');");
			sqlBuilder.Append("Insert Into Neighbor values (353, 350,'AULAS3_Nodo32', 308,'A3-23');");
			sqlBuilder.Append("Insert Into Neighbor values (354, 350,'AULAS3_Nodo32', 351,'AULAS3_Nodo33');");
			sqlBuilder.Append("Insert Into Neighbor values (355, 351,'AULAS3_Nodo33', 309,'A3-24');");
			sqlBuilder.Append("Insert Into Neighbor values (356, 351,'AULAS3_Nodo33', 352,'AULAS3_Nodo34');");
			sqlBuilder.Append("Insert Into Neighbor values (357, 352,'AULAS3_Nodo34', 310,'A3-25');");

			TransactionalQuery(sqlBuilder.ToString());

			#endregion

			#region Planta3

			sqlBuilder = new StringBuilder();

			sqlBuilder.Append("Insert Into Neighbor values (358, 345,'AULAS3_Nodo27', 353,'AULAS3_Nodo35');");
			sqlBuilder.Append("Insert Into Neighbor values (359, 353,'AULAS3_Nodo35', 354,'AULAS3_Nodo36');");
			sqlBuilder.Append("Insert Into Neighbor values (360, 354,'AULAS3_Nodo36', 317,'Salon de Dibujo II');");
			sqlBuilder.Append("Insert Into Neighbor values (361, 354,'AULAS3_Nodo36', 355,'AULAS3_Nodo37');");
			sqlBuilder.Append("Insert Into Neighbor values (362, 354,'AULAS3_Nodo37', 317,'Salon de Dibujo II');");
			sqlBuilder.Append("Insert Into Neighbor values (363, 354,'AULAS3_Nodo37', 356,'AULAS3_Nodo38');");
			sqlBuilder.Append("Insert Into Neighbor values (364, 356,'AULAS3_Nodo38', 357,'AULAS3_Nodo39');");
			sqlBuilder.Append("Insert Into Neighbor values (365, 357,'AULAS3_Nodo39', 357,'AULAS3_Nodo40');");

			sqlBuilder.Append("Insert Into Neighbor values (366, 358,'AULAS3_Nodo40', 359,'AULAS3_Nodo41');");
			sqlBuilder.Append("Insert Into Neighbor values (367, 359,'AULAS3_Nodo41', 313,'A3-32');");
			sqlBuilder.Append("Insert Into Neighbor values (368, 359,'AULAS3_Nodo41', 360,'AULAS3_Nodo42');");
			sqlBuilder.Append("Insert Into Neighbor values (369, 360,'AULAS3_Nodo42', 312,'A3-31');");

			sqlBuilder.Append("Insert Into Neighbor values (370, 358,'AULAS3_Nodo40', 361,'AULAS3_Nodo43');");
			sqlBuilder.Append("Insert Into Neighbor values (371, 361,'AULAS3_Nodo43', 314,'A3-33');");
			sqlBuilder.Append("Insert Into Neighbor values (372, 361,'AULAS3_Nodo43', 362,'AULAS3_Nodo44');");
			sqlBuilder.Append("Insert Into Neighbor values (373, 362,'AULAS3_Nodo44', 315,'A3-34');");
			sqlBuilder.Append("Insert Into Neighbor values (374, 362,'AULAS3_Nodo44', 363,'AULAS3_Nodo45');");
			sqlBuilder.Append("Insert Into Neighbor values (375, 363,'AULAS3_Nodo45', 316,'A3-35');");

			TransactionalQuery(sqlBuilder.ToString());

			#endregion

			#endregion

			#endregion

			#region CoordenadaNodo

			sqlBuilder = new StringBuilder();

			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 1, 8, 19.442731,-70.683049);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 2, 9, 19.443009,-70.681736);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 3, 10, 19.441522,-70.683402);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 4, 11, 19.443083, -70.683407);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 5, 12, 19.443879, -70.682780);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 6, 16, 19.443699, -70.681666);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 7, 6, 19.442237, -70.683398);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 8, 7, 19.442070, -70.683046);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 9, 14, 19.442303, -70.684772);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 10, 5, 19.441756, -70.683045);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 11, 4, 19.441074, -70.682723);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 12, 1, 19.440198, -70.683129);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 13, 3, 19.440648, -70.683352);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 14, 13, 19.443727, -70.684183);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 15, 2, 19.440310, -70.682706);");

			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 30, 30, 19.440313, -70.683129);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 31, 31, 19.440400, -70.683125);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 32, 32, 19.440573, -70.683132);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 33, 33, 19.440544, -70.683199);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 34, 34, 19.440697, -70.683132);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 35, 35, 19.440652, -70.683205);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 36, 36, 19.440788, -70.682727);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 37, 37, 19.440315, -70.682848);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 38, 38, 19.440409, -70.682754);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 39, 39, 19.440293, -70.683218);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 40, 40, 19.440270, -70.683475);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 41, 41, 19.440301, -70.683658);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 42, 42, 19.440711, -70.683580);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 43, 43, 19.440665, -70.683498);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 44, 44, 19.441075, -70.683132);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 45, 45, 19.441135, -70.683132);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 46, 46, 19.441468, -70.683127);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 47, 47, 19.441459, -70.683335);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 48, 48, 19.441140, -70.682723);");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 49, 49, );");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 50, 50, 19.441480, -70.682723);");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 51, 51, );");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 52, 52, 19.441679, -70.682722);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 53, 53, 19.441684, -70.683045);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 54, 54, 19.441683, -70.683398);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 55, 55, 19.441608, -70.683400);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 56, 56, 19.441608, -70.683400);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 57, 57, 19.441145, -70.683655);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 58, 58, 19.441930, -70.683395);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 59, 59, 19.441930, -70.683140);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 60, 60, 19.441979, -70.683082);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 61, 61, 19.442012, -70.683082);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 62, 62, 19.441850, -70.683060);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 63, 63, 19.441853, -70.683066);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 64, 64, 19.442589, -70.683049);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 65, 65, 19.442570, -70.683402);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 66, 66, 19.442305, -70.683405);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 67, 67, 19.442743, -70.682186);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 68, 68, 19.442811, -70.681733);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 69, 69, 19.443074, -70.681760);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 70, 70, 19.442917, -70.681744);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 71, 71, 19.443790, -70.682307);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 72, 72, 19.443403, -70.682822);");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 73, 73, );");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 74, 74, );");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 75, 75, 19.443418, -70.683270);");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 76, 76, );");
			//sqlBuilder.Append("Insert Into CoordenadaNodo values ( 77, 77, );");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 78, 78, 19.443836, -70.683149);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 79, 79, 19.443775, -70.683777);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 80, 80, 19.443732, -70.683898);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 81, 81, 19.443671, -70.684035);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 82, 82, 19.443652, -70.684129);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 83, 83, 19.443666, -70.684180);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 84, 84, 19.443378, -70.684180);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 85, 85, 19.443375, -70.684515);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 86, 86, 19.443272, -70.684155);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 87, 87, 19.442963, -70.683967);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 88, 88, 19.443165, -70.683640);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 89, 89, 19.443158, -70.683489);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 90, 90, 19.443319, -70.683390);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 91, 91, 19.443167, -70.683107);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 92, 92, 19.443163, -70.683390);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 93, 93, 19.442731, -70.682021);");

			//New Nodes
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 94, 94, 19.440695, -70.683214);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 95, 95, 19.440589, -70.683217);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 96, 96, 19.440715, -70.683501);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 97, 97, 19.440581, -70.683506);");
			sqlBuilder.Append("Insert Into CoordenadaNodo values ( 98, 98, 19.440582, -70.683577);");

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
			sqlBuilder.Append("Insert Into Neighbor values (22, 42,'Node 9',    56,'Node 21');");
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
			sqlBuilder.Append("Insert Into Neighbor values (33, 47,'Node 13',   10,'Aulas 3');");
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
			sqlBuilder.Append("Insert Into Neighbor values (43, 55,'Node 20.5', 10,'Aulas 3');");
			sqlBuilder.Append("Insert Into Neighbor values (44, 56,'Node 21',   57,'Node 21.5');");
			sqlBuilder.Append("Insert Into Neighbor values (45, 56,'Node 21',   10,'Aulas 3');");
			sqlBuilder.Append("Insert Into Neighbor values (46, 58,'Node 22',   59,'Node 23');");
			sqlBuilder.Append("Insert Into Neighbor values (47, 58,'Node 22',    6,'iencias Básicas I');");
			sqlBuilder.Append("Insert Into Neighbor values (48, 59,'Node 23',   60,'Node 24');");
			sqlBuilder.Append("Insert Into Neighbor values (49, 59,'Node 23',   62,'Node 25');");
			sqlBuilder.Append("Insert Into Neighbor values (50, 60,'Node 24',   62,'Node 25');");
			sqlBuilder.Append("Insert Into Neighbor values (51, 60,'Node 24',   61,'Node 24.5');");
			sqlBuilder.Append("Insert Into Neighbor values (52, 61,'Node 24.5',  7,'Ciencias Básicas II');");
			sqlBuilder.Append("Insert Into Neighbor values (53, 62,'Node 25',   63,'Node 25.5');");
			sqlBuilder.Append("Insert Into Neighbor values (54, 63,'Node 25.5',  5,'Departamentos de Ingeniería');");
			sqlBuilder.Append("Insert Into Neighbor values (55, 64,'Node 26',    7,'iencias Básicas II');");
			sqlBuilder.Append("Insert Into Neighbor values (56, 64,'Node 26',    8,'Aulas 1');");
			sqlBuilder.Append("Insert Into Neighbor values (57, 64,'Node 26',   65,'Node 27');");
			sqlBuilder.Append("Insert Into Neighbor values (58, 64,'Node 26',   66,'Node 28');");
			sqlBuilder.Append("Insert Into Neighbor values (59, 64,'Node 26',   67,'Node 29');");
			sqlBuilder.Append("Insert Into Neighbor values (60, 65,'Node 27',   66,'Node 28');");
			sqlBuilder.Append("Insert Into Neighbor values (61, 65,'Node 27',   11,'Aulas 4');");
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
			sqlBuilder.Append("Insert Into Neighbor values (97, 88,'Node 49',   89,'Node 50');");
			sqlBuilder.Append("Insert Into Neighbor values (98, 88,'Node 49',   90,'Node 51');");
			sqlBuilder.Append("Insert Into Neighbor values (99, 89,'Node 50',   11,'Aulas 4');");
			sqlBuilder.Append("Insert Into Neighbor values (100, 90,'Node 51',  92,'Node 53');");
			sqlBuilder.Append("Insert Into Neighbor values (101, 91,'Node 52',  92,'Node 53');");
			sqlBuilder.Append("Insert Into Neighbor values (102, 92,'Node 53',  11,'Aulas 4');");

			sqlBuilder.Append("Insert Into Neighbor values (103, 94,'Node 55',    34,'Node 4');");
			sqlBuilder.Append("Insert Into Neighbor values (104, 94,'Node 55',     3,'Suministro y Talleres');");
			sqlBuilder.Append("Insert Into Neighbor values (105, 95,'Node 56',    32,'Node 3');");
			sqlBuilder.Append("Insert Into Neighbor values (106, 95,'Node 56',     3,'Suministro y Talleres');");
			sqlBuilder.Append("Insert Into Neighbor values (107, 96,'Node 57',    42,'Node 9');");
			sqlBuilder.Append("Insert Into Neighbor values (108, 96,'Node 57',     3,'Suministro y Talleres');");
			sqlBuilder.Append("Insert Into Neighbor values (109, 97,'Node 58',    98,'Node 59');");
			sqlBuilder.Append("Insert Into Neighbor values (110, 97,'Node 58',     3,'Suministro y Talleres');");
			sqlBuilder.Append("Insert Into Neighbor values (111, 98,'Node 59',    41,'Node 8.5');");
			sqlBuilder.Append("Insert Into Neighbor values (112, 98,'Node 59',    42,'Node 9');");

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