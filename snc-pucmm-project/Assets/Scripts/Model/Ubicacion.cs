using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model
{
    public class Ubicacion
    {
        #region Atributos

        string _nombre;
        int _idUbicacion;
        private string _abreviacion;

        #endregion

        #region Propiedades

        public String Nombre { get { return _nombre; } }

        public Int32 IdUbicacion { get { return _idUbicacion; } }

        public String Abreviacion { get { return _abreviacion; } }

        #endregion

        #region Constructores

        public Ubicacion()
        {

        }

        public Ubicacion(int idUbicacion, string name, string abreviacion)
        {
            this._nombre = name;
            this._idUbicacion = idUbicacion;
            this._abreviacion = abreviacion;
        }

        #endregion

        #region Metodos

        #endregion
    }
}