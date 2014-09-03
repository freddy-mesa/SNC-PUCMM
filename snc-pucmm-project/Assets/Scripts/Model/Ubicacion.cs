using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SncPucmm.Model
{
    public class Ubicacion
    {
        #region Atributos
        
        #endregion

        #region Propiedades

        public String Name { get; set; }

        public String AbbreviationName { get; set; }

        public Int32 IdUbicacion { get; set; }

        #endregion

        #region Constructores

        public Ubicacion()
        {

        }

        public Ubicacion(int idUbicacion, string name, string abbreviationName)
        {
            this.Name = name;
            this.IdUbicacion = idUbicacion;
            this.AbbreviationName = abbreviationName;
        }

        #endregion

        #region Metodos

        #endregion
    }
}
