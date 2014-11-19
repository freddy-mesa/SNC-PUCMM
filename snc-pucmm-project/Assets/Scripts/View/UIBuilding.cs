using SncPucmm.Controller;
using SncPucmm.Controller.GUI;
using SncPucmm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.View
{
    class UIBuilding : MonoBehaviour
    {
        private TextMesh nameLabel;

        void Start()
        {
            nameLabel = this.transform.FindChild("Text").GetComponent<TextMesh>();
            nameLabel.gameObject.SetActive(true);
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
            if (UICamaraControl.Vista_3era_Persona)
            {
                var cameraPosition = Camera.main.transform.position;
                var cameraRotation = Camera.main.transform.eulerAngles;

                float distance = UIUtils.getDirectDistance(
                    nameLabel.transform.position.x,
                    nameLabel.transform.position.z,
                    cameraPosition.x,
                    cameraPosition.z
                );

                if (distance > 100)
                {
                    nameLabel.transform.eulerAngles = new Vector3(0, cameraRotation.y, 0);
                }
                else
                {
                    nameLabel.transform.eulerAngles = new Vector3(Mathf.Clamp(50 - distance, 10, 40), cameraRotation.y, 0);
                }                
            }
        }

        void OnTriggerEnter(Collider objectCollider)
        {
            if (UICamaraControl.Vista_3era_Persona)
            {
                Camera.main.transform.position = UICamaraControl.lastPosition;
            }
        }
    }
}
