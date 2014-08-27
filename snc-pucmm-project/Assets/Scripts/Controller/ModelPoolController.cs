using System;
using UnityEngine;
using System.Collections.Generic;
using SncPucmm.Model;

namespace SncPucmm.Controller
{
	public class ModelPoolController
	{
		#region Atributos
        /// <summary>
        /// Dictionario de Objectos del Modelo
        /// </summary>
		private Dictionary<String,object> _listModelObject;

        /// <summary>
        /// Instancia del Controlador del Pool del Modelo
        /// </summary>
		private static ModelPoolController _modelManager;

		#endregion

		#region Constructor
        
        public ModelPoolController()
		{
			_listModelObject = new Dictionary<String,object>();
		}
		#endregion

		#region Metodos
        
        /// <summary>
        /// Patrón Singlenton
        /// </summary>
        /// <returns></returns>
        public static ModelPoolController GetInstance()
        {
			if(_modelManager == null)
                _modelManager = new ModelPoolController();
			return _modelManager;
		}

		/// <summary>
		/// Add the specified key and value.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public void Add(string key, object value)
		{
			_listModelObject.Add(key, value);
		}

		/// <summary>
		/// Contains the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		public bool Contains(string key)
		{
			if(_listModelObject.ContainsKey(key))
				return true;

			return false;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="key">Key.</param>
		public object GetValue(string key){
			return _listModelObject[key];
		}

		/// <summary>
		/// Remove the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		public void Remove(string key)
		{
			_listModelObject.Remove(key);
		}
		
		#endregion
	}
}