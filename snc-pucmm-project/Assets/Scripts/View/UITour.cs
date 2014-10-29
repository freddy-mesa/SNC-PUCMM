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

        public IEnumerator CreateTourRoleCreator(Tour tour, List<ModelNode> modelNodeList)
        {
            yield return StartCoroutine(TourController.GetInstance().CreateTourRoleCreator(tour, modelNodeList));
        }

        #endregion

    }
}
