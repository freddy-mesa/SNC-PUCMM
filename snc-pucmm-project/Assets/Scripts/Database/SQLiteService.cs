using System;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

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
                InitializeDataBase();
            }
		}

        private void InitializeDataBase() 
        {
            InitCreateTables();
            InitInsertInTables();
        }

		private void InitCreateTables()
		{
			//TipoUsuario
			CreateTableQuery(
				"TipoUsuario",
				new Dictionary<string, string>{
					{"idTipoUsuario","integer"},
					{"nombre","text"},
					{"descripcion","text"}
				},
				new Dictionary<string, string[]>{
					{"PK_TipoUsuario", new string[] { "idTipoUsuario" }}
				},
				new Dictionary<string, string[]>{}
			);
			
			//CuentaFacebook
			CreateTableQuery(
				"CuentaFacebook",
				new Dictionary<string, string>{
					{"idCuentaFacebook","integer"},
					{"usuarioFacebook","text"},
					{"token","text"}
				},
				new Dictionary<string, string[]>{
					{"PK_CuentaFacebook", new string[] { "idCuentaFacebook" }}
				},
				new Dictionary<string, string[]> { }
			);
			
			//Usuario
			CreateTableQuery(
				"Usuario",
				new Dictionary<string, string>{
					{"idUsuario","integer"},
					{"idTipoUsuario","integer"},
					{"idCuentaFacebook","integer"},
					{"nombre","text"},
					{"apellido","text"},
					{"usuario","text"},
					{"contrasena","text"},
				},
				new Dictionary<string, string[]>{
					{"PK_Usuario", new string[] { "idUsuario" }}
				},
				new Dictionary<string, string[]> {
					{"FK_Usuario_TipoUsuario", new string[] { "idTipoUsuario","TipoUsuario","idTipoUsuario" }},
					{"FK_Usuario_CuentaFacebook", new string[] { "idCuentaFacebook","CuentaFacebook","idCuentaFacebook" }}
				}
			);
			
			//Follow
			CreateTableQuery(
				"Follow",
				new Dictionary<string, string>{
					{"idFollow","integer"},
					{"idUsuarioFollower","integer"},
					{"idUsuarioFollowed","integer"},
					{"estadoSolicitud","text"},
					{"fechaRegistroSolicitud","text"},
					{"fechaRespuestaSolicitud","text"},
				},
				new Dictionary<string, string[]>{
					{"PK_Follow", new string[] { "idFollow" }}
				},
				new Dictionary<string, string[]> {
					{"FK_Follow_TipoUsuario", new string[] { "idUsuarioFollower","Usuario","idUsuario" }},
					{"FK_Follow_CuentaFacebook", new string[] { "idUsuarioFollowed","Usuario","idUsuario" }}
				}
			);
			
			//VideoLlamada
			CreateTableQuery(
				"VideoLlamada",
				new Dictionary<string, string>{
					{"idVideoLlamada","integer"},
					{"idUsuario","integer"},
					{"fechaInicio","text"},
					{"fechaFin","text"},
					{"plataforma","text"},
					{"longitud","real"},
					{"latitud","real"},
				},
				new Dictionary<string, string[]>{
					{"PK_VideoLlamada", new string[] { "idVideoLlamada" }}
				},
				new Dictionary<string, string[]> {
					{"FK_VideoLlamada_Usuario", new string[] { "idUsuario","Usuario","idUsuario" }},
				}
			);
			
			//Ubicacion
			CreateTableQuery(
				"Ubicacion",
				new Dictionary<string, string>{
					{"idUbicacion","integer"},
					{"nombre","text"},
					{"abreviacion","text"}
				},
				new Dictionary<string, string[]>{
					{"PK_Ubicacion", new string[] { "idUbicacion" }}
				},
				new Dictionary<string, string[]> { }
			);
			
			//Localizacion
			CreateTableQuery(
				"Localizacion",
				new Dictionary<string, string>{
					{"idLocalizacion","integer"},
					{"idUbicacion","integer"},
					{"nombre","text"},
				},
				new Dictionary<string, string[]>{
					{"PK_Localizacion", new string[] { "idLocalizacion" }}
				},
				new Dictionary<string, string[]>{
					{"FK_Localizacion_Ubicacion", new string[] { "idUbicacion","Ubicacion","idUbicacion" }}
				}
			);
			
			//LocalizacionUsuario
			CreateTableQuery(
				"LocalizacionUsuario",
				new Dictionary<string, string>{
					{"idLocalizacionUsuario","integer"},
					{"idUsuario","integer"},
					{"idLocalizacion","integer"},
					{"fechaLocalizacion","text"},
				},
				new Dictionary<string, string[]>{
					{"PK_LocalizacionUsuario", new string[] { "idLocalizacionUsuario" }}
				},
				new Dictionary<string, string[]> {
					{"FK_LocalizacionUsuario_Usuario", new string[] { "idUsuario","Usuario","idUsuario" }},
					{"FK_LocalizacionUsuario_Localizacion", new string[] { "idLocalizacion","Localizacion","idLocalizacion" }}
				}
			);
			
			//Tour
			CreateTableQuery(
				"Tour",
				new Dictionary<string,string>{
					{"idTour","integer"},
					{"idUsuario","integer"},
					{"nombreTour","text"},
					{"fechaCreacion","text"},
					{"fechaInicio","text"},
					{"fechaFin","text"}
				},
				new Dictionary<string,string[]>{
					{"PK_Tour", new string[] { "idTour" }}
				},
				new Dictionary<string, string[]>{
					{"FK_Tour_Usuario", new string[] { "idUsuario","Usuario","idUsuario" }}
				}
			);
			
			//UsuarioTour
			CreateTableQuery(
				"UsuarioTour",
				new Dictionary<string, string>{
					{"idUsuarioTour","integer"},
					{"idUsuario","integer"},
					{"idTour","integer"},
					{"estadoUsuarioTour","text"},
					{"fechaInicio","text"},
					{"fechaFin","text"},
				},
				new Dictionary<string, string[]>{
					{"PK_UsuarioTour", new string[] { "idUsuarioTour" }}
				},
				new Dictionary<string, string[]> {
					{"FK_UsuarioTour_Usuario", new string[] { "idUsuario","Usuario","idUsuario" }},
					{"FK_UsuarioTour_Tour", new string[] { "idTour","Tour","idTour" }}
				}
			);
			
			//PuntoReunionTour
			CreateTableQuery(
				"PuntoReunionTour",
				new Dictionary<string, string>{
					{"idPuntoReunion","integer"},
					{"idLocalizacion","integer"},
					{"idTour","integer"},
					{"secuenciaPuntoReunion","integer"},
				},
				new Dictionary<string, string[]>{
					{"PK_PuntoReunionTour", new string[] { "idPuntoReunion" }}
				},
				new Dictionary<string, string[]> {
					{"FK_PuntoReunionTour_Localizacion", new string[] { "idLocalizacion","Localizacion","idLocalizacion" }},
					{"FK_PuntoReunionTour_Tour", new string[] { "idTour","Tour","idTour" }}
				}
			);
			
			//DetalleUsuarioTour
			CreateTableQuery(
				"DetalleUsuarioTour",
				new Dictionary<string, string>{
					{"idDetalleUsuarioTour","integer"},
					{"idUsuarioTour","integer"},
					{"idPuntoReunion","integer"},
					{"estado","text"},
					{"fechaInicio","text"},
					{"fechaLlegada","text"},
				},
				new Dictionary<string, string[]>{
					{"PK_DetalleUsuarioTour", new string[] { "idDetalleUsuarioTour" }}
				},
				new Dictionary<string, string[]> {
					{"FK_DetalleUsuarioTour_UsuarioTour", new string[] { "idUsuarioTour","UsuarioTour","idUsuarioTour" }},
					{"FK_DetalleUsuarioTour_PuntoReunionTour", new string[] { "idPuntoReunion","PuntoReunionTour","idPuntoReunion" }}
				}
			);
			
			//CoordenadaLocalizacion
			CreateTableQuery(
				"CoordenadaLocalizacion",
				new Dictionary<string, string>{
					{"idCoordenada","integer"},
					{"idLocalizacion","integer"},
					{"longitud","real"},
					{"latitud","real"},
				},
				new Dictionary<string, string[]>{
					{"PK_CoordenadaLocalizacion", new string[] { "idCoordenada" }}
				},
				new Dictionary<string, string[]>{
					{"FK_CoordenadaLocalizacion_Localizacion", new string[] { "idLocalizacion","Localizacion","idLocalizacion" }}
				}
			);
		}

        private void InitInsertInTables()
        {

            #region Ubicacion
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",1},{"nombre","Departamento de Ingenierías Electrónica y Electromecánica"},{"abreviacion","MATADERO"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",2},{"nombre","Talleres de Ingeniería Eléctrica y Electromecánica"},{"abreviacion","TALLERIEE"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",3},{"nombre","Suministro"},{"abreviacion","SUMINISTRO"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",4},{"nombre","Laboratorios de Ingeniería"},{"abreviacion","LABING"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",5},{"nombre","Departamentos de Ingeniería"},{"abreviacion","DEPING"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",6},{"nombre","Ciencias Básicas I"},{"abreviacion","CIENBASI"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",7},{"nombre","Ciencias Básicas II"},{"abreviacion","CIENBASII"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",8},{"nombre","Aulas 1"},{"abreviacion","AULAS1"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",9},{"nombre","Aulas 2"},{"abreviacion","AULAS2"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",10},{"nombre","Aulas 3"},{"abreviacion","AULAS3"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",11},{"nombre","Aulas 4"},{"abreviacion","AULAS4"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",12},{"nombre","Centro de Estudiantes"},{"abreviacion","CENTROEST"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",13},{"nombre","Biblioteca"},{"abreviacion","BIBLIOTECA"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",14},{"nombre","Padre Arroyo"},{"abreviacion","PADARROYO"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",15},{"nombre","Edificio Administrativo"},{"abreviacion","ADMINISTRACION"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",16},{"nombre","Ciencias de la Salud"},{"abreviacion","CIENSALUD"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",17},{"nombre","Salón Multiusos"},{"abreviacion","MULTIUSOS"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",18},{"nombre","Centro de Tecnología y Educación Permanente"},{"abreviacion","TEP"}
            });
            InsertQuery("Ubicacion", new Dictionary<string, object> {
                {"idUbicacion",19},{"nombre","Kiosko Universitario"},{"abreviacion","KIOSKO"}
            });
            #endregion

            #region Localizacion
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",1},{"idUbicacion",1},{"nombre","Departamento de Ingenierías Electrónica y Electromecánica"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",2},{"idUbicacion",2},{"nombre","Talleres de Ingeniería Eléctrica y Electromecánica"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",3},{"idUbicacion",3},{"nombre","Suministro"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",4},{"idUbicacion",4},{"nombre","Laboratorios de Ingeniería"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",5},{"idUbicacion",5},{"nombre","Departamentos de Ingeniería"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",6},{"idUbicacion",6},{"nombre","Ciencias Básicas I"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",7},{"idUbicacion",7},{"nombre","Ciencias Básicas II"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",8},{"idUbicacion",8},{"nombre","Aulas 1"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",9},{"idUbicacion",9},{"nombre","Aulas 2"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",10},{"idUbicacion",10},{"nombre","Aulas 3"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",11},{"idUbicacion",11},{"nombre","Aulas 4"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",12},{"idUbicacion",12},{"nombre","Centro de Estudiantes"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",13},{"idUbicacion",13},{"nombre","Biblioteca"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",14},{"idUbicacion",14},{"nombre","Padre Arroyo"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",15},{"idUbicacion",15},{"nombre","Edificio Administrativo"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",16},{"idUbicacion",16},{"nombre","Ciencias de la Salud"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",17},{"idUbicacion",17},{"nombre","Salón Multiusos"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",18},{"idUbicacion",18},{"nombre","Centro de Tecnología y Educación Permanente"}
            });
            InsertQuery("Localizacion", new Dictionary<string, object> {
                {"idLocalizacion",19},{"idUbicacion",19},{"nombre","Kiosko Universitario"}
            });

            #endregion

            #region CoordenadaLocalizacion
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (1,1,19.44023,-70.683369)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (2,1,19.440237,-70.682902)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (3,1,19.43986,-70.682891)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (4,1,19.43986,-70.682891)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (5,2,19.440434,-70.682709)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (6,2,19.440449,-70.682468)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (7,2,19.440081,-70.682452)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (8,2,19.440079,-70.682698)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (9,3,19.440864,-70.683512)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (10,3,19.440873,-70.683209)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (11,3,19.440375,-70.683204)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (12,3,19.440372,-70.683523)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (13,4,19.441171,-70.683106)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (14,4,19.441159,-70.682481)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (15,4,19.440923,-70.682482)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (16,4,19.440923,-70.683114)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (17,5,19.441853,-70.683339)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (18,5,19.441862,-70.682798)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (19,5,19.441703,-70.682787)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (20,5,19.441691,-70.683342)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (21,6,19.442173,-70.683402)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (22,6,19.442181,-70.68284)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (23,6,19.441992,-70.682836)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (24,6,19.441996,-70.683408)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (25,7,19.442315,-70.683633)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (26,7,19.442322,-70.683079)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (27,7,19.442174,-70.683076)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (28,7,19.442179,-70.683628)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (29,8,19.442786,-70.683318)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (30,8,19.44278,-70.682793)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (31,8,19.442657,-70.682794)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (32,8,19.44266,-70.683309)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (33,9,19.442967,-70.681999)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (34,9,19.442963,-70.681488)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (35,9,19.442834,-70.681482)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (36,9,19.442838,-70.682007)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (37,10,19.441628,-70.683687)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (38,10,19.441628,-70.683141)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (39,10,19.441464,-70.683148)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (40,10,19.441465,-70.683334)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (41,10,19.441197,-70.68334)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (42,10,19.441189,-70.683501)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (43,10,19.44147,-70.683513)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (44,10,19.44148,-70.683687)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (45,11,19.443149,-70.683679)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (46,11,19.443151,-70.683147)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (47,11,19.443008,-70.683151)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (48,11,19.443006,-70.683671)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (49,12,19.444131,-70.682992)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (50,12,19.444139,-70.682603)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (51,12,19.443496,-70.682514)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (52,12,19.443547,-70.682928)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (53,13,19.444037,-70.684592)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (54,13,19.444045,-70.684153)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (55,13,19.443407,-70.684158)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (56,13,19.44341,-70.68455)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (57,14,19.442479,-70.684807)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (58,14,19.442499,-70.684536)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (59,14,19.442059,-70.684525)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (60,14,19.442042,-70.684775)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (61,15,19.446231,-70.68363)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (62,15,19.446256,-70.683263)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (63,15,19.445563,-70.683271)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (64,15,19.445563,-70.683614)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (65,16,19.443999,-70.682048)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (66,16,19.444017,-70.681316)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (67,16,19.443382,-70.681299)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (68,16,19.443349,-70.681943)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (69,17,19.44574,-70.681562)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (70,17,19.445753,-70.680594)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (71,17,19.445262,-70.680578)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (72,17,19.445249,-70.681535)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (73,18,19.444108,-70.68552)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (74,18,19.44429,-70.685207)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (75,18,19.444272,-70.68503)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (76,18,19.44403,-70.685174)");
            Query(false, "INSERT INTO CoordenadaLocalizacion VALUES (77,18,19.443969,-70.685542)");
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
		/// Selects the query.
		/// </summary>
		/// <returns>The query.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="columns">Columns.</param>
		/// <param name="constraints">Constraints.</param>
        public IDataReader SelectQuery(string tableName, List<string> columns, Dictionary<string, object> constraints, List<string> order)
        {
			var query = "SELECT ";

			if(columns == null){
				query += "*";
			} else {
				columns.ForEach(x => { query += x + ","; } );
				query += query.Substring(0,query.Length-1);
			}

			query += " FROM main." + tableName;
			if(constraints != null){
				query += " WHERE ";
				foreach(var contraint in constraints){
					query += contraint.Key + " = ";
					if(contraint.Value is String)
						query += "'" + Convert.ToString(contraint.Value) + "' AND ";
					else
						query += Convert.ToString(contraint.Value)+" AND ";
				}
				query = query.Substring(0,query.Length-5);
			}

            if (order != null) 
            {
                query += " ORDER BY ";
                order.ForEach(x => { query += x + ","; } );
                query = query.Substring(0, query.Length - 1);
            }

            Debug.Log(query);

            IDataReader dataReader = null;

            try
            {
                IDbCommand _databaseCommand = _databaseConnection.CreateCommand();
			    _databaseCommand.CommandText = query; // fill the command
			    dataReader = _databaseCommand.ExecuteReader(); // execute command which returns a reader
            }
            catch (SqliteException e)
            {
                Debug.LogException(e);
            }

            return dataReader; // return the reader
		}

		/// <summary>
		/// Deletes the query.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="constraints">Constraints.</param>
		public void DeleteQuery(string tableName, Dictionary<string,object> constraints) 
        {
			var query = "DELETE FROM " + tableName;
			if(constraints != null){
				query += " WHERE ";
				foreach(var contraint in constraints){
					query += contraint.Key + " = ";
					if(contraint.Value is String)
						query += "'" + Convert.ToString(contraint.Value) + "' AND ";
					else
						query += Convert.ToString(contraint.Value)+" AND ";
				}
				query = query.Substring(0,query.Length-5);
			}
			
            query = query.Substring(0,query.Length-5);

            try
            {
                IDbCommand _databaseCommand = _databaseConnection.CreateCommand();
			    _databaseCommand.CommandText = query; 
			    _databaseCommand.ExecuteNonQuery();
            }
            catch (SqliteException e)
            {
                Debug.LogException(e);
            }
		}

		/// <summary>
		/// Inserts the query.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="values">Values.</param>
        public void InsertQuery(string tableName, Dictionary<string, object> values)
        {
            var queryColumns = "INSERT INTO " + tableName + "(";
            var queryValues = " VALUES (";

            foreach (var pair in values)
            {
                queryColumns += pair.Key + ",";
                if (pair.Value is String)
                {
                    queryValues += "'" + Convert.ToString(pair.Value) + "',";
                }
                else
                {
                    queryValues += Convert.ToString(pair.Value) + ",";
                }
            }

            queryColumns = queryColumns.Substring(0, queryColumns.Length - 1) + ")";
            queryValues = queryValues.Substring(0, queryValues.Length - 1) + ")";

            var query = queryColumns + queryValues;

            Debug.Log(query);

            try
            {
                IDbCommand _databaseCommand = _databaseConnection.CreateCommand();
                _databaseCommand.CommandText = query;
                _databaseCommand.ExecuteNonQuery();
            }
            catch (SqliteException e)
            {
                Debug.LogException(e);
            }
        }

		/// <summary>
		/// Updates the query.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="values">Values.</param>
		/// <param name="constraints">Constraints.</param>
		public void UpdateQuery(string tableName, 
		                        Dictionary<string,object> values, 
		                        Dictionary<string,object> constraints)
		{
			var query = "UPDATE " + tableName + " SET ";
			
			foreach(var value in values){
				query += value.Key + " = ";
				if(value.Value is String){
					query += "'" + Convert.ToString(value.Key) + "',";
				} else {
					query += Convert.ToString(value.Key) + ",";
				}
			}
			query = query.Substring(0,query.Length-2) + " WHERE ";
			foreach(var contraint in constraints){
				query += contraint.Key + " = ";
				if(contraint.Value is String)
					query += "'" + Convert.ToString(contraint.Value) + "' AND ";
				else
					query += Convert.ToString(contraint.Value)+" AND ";
			}
			query = query.Substring(0,query.Length-5);

            try
            {
                IDbCommand _databaseCommand = _databaseConnection.CreateCommand();
                _databaseCommand.CommandText = query;
                _databaseCommand.ExecuteNonQuery();
            }
            catch (SqliteException e)
            {
                Debug.LogException(e);
            }
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

		#endregion

		#region Destructor
		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="SncPucmm.Database.SQLiteService"/> is reclaimed by garbage collection.
		/// </summary>
		~ SQLiteService(){
			CloseDataBase();
		}
		#endregion
	}
}