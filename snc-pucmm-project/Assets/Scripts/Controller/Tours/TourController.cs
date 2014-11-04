using SncPucmm.Model;
using SncPucmm.Model.Domain;
using SncPucmm.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller.Tours
{
    class TourController
    {
        #region Atributos

        List<SectionTourData> sectionTourDataList;
        SectionTourData currentSectionTourData;
        int currentIndex = 0;

        bool isEndingSectionTour;
        bool isStartingSectionTour;
        private List<DetalleUsuarioTour> detalleUsuarioTourList;

        #endregion

        #region Propiedades

        public List<DetalleUsuarioTour> DetalleUsuarioTourList { get { return detalleUsuarioTourList; } }

        #endregion

        #region Constructor

        public TourController()
        {
            sectionTourDataList = new List<SectionTourData>();

            ResetValues();
        }

        public TourController(List<DetalleUsuarioTour> detalleUsuarioTourList)
        {
            this.detalleUsuarioTourList = detalleUsuarioTourList;
        }

        #endregion

        #region Metodos

        public void ResetValues()
        {
            isEndingSectionTour = false;
            isStartingSectionTour = false;
        }

        public void SetStartSectionTour(int index)
        {
            currentIndex = index;
            currentSectionTourData = sectionTourDataList[currentIndex];
        }

        public bool UpdateSectionTour()
        {
            bool isTourEnd = false;
            string nodeName;
            if (IsUserCollidingBuilding(out nodeName))
            {
                if (!isStartingSectionTour && currentSectionTourData.Desde == nodeName)
                {
                    isStartingSectionTour = true;

                    var detalleUsuarioTour = this.detalleUsuarioTourList.Find(detalle =>
                    {
                        return detalle.idPuntoReunionTour == currentSectionTourData.IdPuntoReuionNodoDesde;
                    });

                    detalleUsuarioTour.fechaInicio = DateTime.Now;
                }
                else if (!isEndingSectionTour && currentSectionTourData.Hasta == nodeName)
                {
                    isEndingSectionTour = true;
                    var detalleUsuarioTour = this.detalleUsuarioTourList.Find(detalle =>
                    {
                        return detalle.idPuntoReunionTour == currentSectionTourData.IdPuntoReuionNodoDesde;
                    });

                    detalleUsuarioTour.fechaFin = DateTime.Now;
                }
            }

            if (isEndingSectionTour && isStartingSectionTour)
            {
                ResetValues();
                if (!CanChangeToNextSectionTour())
                {
                    isTourEnd = true;
                }
            }

            return isTourEnd;
        }

        private bool IsUserCollidingBuilding(out string buildingName)
        {
            if (UICharacter.isCollidingWithBuilding)
            {
                buildingName = UICharacter.BuildingColliding;
                return true;
            }
            else
            {
                buildingName = string.Empty;
                return false;
            }
        }

        private bool CanChangeToNextSectionTour()
        {
            currentIndex++;
            if (currentIndex < sectionTourDataList.Count)
            {
                currentSectionTourData = sectionTourDataList[currentIndex];
                return true;
            }

            return false;
        }

        public void AddSectionTour(SectionTourData data)
        {
            sectionTourDataList.Add(data);
        }
        
        #endregion
    }

    class SectionTourData
    {
        public string Desde { get; set; }
        public string Hasta { get; set; }
        public int IdPuntoReuionNodoDesde { get; set; }
        public int IdPuntoReuionNodoHasta { get; set; }

        public override bool Equals(object obj)
        {
            SectionTourData other = obj as SectionTourData;
            if (this.Desde == other.Desde && this.Hasta == other.Hasta)
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}