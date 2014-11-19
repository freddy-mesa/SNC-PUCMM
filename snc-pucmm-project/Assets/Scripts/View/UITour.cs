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
                var tourController = ModelPoolManager.GetInstance().GetValue("tourCtrl") as TourController;

                bool isEndTour = tourController.UpdateSectionTour();
                if (isEndTour)
                {
                    //Ha llegado a ultimo punto de reunion
                    //var tourNotification = UIUtils.FindGUI("MenuNavigation/TourNotification");
                    //tourNotification.SetActive(true);
                    //tourNotification.GetComponent<UILabel>().text = "Ha finalizado el Tour";
                }
            }
        }        

        #endregion
    }
}
