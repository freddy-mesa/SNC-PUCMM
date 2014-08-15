using System;

namespace SncPucmm.Model
{
	public class Building : ModelObject
	{
		#region Atributos
		string _name;
		string _abbreviaton;
		int _id;

		#endregion

		#region Propiedades
		public int Id {
			get { return _id; }
		}
		public string Abbreviation {
			get { return _abbreviaton; }
		}

		public string Name {
			get { return _name; }
		}

		#endregion

		#region Contructores
		public Building (int id, string name, string abbreviaton, string menuName) : this(menuName)
		{
			_id = id;
			_name = name;
			_abbreviaton = abbreviaton;
		}

		public Building (string menuName) : base(menuName){}

		#endregion
	}
}

