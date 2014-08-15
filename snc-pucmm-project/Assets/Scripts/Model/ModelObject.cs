using System;

namespace SncPucmm.Model
{
	public class ModelObject
	{
		string _menu;

		public String Menu { get{return _menu;}}

		public ModelObject (string menuName)
		{
			_menu = menuName;
		}
	}
}

