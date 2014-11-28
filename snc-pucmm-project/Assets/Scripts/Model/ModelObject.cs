using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Model
{
    class ModelObject : MonoBehaviour
    {
        [SerializeField]
        private int _id;

        [SerializeField]
        private bool _containsInsideNodes;

        public int Id { get { return _id; } set { _id = value; } }

        public bool ContainsInsideNodes { get { return _containsInsideNodes; } set { _containsInsideNodes = value; } }

        public object ObjectTag { get; set; }
    }
}