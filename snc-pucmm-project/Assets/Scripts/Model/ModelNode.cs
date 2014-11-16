using System;

namespace SncPucmm.Model
{
    public class ModelNode : IEquatable<ModelNode>
    {
        #region Atributos

        public int idNodo;
        public string name;
        public int idUbicacion;
        public bool isBuilding;
        public string abreviacion;
        public int cantidadPlantas;
        public int planta;

        #endregion 

        #region Metodos

        public bool Equals(ModelNode other)
        {
            return this.idNodo == other.idNodo;
        }

        #endregion
    }
}
