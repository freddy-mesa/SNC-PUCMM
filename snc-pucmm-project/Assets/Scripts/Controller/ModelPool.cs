using System;
using UnityEngine;
using System.Collections.Generic;
using SncPucmm.Model;

namespace SncPucmm.Controller
{
	public class ModelPool
	{
		#region Atributos
		private Dictionary<String,ModelObject> _listModelObject;

		private static ModelPool _modelManager;
		#endregion

		#region Constructor
		public ModelPool ()
		{
			_listModelObject = new Dictionary<String,ModelObject>();
		}
		#endregion

		#region Metodos

		public static ModelPool GetInstance(){
			if(_modelManager == null)
				_modelManager = new ModelPool();
			return _modelManager;
		}

		/// <summary>
		/// Add the specified key and value.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public void Add(string key, ModelObject value)
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
		public ModelObject GetValue(string key){
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

