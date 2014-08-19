using System;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

namespace SncPucmm.Database
{
	public class SQLiteService
	{
		#region Atributos
		private static SQLiteService _dataBaseService;
		private SqliteConnection _databaseConnection;
		private SqliteCommand _databaseCommand;

		private const string  databaseName = "sncpucmm.s3db";

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
				connectionString = "URI=file:" + databaseName;
			}
			else if(Application.platform == RuntimePlatform.Android)
			{
				var filePath = Application.persistentDataPath + "/" + databaseName;				

				if(!File.Exists(filePath))
				{ 	
					// this is the path to your StreamingAssets in android
					WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + databaseName);  
					
					while(!loadDB.isDone);  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
					
					// then save to Application.persistentDataPath
					File.WriteAllBytes(filePath, loadDB.bytes);
					
				} else {
					createDataBase = true;
				}
				connectionString = filePath;
			}

			_databaseConnection = new SqliteConnection(connectionString);
			_databaseConnection.Open();

			if(createDataBase){
				//InitializeDataBase();
			}
		}

		private void InitializeDataBase()
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

		/// <summary>
		/// Closes the data base.
		/// </summary>
		private void CloseDataBase() {
			_databaseCommand.Dispose(); 
			_databaseCommand = null; 
			_databaseConnection.Close(); 
			_databaseConnection = null; 
		}

		/// <summary>
		/// Selects the query.
		/// </summary>
		/// <returns>The query.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="columns">Columns.</param>
		/// <param name="constraints">Constraints.</param>
		public SqliteDataReader SelectQuery(string tableName, List<string> columns, Dictionary<string,object> constraints) {
			var query = "SELECT ";

			if(columns == null){
				query += "*";
			} else {
				columns.ForEach(x => { query += x + ","; } );
				query += query.Substring(0,query.Length-1);
			}

			query += " FROM " + tableName;
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

			_databaseCommand = _databaseConnection.CreateCommand();
			_databaseCommand.CommandText = query; // fill the command
			var dataReader = _databaseCommand.ExecuteReader(); // execute command which returns a reader
			return dataReader; // return the reader
		}

		/// <summary>
		/// Deletes the query.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="constraints">Constraints.</param>
		public void DeleteQuery(string tableName, Dictionary<string,object> constraints) {
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

			_databaseCommand = _databaseConnection.CreateCommand();
			_databaseCommand.CommandText = query; 
			_databaseCommand.ExecuteNonQuery();
		}

		/// <summary>
		/// Inserts the query.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="values">Values.</param>
		public void InsertQuery(string tableName, Dictionary<string,object> values) {
			var queryColumns = "INSERT INTO (";
			var queryValues = " VALUES (";
			
			foreach(var pair in values){
				queryColumns += pair.Key + ",";
				if(queryValues is String){
					queryValues += "'" + Convert.ToString(pair.Value) + "',";
				} else {
					queryValues += Convert.ToString(pair.Value) + ",";
				}
			}
			
			queryColumns = queryColumns.Substring(0,queryColumns.Length-2) + ")";
			queryValues = queryValues.Substring(0,queryValues.Length-2) + ")";
			
			var query = queryColumns + queryValues;
			
			_databaseCommand = _databaseConnection.CreateCommand();
			_databaseCommand.CommandText = query; 
			_databaseCommand.ExecuteNonQuery();
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
			
			_databaseCommand = _databaseConnection.CreateCommand();
			_databaseCommand.CommandText = query; 
			_databaseCommand.ExecuteNonQuery();
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
			var query = "CREATE TABLE " + tableName + " ( \n";
			foreach (var pair in columns)
			{
				query += String.Format("{0} {1},\n", pair.Key, pair.Value);
			}
			
			foreach (var pair in primaryColumnsConstraint)
			{
				query += String.Format("CONSTRAINT {0} PRIMARY KEY (", pair.Key);
				foreach (var column in pair.Value)
				{
					query += column + ",";
				}
				query = query.Substring(0, query.Length - 1) + "),\n";
			}
			
			foreach (var pair in foreignColumnsConstraint)
			{
				query += String.Format("CONSTRAINT {0} FOREIGN KEY ({1}) REFERENCES {2}({3}),\n", pair.Key, pair.Value[0], pair.Value[1], pair.Value[3]);
			}
			query = query.Substring(0, query.Length - 2) + "\n)";

			_databaseCommand = _databaseConnection.CreateCommand();
			_databaseCommand.CommandText = query; 
			_databaseCommand.ExecuteNonQuery();
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