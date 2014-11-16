using SncPucmm.Controller;
using SncPucmm.Controller.Control;
using SncPucmm.Model;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.View
{
    class UIInsideBuilding : UITouch
    {
        #region Metodos

        public void OnTouchBuildingInsideLocation(string buttonName)
        {
            var menu = MenuManager.GetInstance().GetCurrentMenu() as IButton;
            menu.GetButtonList().ForEach(x =>
            {
                if (x.Name.Equals(buttonName))
                {
                    x.OnTouch(null);
                }
            });
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, transform.lossyScale);
        } 

        #endregion
    }
}
