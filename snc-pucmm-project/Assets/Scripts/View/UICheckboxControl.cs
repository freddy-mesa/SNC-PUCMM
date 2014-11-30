using SncPucmm.Controller;
using SncPucmm.Controller.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.View
{
    class UICheckboxControl : UITouch
    {
        void OnClick()
        {
            isButtonTapped = true;

            var checkBoxList = (MenuManager.GetInstance().GetCurrentMenu() as ICheckBox).GetCheckBoxList();
            foreach (var checkBox in checkBoxList)
            {
                if (checkBox.Name == this.name)
                {
                    checkBox.OnChange(null);
                }
            }
        }
    }
}
