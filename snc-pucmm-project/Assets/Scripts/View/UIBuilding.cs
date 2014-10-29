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
        private TextMesh numberLabel;
        private Transform prefabNumber;
        private Transform prefabSelection;
        private bool isModeTourCreationActive;

        void Start()
        {
            nameLabel = this.transform.FindChild("Text").GetComponent<TextMesh>();
            nameLabel.gameObject.SetActive(true);

            prefabNumber = this.transform.FindChild("TourSelection");
            prefabNumber.gameObject.SetActive(false);

            numberLabel = prefabNumber.FindChild("Number").gameObject.GetComponent<TextMesh>();

            prefabSelection = this.transform.FindChild("SelectionBuilding");
            prefabSelection.gameObject.SetActive(false);

            isModeTourCreationActive = false;
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
                var modelObject = this.GetComponent<ModelObject>();
                Transform objToMove = null;

                if (State.GetCurrentState().Equals(eState.TourCreation))
                {
                    if (!isModeTourCreationActive)
                    {
                        isModeTourCreationActive = true;
                    }

                    if (modelObject.isSeleted)
                    {
                        prefabSelection.gameObject.SetActive(true);
                    }
                    else
                    {
                        prefabSelection.gameObject.SetActive(false);
                    }

                    var modelNodeList = (MenuManager.GetInstance().GetCurrentMenu() as GUIMenuTourCreation).GetModelNodeList();

                    if (modelObject.TourActive && modelNodeList.Count != 0)
                    {
                        objToMove = prefabNumber;
                        nameLabel.gameObject.SetActive(false);

                        for (int i = 0; i < modelNodeList.Count; ++i)
                        {
                            if (modelNodeList[i].idNodo == modelObject.Id)
                            {
                                numberLabel.text = (i + 1).ToString();
                                break;
                            }
                        }

                    }
                    else
                    {
                        objToMove = nameLabel.transform;

                        modelObject.TourActive = false;
                        numberLabel.text = "";
                        prefabNumber.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (isModeTourCreationActive)
                    {
                        modelObject.TourActive = false;
                        numberLabel.text = "";
                        prefabNumber.gameObject.SetActive(false);

                        isModeTourCreationActive = false;
                    }

                    objToMove = nameLabel.transform;
                }

                objToMove.gameObject.SetActive(true);

                float distance = UIUtils.getDirectDistance(
                    objToMove.position.x,
                    objToMove.position.z,
                    cameraPosition.x,
                    cameraPosition.z
                );

                if (distance > 120)
                {
                    nameLabel.transform.eulerAngles = new Vector3(0, cameraRotation.y, 0);
                }
                else
                {
                    nameLabel.transform.eulerAngles = new Vector3(Mathf.Clamp(120 - distance, 15, 90), cameraRotation.y, 0);
                }                
            }
        }
    }
}
