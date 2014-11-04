using SncPucmm.Controller;
using SncPucmm.Controller.Control;
using SncPucmm.Controller.Navigation;
using SncPucmm.Database;
using SncPucmm.Model.Domain;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Controller.GUI
{
    class MenuUsuarioTourSelection : IMenu, IButton
    {
        #region Atributos

        string name;
        string tourName;

        List<Button> buttonList;
        UsuarioTour usuarioTour;
        List<DetalleUsuarioTour> detalleUsuarioTourList;
        Transform scrollViewItemTemplate;
        
        #endregion

        #region Constructores

        public MenuUsuarioTourSelection(string name, string tourName, UsuarioTour usuarioTour, List<DetalleUsuarioTour> detalleUsuarioTourList)
        {
            this.name = name;
            this.usuarioTour = usuarioTour;
            this.detalleUsuarioTourList = detalleUsuarioTourList;
            this.tourName = tourName;

            Initializer();
        }
        
        #endregion

        #region Metodos

        private void Initializer()
        {
            scrollViewItemTemplate = Resources.Load("GUI/DetalleUsuarioTourItem") as Transform;

            buttonList = new List<Button>();

            Button btnExit = new Button("ButtonExit");
            btnExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
            buttonList.Add(btnExit);

            Button btnResume = new Button("ButtonResumen");
            btnResume.OnTouchEvent += new OnTouchEventHandler(OnTouchResumeButton);
            buttonList.Add(btnResume);

            Button btnReset = new Button("ButtonReset");
            btnReset.OnTouchEvent += new OnTouchEventHandler(OnTouchResetButton);
            buttonList.Add(btnReset);

            CreateScrollView();
        }

        private void OnTouchResetButton(object sender, TouchEventArgs e)
        {
            //Poner las fechas de la lista de los detalleUsuarioTourList en blanco            
            foreach (var detalleUsuarioTour in detalleUsuarioTourList)
            {
                //En la base de datos
                SQLiteService.GetInstance().Query(false,
                    "UPDATE DetalleUsuarioTour SET fechaInicio = '', fechaLlegada = '', fechaFin = '' "+
                    "WHERE id = " + detalleUsuarioTour.idDetalleUsuarioTour.Value
                );

                //En la lista
                detalleUsuarioTour.fechaFin = null;
                detalleUsuarioTour.fechaInicio = null;
                detalleUsuarioTour.fechaLlegada = null;
            }

            //Quitar los hijos del ScrollView
            UIUtils.DestroyChilds("MenuUsuarioTourSelection/ScrollView", true);

            //Poner los hijos actualizados
            CreateScrollView();
        }

        private void OnTouchResumeButton(object sender, TouchEventArgs e)
        {
            NavigationController controller = ModelPoolManager.GetInstance().GetValue("navigationCtrl") as NavigationController;
            controller.StartTourNavigation(tourName, detalleUsuarioTourList);
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
        }

        private void CreateScrollView()
        {
            for (int i = 0; i < detalleUsuarioTourList.Count; i++)
            {
                //Creando el item del Tree View con world coordinates
                var item = GameObject.Instantiate(scrollViewItemTemplate) as Transform;

                item.name = "DetalleUsuarioTourItem" + i;

                //Agregando relacion de padre (Tree View List) - hijo (item del Tree View List)
                item.parent = UIUtils.FindGUI("MenuUsuarioTourSelection/ScrollView").transform;

                //Agregando la posicion relativa del hijo con relacion al padre
                item.transform.localPosition = new Vector3(
                    scrollViewItemTemplate.localPosition.x,
                    scrollViewItemTemplate.localPosition.y - 65f * i,
                    scrollViewItemTemplate.localPosition.z
                );

                //Agregando la escala relativa del hijo con relacion al padre
                item.localScale = scrollViewItemTemplate.localScale;

                //Encontrando texto del un item (su hijo)
                var itemText = item.FindChild("Label").GetComponent<UILabel>();

                var result = SQLiteService.GetInstance().Query(true,
                    "SELECT NOD.nombre FROM PuntoReunionTour PUN, Nodo NOD " +
                    "WHERE PUN.id = "+ detalleUsuarioTourList[i].idPuntoReunionTour +" AND PUN.idNodo = NOD.idNodo"
                );

                if(result.Read())
                {
                    itemText.text = Convert.ToString(result["nombre"]);
                }

                if (!detalleUsuarioTourList[i].fechaLlegada.HasValue)
                {
                    item.FindChild("checkImg").gameObject.SetActive(true);
                }
            }
        }

        #region Metodos Implementados

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

        ~MenuUsuarioTourSelection()
        {
            this.name = null;
            this.usuarioTour = null;
            this.buttonList = null;
            this.detalleUsuarioTourList = null;
            this.scrollViewItemTemplate = null;
        }

        #endregion

    }
}
