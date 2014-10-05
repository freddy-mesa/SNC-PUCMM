using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace SncPucmm.View
{
    class UIBuilding : MonoBehaviour
    {
        private TextMesh label;

        void Start()
        {
            label = this.transform.FindChild("Text").GetComponent<TextMesh>();
        }

        void OnBecameVisible()
        {
            this.enabled = true;
        }

        void OnBecameInvisible()
        {
            this.enabled = false;
        }

        void Update()
        {
            var cameraRotation = Camera.main.transform.eulerAngles;

            if (UICamara.Vista_3era_Persona)
            {
                var cameraPosition = Camera.main.transform.position;

                label.transform.localPosition = new Vector3(0.35f, 7f, 11f);

                float distance = UIUtils.getDirectDistance(
                    label.transform.position.x,
                    label.transform.position.z,
                    cameraPosition.x,
                    cameraPosition.z
                );

                if (distance > 120)
                {
                    label.transform.eulerAngles = new Vector3(0, cameraRotation.y, 0);
                }
                else
                {
                    label.transform.eulerAngles = new Vector3(Mathf.Clamp(120 - distance, 0 , 90), cameraRotation.y, 0);
                }
            }
        }
    }
}
