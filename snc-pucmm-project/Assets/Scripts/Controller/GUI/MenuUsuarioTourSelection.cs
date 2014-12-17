using SncPucmm.Controller;
using SncPucmm.Controller.Control;
using SncPucmm.Controller.Navigation;
using SncPucmm.Controller.Tours;
using SncPucmm.Database;
using SncPucmm.Model.Domain;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        int idUsuarioTour;

        List<Button> buttonList;
        List<DetalleUsuarioTour> detalleUsuarioTourList;
        Transform scrollViewItemTemplate;
        
        #endregion

        #region Constructores

        public MenuUsuarioTourSelection(string name, string tourName, UsuarioTour usuarioTour, List<DetalleUsuarioTour> detalleUsuarioTourList)
        {
            this.name = name;
            this.detalleUsuarioTourList = detalleUsuarioTourList;
            this.tourName = tourName;
            this.idUsuarioTour = usuarioTour.idUsuarioTour.Value;

            Initializer();
        }
        
        #endregion

        #region Metodos

        private void Initializer()
        {
            scrollViewItemTemplate = (Resources.Load("GUI/DetalleUsuarioTourItem") as GameObject).transform;

            buttonList = new List<Button>();

            Button btnExit = new Button("ButtonExit");
            btnExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
            buttonList.Add(btnExit);

            Button btnResume = new Button("ButtonResume");
            btnResume.OnTouchEvent += new OnTouchEventHandler(OnTouchResumeButton);
            buttonList.Add(btnResume);

            Button btnReset = new Button("ButtonReset");
            btnReset.OnTouchEvent += new OnTouchEventHandler(OnTouchResetButton);
            buttonList.Add(btnReset);

            UIUtils.FindGUI("MenuUsuarioTourSelection/Label").GetComponent<UILabel>().text = tourName;

            UpdateCreateScrollView();
        }

        private void OnTouchResetButton(object sender, TouchEventArgs e)
        {
            StringBuilder sqlQueryBuilder = new StringBuilder();

            //Poner las fechas de la lista de los detalleUsuarioTourList en blanco            
            foreach (var detalleUsuarioTour in detalleUsuarioTourList)
            {
                ////En la base de datos
                sqlQueryBuilder.Append(
                    "UPDATE DetalleUsuarioTour SET fechaInicio = null, fechaLlegada = null, fechaFin = null " +
                    "WHERE id = " + detalleUsuarioTour.idDetalleUsuarioTour.Value + ";"
                );

                ////En la lista
                detalleUsuarioTour.fechaFin = null;
                detalleUsuarioTour.fechaInicio = null;
                detalleUsuarioTour.fechaLlegada = null;
            }

            using (var service = new SQLiteService())
            {
                service.TransactionalQuery(sqlQueryBuilder.ToString());
            }

            //Poner los hijos actualizados
            UpdateCreateScrollView();
        }

        private void OnTouchResumeButton(object sender, TouchEventArgs e)
        {
            NavigationController controller = ModelPoolManager.GetInstance().GetValue("navigationCtrl") as NavigationController;
            controller.StartTourNavigation(tourName, detalleUsuarioTourList);
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            //Delete ScrollView Childrens
            UIUtils.DestroyChilds("MenuUsuarioTourSelection/ScrollView", true);

            //Remove Menu
            MenuManager.GetInstance().RemoveCurrentMenu();
        }

        private void UpdateCreateScrollView()
        {
            var scrollView = UIUtils.FindGUI("MenuUsuarioTourSelection/ScrollView").transform;

            //Delete ScrollView Childrens
            UIUtils.DestroyChilds("MenuUsuarioTourSelection/ScrollView", true);

            using (var sqlService = new SQLiteService())
            {

                for (int i = 0; i < detalleUsuarioTourList.Count; i++)
                {
                    //Creando el item del Tree View con world coordinates
                    var item = (GameObject.Instantiate(scrollViewItemTemplate.gameObject) as GameObject).transform;

                    item.name = "DetalleUsuarioTourItem" + i;

                    //Agregando relacion de padre (Tree View List) - hijo (item del Tree View List)
                    item.parent = scrollView;

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

                    var sql = "SELECT NOD.nombre FROM PuntoReunionTour PUN, Nodo NOD " +
                            "WHERE PUN.id = " + detalleUsuarioTourList[i].idPuntoReunionTour + " AND PUN.idNodo = NOD.idNodo";

                    using (var result = sqlService.SelectQuery(sql))
                    {
                        if (result.Read())
                        {
                            itemText.text = Convert.ToString(result["nombre"]);
                        }
                    }

                    item.FindChild("checkImg").gameObject.SetActive(false);

                    if (detalleUsuarioTourList[i].fechaLlegada.HasValue || (i == 0 && detalleUsuarioTourList[i].fechaInicio.HasValue))
                    {
                        item.FindChild("checkImg").gameObject.SetActive(true);
                    }
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

        public void Update()
        {
            State.ChangeState(eState.Tour);

            
            using (var sqlService = new SQLiteService())
            {
                //Verificar si el tour se completó
                var tourCtrl = ModelPoolManager.GetInstance().GetValue("tourCtrl") as TourController;

                if (tourCtrl.isEndTour)
                {
                    sqlService.TransactionalQuery(
                        "UPDATE UsuarioTour SET estado = 'finalizado' AND fechaFin = '" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "' WHERE id = " + this.idUsuarioTour
                    );
                }
                else
                {
                    sqlService.TransactionalQuery(
                        "UPDATE UsuarioTour SET estado = 'inconcluso' AND fechaFin = null WHERE id = " + this.idUsuarioTour
                    );
                }

                ModelPoolManager.GetInstance().Remove("tourCtrl");

                this.detalleUsuarioTourList.Clear();
                var sql = "SELECT * FROM DetalleUsuarioTour WHERE idUsuarioTour = " + idUsuarioTour;

                using (var resultDetalleUsuarioTour = sqlService.SelectQuery(sql))
                {
                    while (resultDetalleUsuarioTour.Read())
                    {
                        DateTime? updatedDate = null, startDate = null, endDate = null;
                        DateTime temp;

                        var obj = resultDetalleUsuarioTour["fechaInicio"];
                        if (obj != null)
                        {
                            var fechaInicio = Convert.ToString(obj);
                            if (DateTime.TryParseExact(fechaInicio, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                            {
                                startDate = temp;
                            }
                        }

                        obj = resultDetalleUsuarioTour["fechaLlegada"];
                        if (obj != null)
                        {
                            var fechaFin = Convert.ToString(obj);
                            if (DateTime.TryParseExact(fechaFin, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                            {
                                endDate = temp;
                            }
                        }

                        obj = resultDetalleUsuarioTour["fechaFin"];
                        if (obj != null)
                        {
                            var fechaActualizacion = Convert.ToString(obj);
                            if (DateTime.TryParseExact(fechaActualizacion, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                            {
                                updatedDate = temp;
                            }
                        }

                        this.detalleUsuarioTourList.Add(
                            new DetalleUsuarioTour()
                            {
                                idDetalleUsuarioTour = Convert.ToInt32(resultDetalleUsuarioTour["id"]),
                                idPuntoReunionTour = Convert.ToInt32(resultDetalleUsuarioTour["idPuntoReunionTour"]),
                                idUsuarioTour = idUsuarioTour,
                                fechaInicio = startDate,
                                fechaLlegada = endDate,
                                fechaFin = updatedDate
                            }
                        );
                    }
                }
            }

            UpdateCreateScrollView();
        }

        #endregion

        #endregion

        #region Destructor

        ~MenuUsuarioTourSelection()
        {
            this.name = null;
            this.buttonList = null;
            this.detalleUsuarioTourList = null;
            this.scrollViewItemTemplate = null;
        }

        #endregion

    }
}
