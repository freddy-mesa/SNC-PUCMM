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
        private bool _selectedForTour;

        public int Id { get { return _id; } set { _id = value; } }

        public bool TourActive { get { return _selectedForTour; } set { _selectedForTour = value; } }

        public bool isSeleted { get; set; }

        public object ObjectTag { get; set; }
    }
}