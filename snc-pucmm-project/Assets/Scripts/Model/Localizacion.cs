using System;

namespace SncPucmm.Model
{
    public class Localizacion
    {
        #region Atributos

        string _nombre;
        int _idUbicacion;
        int _idLocalizacion;

        #endregion

        #region Propiedades

        public int IdLocalizacion { get { return _idLocalizacion; } }
        public int IdUbicacion { get { return _idUbicacion; } }
        public string Nombre { get { return _nombre; } }

        #endregion

        #region Contructores

        public Localizacion(int idLocalizacion, int idUbicacion, string nombre)
        {
            _idLocalizacion = idLocalizacion;
            _idUbicacion = idUbicacion;
            _nombre = nombre;
        }

        #endregion
    }
}