using System;
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
			// we set the connection to our database
			string connectionString = "URI=file:sncpucmm.s3db";
			_databaseConnection = new SqliteConnection(connectionString);
			_databaseConnection.Open();
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
		public void UpdateQuery(string tableName, Dictionary<string,object> values, Dictionary<string,object> constraints){ // Selects a single Item
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

		#endregion
	}
}

