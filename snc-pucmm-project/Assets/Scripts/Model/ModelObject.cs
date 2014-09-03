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

        public int Id { get { return _id; } set { _id = value; } }

        public object ObjectTag { get; set; }
    }
}
