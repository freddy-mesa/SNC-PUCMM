using SncPucmm.Controller.Control;
using SncPucmm.Database;
using SncPucmm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SncPucmm.Controller.GUI
{
    class MenuSendShareLocationRequest : IMenu, IButton, IScrollView
    {
        #region Atributos

        string name;
        List<Button> buttonList;
        List<long> friendList;
        int idNodo;
        ScrollView scrollView;

        #endregion

        #region Constructor

        public MenuSendShareLocationRequest(string name, List<long> shareLocationRequest)
        {
            this.name = name;
            this.friendList = shareLocationRequest;

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

            var buttonSend = new Button("ButtonSend");
            buttonSend.OnTouchEvent += new OnTouchEventHandler(OnTouchSendButton);
            buttonList.Add(buttonSend);

            var ButtonClearText = new Button("ButtonClearText");
            ButtonClearText.OnTouchEvent += new OnTouchEventHandler(OnTouchClearTextButton);
            buttonList.Add(ButtonClearText);

            var ButtonSearchBox = new Button("SearchBox");
            ButtonSearchBox.OnTouchEvent += new OnTouchEventHandler(OnTouchSearchBoxButton);
            buttonList.Add(ButtonSearchBox);

            scrollView = new ScrollView("ScrollView");
            scrollView.OnChangeEvent += new OnChangeEventHandler(OnChangeScrollView);
            scrollView.OnCloseEvent += new OnCloseEventHandler(OnCloseScrollView);
        }

        private void OnTouchSearchBoxButton(object sender, TouchEventArgs e)
        {
            var search = UIUtils.FindGUI(name).transform.FindChild("Search").gameObject;
            search.SetActive(true);

            var searchbox = UIUtils.FindGUI(name + "/SearchBox").transform;
            var scrollViewTranform = UIUtils.FindGUI(name).transform.FindChild("Search").FindChild("ScrollView");
            var template = Resources.Load("GUI/TreeViewScrollItem") as GameObject;

            scrollViewTranform.GetComponent<UIScrollViewControl>().SetTextSearch(searchbox.transform, template);
        }

        private void OnTouchSendButton(object sender, TouchEventArgs e)
        {
            var mensaje = UIUtils.FindGUI(name + "/TextBoxMessage").GetComponent<UIInput>().value;
            WebService.Instance.SendShareLocationRequest(friendList, mensaje, idNodo);

            MenuManager.GetInstance().RemoveCurrentMenu();
            MenuManager.GetInstance().RemoveCurrentMenu();
        }

        private void OnTouchExitButton(object sender, TouchEventArgs e)
        {
            MenuManager.GetInstance().RemoveCurrentMenu();
            MenuManager.GetInstance().RemoveCurrentMenu();
        }

        private void OnTouchClearTextButton(object sender, TouchEventArgs e)
        {
            idNodo = 0;
            UIUtils.FindGUI(name + "/SearchBox").GetComponent<UIInput>().value = "";
            scrollView.OnClose(null);

            UIUtils.FindGUI(name + "/Search").SetActive(false);
        }

        #region ScrollView

        private void OnTouchScrollViewItem(object sender, TouchEventArgs e)
        {
            var button = sender as Button;
            this.idNodo = Convert.ToInt32(button.ObjectTag.GetType().GetProperty("node").GetValue(button.ObjectTag, null));
            var text = Convert.ToString(button.ObjectTag.GetType().GetProperty("nombre").GetValue(button.ObjectTag, null));
            UIUtils.FindGUI(name + "/SearchBox").GetComponent<UIInput>().value = text;

            scrollView.OnClose(null);
            UIUtils.FindGUI(name + "/Search").SetActive(false);
        }

        private void OnChangeScrollView(object sender, ChangeEventArgs e)
        {
            var type = e.Mensaje.GetType();

            var text = Convert.ToString(type.GetProperty("text").GetValue(e.Mensaje, null));
            var parent = (Transform)type.GetProperty("parent").GetValue(e.Mensaje, null);
            var template = (GameObject)type.GetProperty("template").GetValue(e.Mensaje, null);

            this.UpdateScrollView(text, parent, template);
        }

        private void OnCloseScrollView(object sender, CloseEventArgs e)
        {
            UIUtils.DestroyChilds(name + "/Search/ScrollView", true);
            DeleteScrollViewItem();
        }

        private void UpdateScrollView(string searchText, Transform Parent, GameObject Template)
        {
            UIUtils.ActivateCameraLabels(false);

            var textList = new List<object>();

            //Obteniendo de la Base de datos
            using (var sqlService = new SQLiteService())
            {
                var sql = "SELECT nombre, idNodo, edificio " +
                        "FROM Nodo " +
                        "WHERE idUbicacion is not null and nombre not like '%Nodo%' and nombre LIKE '%" + searchText + "%' " +
                        "ORDER BY idUbicacion, idNodo";

                using (var reader = sqlService.SelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        var lugar = new
                        {
                            nombre = Convert.ToString(reader["nombre"]),
                            node = Convert.ToString(reader["idNodo"]),
                            edificio = Convert.ToString(reader["edificio"])
                        };

                        textList.Add(lugar);
                    }
                }
            }

            //Eliminando los hijos del Tree View List
            UIUtils.DestroyChilds(name + "/Search/ScrollView", true);

            //Eliminando los item del tree view de la lista de botones de MenuMain
            DeleteScrollViewItem();

            Parent.GetComponent<UIScrollView>().ResetPosition();
            Parent.GetComponent<UIPanel>().clipOffset = new Vector2(-2, 4.5f);
            Parent.localPosition = new Vector3(-8f, 11f, Parent.localPosition.z);

            string edificioName = string.Empty;

            //Agregando los hijos al Tree View List
            for (int i = 0; i < textList.Count; i++)
            {
                //Creando el item del Tree View con world coordinates
                var item = GameObject.Instantiate(Template) as GameObject;

                item.transform.name = "ScrollViewItem" + i;

                //Agregando relacion de padre (Tree View List) - hijo (item del Tree View List)
                item.transform.parent = Parent;

                //Agregando la posicion relativa del hijo con relacion al padre
                item.transform.localPosition = new Vector3(
                    Template.transform.localPosition.x,
                    Template.transform.localPosition.y - 65f * i,
                    Template.transform.localPosition.z
                );

                //Agregando la escala relativa del hijo con relacion al padre
                item.transform.localScale = Template.transform.localScale;

                //Encontrando texto del un item (su hijo)
                var itemText = item.transform.FindChild("Label").GetComponent<UILabel>();

                var nombre = Convert.ToString(textList[i].GetType().GetProperty("nombre").GetValue(textList[i], null));
                var node = Convert.ToInt32(textList[i].GetType().GetProperty("node").GetValue(textList[i], null));
                int edificio;
                int.TryParse(Convert.ToString(textList[i].GetType().GetProperty("edificio").GetValue(textList[i], null)), out edificio);

                //Si son iguales la localizacion es un nombre de un edificio
                if (edificio != 0)
                {
                    itemText.text = nombre;
                    edificioName = nombre;
                }
                //De lo contrario esta dentro del edificio
                else
                {
                    //Se agrega un padding de 5 espacios
                    itemText.text = edificioName + " - " + nombre;
                }

                var button = new Button(item.name);
                button.OnTouchEvent += new OnTouchEventHandler(OnTouchScrollViewItem);
                button.ObjectTag = new { node, nombre = itemText.text };

                buttonList.Add(button);
                scrollView.ButtonCount++;
            }
        }

        private void DeleteScrollViewItem()
        {
            List<Button> buttonForClear = new List<Button>();

            //Buscando las buttones
            for (int i = 0, k = 0; i < buttonList.Count; ++i)
            {
                if (k == scrollView.ButtonCount)
                {
                    break;
                }

                if (buttonList[i].Name.Equals("ScrollViewItem" + k))
                {
                    buttonForClear.Add(buttonList[i]);
                    k++;
                }
            }

            //Eliminando los botones del Menu Main
            foreach (Button button in buttonForClear)
            {
                buttonList.Remove(button);
            }

            //Borrando la cantidad botones del tree view
            scrollView.ButtonCount = 0;
        }

        #endregion

        #region Implementado

        public string GetMenuName()
        {
            return name;
        }

        public void Update()
        {

        }

        public List<Button> GetButtonList()
        {
            return buttonList;
        }

        public ScrollView GetScrollView()
        {
            return scrollView;
        }

        #endregion

        #endregion
    }
}
