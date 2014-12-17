using SncPucmm.Controller.Control;
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
    class MenuShareLocation : IMenu, IButton
    {
        #region Atributos

        string name;
        List<Button> buttonList;
        ModelNode modelNode;

        #endregion

        #region Constructor

        public MenuShareLocation(string name, object obj)
        {
            this.name = name;
            ProcessRequest(obj);
            Initializer();
        }

        #endregion

        #region Methods

        private void Initializer()
        {
            buttonList = new List<Button>();

            var buttonExit = new Button("ButtonExit");
            buttonExit.OnTouchEvent += new OnTouchEventHandler(OnTouchExitButton);
            buttonList.Add(buttonExit);

            var ButtonNavigation = new Button("ButtonNavigation");
            ButtonNavigation.OnTouchEvent += new OnTouchEventHandler(OnTouchNavigationButton);
            buttonList.Add(ButtonNavigation);
        }

        private void OnTouchNavigationButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().AddMenu(new MenuBuilding("MenuBuilding", modelNode));
            State.ChangeState(eState.MenuBuilding);
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
        }

        private void ProcessRequest(object obj)
        {
            using (var service = new SQLiteService())
            {
                var idNodo = Convert.ToInt32(obj.GetType().GetProperty("nodo").GetValue(obj, null));

                var sql = "SELECT idUbicacion, edificio, nombre " +
                    "FROM Nodo " +
                    "WHERE idNodo = " + idNodo;

                using (var reader = service.SelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        this.modelNode = new ModelNode()
                        {
                            name = Convert.ToString(reader["nombre"]),
                            idNodo = idNodo,
                            idUbicacion = Convert.ToInt32(reader["idUbicacion"])
                        };

                        var edificio = Convert.ToInt32(reader["edificio"]);
                        modelNode.isBuilding = (edificio == modelNode.idUbicacion ? true : false);
                    }
                }
            }

            UIUtils.FindGUI(name + "/TextBoxUbicacion/Label").GetComponent<UILabel>().text = modelNode.name;

            var mensaje = Convert.ToString(obj.GetType().GetProperty("mensaje").GetValue(obj, null));
            UIUtils.FindGUI(name + "/TextBoxMessage/Label").GetComponent<UILabel>().text = mensaje;
        }

        #region Implementados

        public string GetMenuName()
        {
            return name;
        }

        public void Update()
        {
            State.ChangeState(eState.Menu);
        }

        public List<Button> GetButtonList()
        {
            return buttonList;
        }

        #endregion

        #endregion
    }
}
