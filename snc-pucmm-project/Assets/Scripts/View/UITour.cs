using SncPucmm.Controller;
using SncPucmm.Controller.GUI;
using SncPucmm.Controller.Tours;
using SncPucmm.Model;
using SncPucmm.Model.Domain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SncPucmm.View
{
    class UITour : UITouch
    {
        #region Metodos

        new void Update()
        {
            base.Update();

            if (State.GetCurrentState().Equals(eState.MenuNavigation) && ModelPoolManager.GetInstance().Contains("tourCtrl"))
            {
                var tourCtrl = ModelPoolManager.GetInstance().GetValue("tourCtrl") as TourController;

                if (!tourCtrl.isTourActive)
                {
                    string nodeName;
                    if (tourCtrl.IsUserCollidingBuilding(out nodeName) && nodeName == tourCtrl.SectionTourDataList[tourCtrl.CurrentSectionIndex].Desde)
                    {
                        tourCtrl.isTourActive = true;
                    }
                }

                bool isEndTour;
                //If true, One section of tour (from puntoReunion to other) is completed
                if (tourCtrl.isTourActive && tourCtrl.UpdateSectionTour(out isEndTour))
                {
                    //Ha Completado una seccion del tour
                    var tourNotification = UIUtils.FindGUI("MenuNavigation/NotificationSeccionTourCompletada");
                    tourNotification.SetActive(true);

                    if (isEndTour)
                    {
                        tourNotification.transform.FindChild("Info").GetComponent<UILabel>().text = "Ha finalizado el Tour";
                        tourCtrl.isEndTour = true;
                    }
                    else
                    {
                        string desde = tourCtrl.SectionTourDataList[tourCtrl.CurrentSectionIndex].Desde;
                        string hasta = tourCtrl.SectionTourDataList[tourCtrl.CurrentSectionIndex].Hasta;

                        tourNotification.transform.FindChild("Info").GetComponent<UILabel>().text = string.Format("Ha recorrido el tour desde {0} hasta {1}", desde, hasta);
                    }
                }
            }
        }        

        #endregion
    }
}
