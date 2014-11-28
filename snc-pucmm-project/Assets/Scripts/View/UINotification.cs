using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.View
{
    class UINotification : MonoBehaviour
    {
        public static bool StartNotificationRecalcularRuta;

        void Start()
        {
            StartNotificationRecalcularRuta = false;
        }

        void Update()
        {
            if (StartNotificationRecalcularRuta)
            {
                StartNotificationRecalcularRuta = false;
                UIUtils.FindGUI("MenuNavigation/NotificationRecalcularRuta").SetActive(true);
                StartCoroutine(ShowNotificationRecalcularRuta());
            }
        }

        IEnumerator ShowNotificationRecalcularRuta()
        {
            float t = 0.0f, transitionDuration = 1.5f;
            var slider = UIUtils.FindGUI("MenuNavigation/NotificationRecalcularRuta/ProcessBar").GetComponent<UISlider>();
            while (t < 1.0f)
            {
                t += Time.deltaTime * (Time.timeScale / transitionDuration);
                slider.value = t;
                yield return 0;
            }

            if (t > 1.0f)
            {
                UIUtils.FindGUI("MenuNavigation/NotificationRecalcularRuta").SetActive(false);
            }
        }
    }
}
