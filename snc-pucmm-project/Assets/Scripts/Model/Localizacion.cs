using System;

namespace SncPucmm.Model
{
	public class Localizacion
	{
		#region Atributos
		
        string _name;
		int _idUbicacion;
		int _idLocalizacion;

		#endregion

		#region Propiedades
		
        public int IdLocalizacion { get { return _idLocalizacion; } }
		public int IdUbicacion { get { return _idUbicacion; } }
		public string Name { get { return _name; } }

		#endregion

		#region Contructores
        
        public Localizacion(int idLocalizacion, int idUbicacion, string name)
		{
            _idLocalizacion = idLocalizacion;
            _idUbicacion = idUbicacion;
            _name = name;
		}

		#endregion
	}
}

