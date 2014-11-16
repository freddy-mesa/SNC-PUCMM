using SncPucmm.Controller.Control;
using SncPucmm.Controller.Navigation;
using SncPucmm.Database;
using SncPucmm.Model;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller.GUI
{
    class MenuInsideBuilding : IMenu, IButton
    {
        #region Atributos

        ModelNode modelNode;
        int currentFloor;

        string name;
        List<Button> buttonList;

        bool isPreviousButtonActive;
        bool isNextButtonActive;

        #endregion

        #region Constructor

        public MenuInsideBuilding(string name, ModelNode modelNode)
        {
            this.name = name;
            this.modelNode = modelNode;
            currentFloor = 1;

            Initializer();
        }
        
        #endregion

        #region Metodos

        public void Initializer()
        {
            buttonList = new List<Button>();

            Button buttonPreviousFloor = new Button("ButtonPreviousFloor");
            buttonPreviousFloor.OnTouchEvent += new OnTouchEventHandler(OnTouchPreviousFloorButton);
            buttonList.Add(buttonPreviousFloor);

            Button buttonNextFloor = new Button("ButtonNextFloor");
            buttonNextFloor.OnTouchEvent += new OnTouchEventHandler(OnTouchNextFloorButton);
            buttonList.Add(buttonNextFloor);

            Button buttonExit = new Button("ButtonExit");
            buttonExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
            buttonList.Add(buttonExit);

            UIUtils.FindGUI("MenuInsideBuilding/" + buttonList[0].Name).SetActive(false);
            UIUtils.FindGUI("MenuInsideBuilding/" + buttonList[1].Name).SetActive(true);
            UIUtils.FindGUI("MenuInsideBuilding/Label").GetComponent<UILabel>().text = modelNode.name;

            FillButtonListWithLocations();

            ShowInsidePlaneBuilding(modelNode, "Planta" + currentFloor);
        }

        private void FillButtonListWithLocations()
        {
            var sql = "SELECT idNodo, nombre FROM Nodo WHERE edificio is null AND idUbicacion = " + modelNode.idUbicacion;
            using (var service = new SQLiteService())
            {
                using (var result = service.SelectQuery(sql))
                {
                    while (result.Read())
                    {
                        var idNodo = Convert.ToInt32(result["idNodo"]);
                        var nombre = Convert.ToString(result["nombre"]);
                        //var planta = Convert.ToInt32(result["planta"]);

                        Button button = new Button(nombre);
                        button.OnTouchEvent += new OnTouchEventHandler(OnTouchLocationInsideBuilding);
                        button.ObjectTag = new ModelNode { idNodo = idNodo, name = nombre };
                        buttonList.Add(button);
                    }
                }
            }
        }

        private void OnTouchLocationInsideBuilding(object sender, TouchEventArgs e)
        {
            NavigationController controller = ModelPoolManager.GetInstance().GetValue("navigationCtrl") as NavigationController;

            var modelNode = (sender as Button).ObjectTag as ModelNode;

            Debug.Log(modelNode.name);

            //controller.StartNavigation(modelNode.name);            
            //controller.StartNavigation(modelNode.idNodo);
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
            State.ChangeState(eState.MenuBuilding);

            ShowEntireBuilding(modelNode);

            //enable el box collider del edificio
            UIUtils.Find("/PUCMM/Model3D/" + modelNode.abreviacion).GetComponent<BoxCollider>().enabled = true;
        }

        private void OnTouchPreviousFloorButton(object sender, TouchEventArgs e)
        {
            currentFloor--;

            if (currentFloor <= 1)
            {
                currentFloor = 1;
                UIUtils.FindGUI("MenuInsideBuilding/" + buttonList[0].Name).SetActive(false);
                isPreviousButtonActive = false;
            }

            if (!isNextButtonActive)
            {
                UIUtils.FindGUI("MenuInsideBuilding/" + buttonList[1].Name).SetActive(true);
                isNextButtonActive = true;
            }

            ShowInsidePlaneBuilding(modelNode, "Planta" + currentFloor);
        }

        private void OnTouchNextFloorButton(object sender, TouchEventArgs e)
        {
            currentFloor++;

            if (currentFloor >= modelNode.cantidadPlantas)
            {
                currentFloor = modelNode.cantidadPlantas;
                UIUtils.FindGUI("MenuInsideBuilding/" + buttonList[1].Name).SetActive(false);
                isNextButtonActive = false;
            }

            if (!isPreviousButtonActive)
            {
                UIUtils.FindGUI("MenuInsideBuilding/" + buttonList[0].Name).SetActive(true);
                isPreviousButtonActive = true;
            }

            ShowInsidePlaneBuilding(modelNode, "Planta" + currentFloor);
        }

        private void ShowInsidePlaneBuilding(ModelNode modelNode, string namePlaneFloor)
        {
            //Haciendo un reset del edificio
            var edificio = ShowEntireBuilding(modelNode);

            edificio.FindChild("Otros").gameObject.SetActive(false);
            edificio.FindChild("Columnas").gameObject.SetActive(false);

            var planos = edificio.transform.FindChild("Planos");

            foreach (Transform plano in planos)
            {
                if (plano.name == namePlaneFloor)
                {
                    plano.gameObject.SetActive(true);
                }
            }

            var plantas = edificio.FindChild("Plantas").transform;
            if (namePlaneFloor == "Planta1")
            {
                plantas.FindChild("Planta1").gameObject.SetActive(false);
                plantas.FindChild("Planta2").gameObject.SetActive(false);
                plantas.FindChild("Planta3").gameObject.SetActive(false);
            }
            else if (namePlaneFloor == "Planta2")
            {
                plantas.FindChild("Planta2").gameObject.SetActive(false);
                plantas.FindChild("Planta3").gameObject.SetActive(false);
            }
            else
            {
                plantas.FindChild("Planta3").gameObject.SetActive(false);
            }
        }

        private Transform ShowEntireBuilding(ModelNode modelNode)
        {
            var edificio = UIUtils.Find("/PUCMM/Model3D/" + modelNode.abreviacion);

            var plantas = edificio.transform.FindChild("Plantas");
            foreach (Transform planta in plantas) { planta.gameObject.SetActive(true); }

            var planos = edificio.transform.FindChild("Planos");
            foreach (Transform plano in planos) { plano.gameObject.SetActive(false); }

            edificio.transform.FindChild("Otros").gameObject.SetActive(true);
            edificio.transform.FindChild("Columnas").gameObject.SetActive(true);

            return edificio.transform;
        }

        #region Implementados

        public string GetMenuName()
        {
            return name;
        }

        public List<Button> GetButtonList()
        {
            return buttonList;
        }

        #endregion

        #endregion

        #region Destructor

        ~MenuInsideBuilding()
        {
            this.name = null;
            this.buttonList = null;
            this.modelNode = null;
        }

        #endregion

    }
}
