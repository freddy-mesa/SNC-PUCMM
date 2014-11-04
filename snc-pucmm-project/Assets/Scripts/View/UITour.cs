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
                var tourCotroller = ModelPoolManager.GetInstance().GetValue("tourCtrl") as TourController;

                if (!tourCotroller.UpdateSectionTour())
                {
                    //Ha llegado a ultimo punto de reunion
                    
                    //Mensaje

                    //
                }
            }
        }        

        #endregion
    }
}
